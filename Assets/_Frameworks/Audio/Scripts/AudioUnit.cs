using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

using UnityEngine;

public enum SoundType
{
    BGM,
    SFX
}


public class AudioUnit : MonoBehaviour
{
    private AudioSource audioSource;

    public SoundType type;

    void Start()
    {
        if(TryGetComponent(out AudioSource source))
        {
            audioSource = source;
        }
        else
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (SoundType.SFX == type)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }


        var amg = AudioMixerController.Instance.mixer.FindMatchingGroups("Master");
        audioSource.outputAudioMixerGroup = amg[(int)type+1];
    }

    public void PlaySFX()
    {
        var clip = audioSource.clip;
        audioSource.PlayOneShot(clip);
    }

    public void PlaySFX(SFX sfx)
    {
        var clip = AudioMixerController.Instance.GetSFXClip(sfx);
        audioSource.PlayOneShot(clip);
    }
}
