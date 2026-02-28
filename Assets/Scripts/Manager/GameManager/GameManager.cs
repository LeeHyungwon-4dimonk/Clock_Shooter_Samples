using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int Direction { get; private set; } = 8;
    public int Score { get; private set; }
    public bool isGamePaused { get; private set; } = true;
    public event Action IsGamePaused;
    public event Action OnGameOver;
    public TurnStack turnStack { get; private set; }
    public MonsterPositionManager monsterPositionManager { get; private set; }

    public int MonsterEnhanceRate { get; private set; } = 0;

    public GameObject Player { get; private set; }
    public GameObject Boss { get; private set; }

    private const int CriteriaTurn = 1500;
    private const int BonusScore = 1000;
    private int _requireKilledMonsterNum = 300;
    private int _killedMonsterNum = 0;
    public int KilledMonsterNum => _killedMonsterNum;

    public async Task Initialize()
    {
        await WaitForSceneObjects();

        InitializeGameManager();
        IsInitialized = true;
        Debug.Log("Initialized GameManager");
    }

    private async Task WaitForSceneObjects()
    {
        while (GameObject.FindGameObjectWithTag("Player") == null)
        {
            await Task.Yield();
        }

        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void InitializeGameManager()
    {
        turnStack = new TurnStack();
        monsterPositionManager = new MonsterPositionManager();

        turnStack.Initialize();
        monsterPositionManager.Initialize();

        MonsterEnhanceRate = 0;
        _killedMonsterNum = 0;
        isGamePaused = false;
        IsGamePaused?.Invoke();
    }

    public void GameInitialize()
    {
        turnStack.Initialize();
        monsterPositionManager.Initialize();
        GamePause(false);
        OnGameOver?.Invoke();
    }

    public void AddKilledMonster()
    {
        _killedMonsterNum++;
    }

    #region Score

    /*
    private void SaveScore()
    {
        int currentScore = CalculateScore();
        int savedScore = PlayerPrefs.GetInt("HighScore", 0);
        PlayerPrefs.SetInt("HighScore", Math.Max(savedScore, currentScore));
    }

    private int CalculateScore()
    {
        int HP = Manager.Status.PlyStats.CurrentHP;

        if (HP > 0)
            return Score = (CriteriaTurn - turnStack.TotalTurn) + (_killedMonsterNum * 10)
                + (HP * 20) + BonusScore;
        else
            return Score = (int)((CriteriaTurn - turnStack.TotalTurn) * 0.1f)
                + (_killedMonsterNum * 10);
    }
    */

    #endregion

    #region Get Data

    public float GetMonsterRate()
    {
        MonsterEnhanceRate = _killedMonsterNum / 25;
        return MonsterEnhanceRate * 0.125f;
    }

    public float GetProgressRate()
    {
        return (float)_killedMonsterNum / _requireKilledMonsterNum;
    }

    #endregion

    public void GamePause(bool gamePause)
    {
        isGamePaused = gamePause;
        IsGamePaused?.Invoke();
    }
}