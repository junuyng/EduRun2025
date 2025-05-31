using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum BGM
{
    Lobby,
    Runner,
}

public enum SFX
{
    Button,
    ButtonHigh,
    Hit,
    ButtonLow,
    Wrong,
    Correct,
    Jump

}

public class AudioMixerController : MonoSingleton<AudioMixerController>
{
    private const string AUDIO_PATH = "Audio";

    public AudioMixer mixer;

    private Dictionary<BGM, AudioClip> bgmPlayer = new Dictionary<BGM, AudioClip>();
    private Dictionary<SFX, AudioClip> sfxPlayer = new Dictionary<SFX, AudioClip>();

    private AudioSource curBgm;

    protected override void Init()
    {
        base.Init();

        mixer = Resources.Load($"{AUDIO_PATH}/Mixer", typeof(AudioMixer)) as AudioMixer;
        curBgm = GetComponent<AudioSource>();
        LoadBGMPlayer();
        LoadSFXPlayer();
 
    }

private void LoadBGMPlayer()
    {
        foreach (var bgm in Enum.GetValues(typeof(BGM)))
        {
            var audioName = bgm.ToString();
            var pathStr = $"{AUDIO_PATH}/BGM/{audioName}";
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip;
            if (!audioClip)
            {
                Logger.ErrorLog($"{audioName} clip does not exist.");
            }

            bgmPlayer[(BGM)bgm] = audioClip;
        }
    }

    private void LoadSFXPlayer()
    {
        foreach (var sfx in Enum.GetValues(typeof(SFX)))
        {
            var audioName = sfx.ToString();
            var pathStr = $"{AUDIO_PATH}/SFX/{audioName}";
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip;
            if (!audioClip)
            {
                Logger.ErrorLog($"{audioName} clip does not exist.");
            }

            sfxPlayer[(SFX)sfx] = audioClip;
        }
    }

    public void PlayBGM(BGM bgm)
    {
        if (curBgm.isPlaying)
        {
            curBgm.Stop();
        }

        if (!bgmPlayer.ContainsKey(bgm))
        {
            Logger.ErrorLog($"Invalid clip name. {bgm}");
            return;
        }

        curBgm.clip = bgmPlayer[bgm];
        curBgm.Play();
    }

    public void PauseBGM()
    {
        if (curBgm.isPlaying) curBgm.Pause();
    }

    public void ResumeBGM()
    {
        if (curBgm.isPlaying) curBgm.UnPause();
    }

    public void StopBGM()
    {
        if (curBgm.isPlaying) curBgm.Stop();
    }

    public AudioClip GetBGMClip(BGM bgm)
    {
        if (!bgmPlayer.ContainsKey(bgm))
        {
            Logger.ErrorLog($"Invalid clip name. {bgm}");
            return null;
        }
        else
        {
            return bgmPlayer[bgm];
        }
    }

    public AudioClip GetSFXClip(SFX sfx)
    {
        if (!sfxPlayer.ContainsKey(sfx))
        {
            Logger.ErrorLog($"Invalid clip name. {sfx}");
            return null;
        }
        else
        {
            return sfxPlayer[sfx];
        }
    }

    public void SetMasterVolume(float volume)
    {
        float clamped = Mathf.Clamp(volume, 0.0001f, 1f); // 0일 때 -∞ 방지
        float dB = Mathf.Log10(clamped) * 20;
        mixer.SetFloat("Master", dB);
        Debug.Log($"[Audio] SetMasterVolume: volume={volume}, clamped={clamped}, dB={dB}");
    }

    public void SetBGMVolume(float volume)
    {
        float clamped = Mathf.Clamp(volume, 0.0001f, 1f);
        float dB = Mathf.Log10(clamped) * 20;
        mixer.SetFloat("BGM", dB);
        Debug.Log($"[Audio] SetBGMVolume: volume={volume}, clamped={clamped}, dB={dB}");
    }

    public void SetSFXVolume(float volume)
    {
        float clamped = Mathf.Clamp(volume, 0.0001f, 1f);
        float dB = Mathf.Log10(clamped) * 20;
        mixer.SetFloat("SFX", dB);
        Debug.Log($"[Audio] SetSFXVolume: volume={volume}, clamped={clamped}, dB={dB}");
    }

}
