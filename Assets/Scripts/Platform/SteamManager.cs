using Steamworks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SteamManager : Singleton<SteamManager>
{
    private const string _leaderboardName = "High_Score";

    private SteamLeaderboard_t _leaderboard;
    private bool _initializing = false;
    private bool _steamAvailable = false;

    private CallResult<LeaderboardFindResult_t> _findResult;
    private CallResult<LeaderboardScoreUploaded_t> _uploadResult;
    private CallResult<LeaderboardScoresDownloaded_t> _downloadResult;

    private TaskCompletionSource<bool> _initTcs;
    private TaskCompletionSource<bool> _uploadTcs;
    private TaskCompletionSource<List<LeaderboardEntry>> _downloadTcs;

    private void Awake()
    {
        try
        {
            _steamAvailable = SteamAPI.Init();
        }
        catch (Exception e)
        {
            Debug.LogWarning("Steam Init Failed : " + e.Message);
            _steamAvailable = false;
        }

        if (!_steamAvailable)
        {
            Debug.LogWarning("Steam is not available.");
            return;
        }

        _findResult = CallResult<LeaderboardFindResult_t>.Create();
        _uploadResult = CallResult<LeaderboardScoreUploaded_t>.Create();
        _downloadResult = CallResult<LeaderboardScoresDownloaded_t>.Create();
    }

    private void Update()
    {
        SteamAPI.RunCallbacks();
    }

    #region Init

    public async Task Initialize()
    {
        if (!_steamAvailable)
            return;

        if (IsInitialized)
            return;

        if (_initializing)
        {
            await _initTcs.Task;
            return;
        }

        _initializing = true;
        _initTcs = new TaskCompletionSource<bool>();

        SteamAPICall_t handle = SteamUserStats.FindLeaderboard(_leaderboardName);
        _findResult.Set(handle, OnLeaderboardFound);

        await _initTcs.Task;
    }

    private void OnLeaderboardFound(LeaderboardFindResult_t result, bool ioFailure)
    {
        if (ioFailure || result.m_bLeaderboardFound == 0)
        {
            _initTcs.TrySetException(new Exception("Leaderboard not found."));
            return;
        }

        _leaderboard = result.m_hSteamLeaderboard;

        IsInitialized = true;
        _initializing = false;

        _initTcs.TrySetResult(true);
    }

    #endregion

    #region Upload Score

    public async Task UploadScoreAsync(int score)
    {
        if (!_steamAvailable)
            return;

        await Initialize();

        _uploadTcs = new TaskCompletionSource<bool>();

        SteamAPICall_t handle = SteamUserStats.UploadLeaderboardScore(
            _leaderboard,
            ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest,
            score,
            null,
            0);

        _uploadResult.Set(handle, OnScoreUploaded);

        await _uploadTcs.Task;
    }

    private void OnScoreUploaded(LeaderboardScoreUploaded_t result, bool ioFailure)
    {
        if (!_steamAvailable)
            return;

        if (ioFailure || result.m_bSuccess == 0)
        {
            _uploadTcs.TrySetException(new Exception("Score upload failed."));
            return;
        }

        _uploadTcs.TrySetResult(true);
    }

    #endregion

    #region Download Top Scores

    public async Task<List<LeaderboardEntry>> DownloadTopScoresAsync(int topN = 10)
    {
        await Initialize();

        _downloadTcs = new TaskCompletionSource<List<LeaderboardEntry>>();

        SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(
            _leaderboard,
            ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal,
            1,
            topN);

        _downloadResult.Set(handle, OnScoresDownloaded);

        return await _downloadTcs.Task;
    }

    private async void OnScoresDownloaded(LeaderboardScoresDownloaded_t result, bool ioFailure)
    {
        if (ioFailure)
        {
            _downloadTcs.TrySetException(new Exception("Download failed."));
            return;
        }

        List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
        List<CSteamID> steamIDs = new List<CSteamID>();

        for (int i = 0; i < result.m_cEntryCount; i++)
        {
            LeaderboardEntry_t entry;
            SteamUserStats.GetDownloadedLeaderboardEntry(
                result.m_hSteamLeaderboardEntries,
                i,
                out entry,
                null,
                0);

            steamIDs.Add(entry.m_steamIDUser);

            entries.Add(new LeaderboardEntry
            {
                Rank = entry.m_nGlobalRank,
                Score = entry.m_nScore,
                SteamID = entry.m_steamIDUser
            });
        }

        foreach (var id in steamIDs)
        {
            SteamFriends.RequestUserInformation(id, true);
        }

        await Task.Delay(200);

        foreach (var entry in entries)
        {
            entry.PersonalName = SteamFriends.GetFriendPersonaName(entry.SteamID);
        }

        _downloadTcs.TrySetResult(entries);
    }

    public async Task<LeaderboardEntry> DownloadMyScoreAsync()
    {
        await Initialize();

        var tcs = new TaskCompletionSource<LeaderboardEntry>();

        SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(
            _leaderboard,
            ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser,
            -5,
            5);

        CallResult<LeaderboardScoresDownloaded_t> call =
            CallResult<LeaderboardScoresDownloaded_t>.Create();

        call.Set(handle, (result, failure) =>
        {
            if (failure)
            {
                tcs.SetException(new Exception("Failed to load my score."));
                return;
            }

            CSteamID myID = SteamUser.GetSteamID();

            for (int i = 0; i < result.m_cEntryCount; i++)
            {
                LeaderboardEntry_t entry;

                SteamUserStats.GetDownloadedLeaderboardEntry(
                    result.m_hSteamLeaderboardEntries,
                    i,
                    out entry,
                    null,
                    0);

                if (entry.m_steamIDUser == myID)
                {
                    tcs.SetResult(new LeaderboardEntry
                    {
                        Rank = entry.m_nGlobalRank,
                        Score = entry.m_nScore,
                        SteamID = entry.m_steamIDUser,
                        PersonalName = SteamFriends.GetFriendPersonaName(entry.m_steamIDUser)
                    });
                    return;
                }
            }

            tcs.SetResult(null);
        });

        return await tcs.Task;
    }

    #endregion

    public void Achieve(string apiName)
    {
        if (!_steamAvailable || !IsInitialized)
            return;

        if (Manager.Steam.IsInitialized)
        {
            SteamUserStats.GetAchievement(apiName, out bool isAchieved);

            if (!isAchieved)
            {
                SteamUserStats.SetAchievement(apiName);
                SteamUserStats.StoreStats();
            }
        }
    }
}
public class LeaderboardEntry
{
    public int Rank;
    public int Score;
    public CSteamID SteamID;
    public string PersonalName;
}