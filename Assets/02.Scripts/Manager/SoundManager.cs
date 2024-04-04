using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SoundManager : MonoBehaviour
{
    public static SoundManager soundInst; 

    private void Awake()
    {
        if (soundInst == null)
        {
            soundInst = this;
        }
        else if(soundInst != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
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
