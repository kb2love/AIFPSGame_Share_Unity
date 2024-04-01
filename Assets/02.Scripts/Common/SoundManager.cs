using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager soundInst; 

    private void Awake()
    {
        if (soundInst == null)
        {
            soundInst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip, AudioSource source)
    {
        source.clip = clip;
        source.Play();
    }
    public void PlayeOneShot(AudioClip clip, AudioSource source)
    {
        source.clip = clip;
        source.PlayOneShot(clip, 1.0f);
    }
    public void StopSound(AudioSource source)
    {
        source.Stop();
    }
}
