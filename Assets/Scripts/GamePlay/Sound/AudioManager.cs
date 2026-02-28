using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public event Action OnInitialized;

    private AudioMixer _mixer;

    private AudioMixerGroup _bgmGroup;
    private AudioMixerGroup _sfxGroup;

    private const string BGM = "BGM";
    private const string SFX = "SFX";

    public async Task Initialize()
    {
        await Manager.Data.WaitForReady();

        _mixer = await AssetLoaderProvider.Loader.LoadAsync<AudioMixer>("AudioMixer");

        _bgmGroup = _mixer.FindMatchingGroups("BGM")[0];
        _sfxGroup = _mixer.FindMatchingGroups("SFX")[0];

        if (!PlayerPrefs.HasKey(BGM)) PlayerPrefs.SetFloat(BGM, 1f);
        if (!PlayerPrefs.HasKey(SFX)) PlayerPrefs.SetFloat(SFX, 1f);

        IsInitialized = true;
        OnInitialized?.Invoke();
    }

    public void ApplySavedVolume()
    {
        SetBGMVolume(PlayerPrefs.GetFloat(BGM));
        SetSFXVolume(PlayerPrefs.GetFloat(SFX));
    }

    public AudioMixerGroup GetBGMGroup() => _bgmGroup;
    public AudioMixerGroup GetSFXGroup() => _sfxGroup;

    public float GetBGMVolume() => PlayerPrefs.GetFloat(BGM);
    public float GetSFXVolume() => PlayerPrefs.GetFloat(SFX);

    public void SetBGMVolume(float value)
    {
        _mixer.SetFloat(BGM, ToDB(value));
        PlayerPrefs.SetFloat(BGM, value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        _mixer.SetFloat(SFX, ToDB(value));
        PlayerPrefs.SetFloat(SFX, value);
        PlayerPrefs.Save();
    }

    private float ToDB(float value)
    {
        return value <= 0.0001f ? -80f : Mathf.Log10(value) * 20f;
    }
}