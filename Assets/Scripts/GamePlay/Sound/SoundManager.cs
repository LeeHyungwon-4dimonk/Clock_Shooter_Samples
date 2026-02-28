using System.Collections;
using System.Threading.Tasks;
using Unity.VectorGraphics.Editor;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource _sfxAudioSource;
    private AudioSource _bgmAudioSource;

    private FireSoundData _fireSounds;
    private BGMSoundData _bgmSounds;
    private SFXSoundData _sfxSounds;

    private Coroutine _bgmFadeCoroutine;

    public async Task Initialize()
    {
        await Manager.Audio.WaitForReady();
        await Manager.Data.WaitForReady();

        _sfxAudioSource = gameObject.AddComponent<AudioSource>();
        _bgmAudioSource = gameObject.AddComponent<AudioSource>();

        _sfxAudioSource.outputAudioMixerGroup = Manager.Audio.GetSFXGroup();
        _bgmAudioSource.outputAudioMixerGroup = Manager.Audio.GetBGMGroup();

        _fireSounds = Manager.Data.Get<FireSoundData>();
        _bgmSounds = Manager.Data.Get<BGMSoundData>();
        _sfxSounds = Manager.Data.Get<SFXSoundData>();

        Manager.Audio.ApplySavedVolume();

        IsInitialized = true;
    }

    public void PlayShootSound(FireResultType type)
    {
        var entry = _fireSounds.Get(type);
        if (entry == null) return;

        _sfxAudioSource.PlayOneShot(entry.clip, entry.volume);
    }

    public void PlaySFX(SFXType type)
    {
        var entry = _sfxSounds.Get(type);
        if (entry == null) return;

        _sfxAudioSource.PlayOneShot(entry.Clip);
    }

    public void StopSFX()
    {
        _sfxAudioSource.Stop();
    }

    public void PlayBGM(ConditionType type, float fadeTime = 0f)
    {
        var entry = _bgmSounds.Get(type);
        if (entry == null) return;

        if (_bgmFadeCoroutine != null)
            StopCoroutine(_bgmFadeCoroutine);

        _bgmFadeCoroutine = StartCoroutine(FadeBGM(entry.clip, fadeTime));
    }

    public void StopBGM(float fadeTime = 0f)
    {
        if (_bgmFadeCoroutine != null)
            StopCoroutine(_bgmFadeCoroutine);

        _bgmFadeCoroutine = StartCoroutine(FadeOutBGM(fadeTime));
    }

    private IEnumerator FadeBGM(AudioClip newClip, float fadeTime)
    {
        if (_bgmAudioSource.isPlaying)
        {
            yield return FadeOutBGM(fadeTime);
        }

        _bgmAudioSource.clip = newClip;
        _bgmAudioSource.volume = 0f;
        _bgmAudioSource.loop = true;
        _bgmAudioSource.Play();

        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            _bgmAudioSource.volume = Mathf.Lerp(0f, 1f, t / fadeTime);
            yield return null;
        }

        _bgmAudioSource.volume = 1f;
    }

    private IEnumerator FadeOutBGM(float fadeTime)
    {
        float startVolume = _bgmAudioSource.volume;
        float t = 0f;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            _bgmAudioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }

        _bgmAudioSource.volume = 0f;
        _bgmAudioSource.Stop();
    }
}