using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossfading : MonoBehaviour
{
    public AudioClip audioClip;
    public float offsetTime = 1f;
    public float volume = 0.2f;
    public float pitch = 1f;
    private AudioSource audioSource1;
    private AudioSource audioSource2;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioSource1 = audioSources[0];
        audioSource2 = audioSources[1];

        audioSource1.clip = audioClip;
        audioSource2.clip = audioClip;

        audioSource1.volume = volume;
        audioSource2.volume = volume;
        
        audioSource1.pitch = pitch;
        audioSource2.pitch = pitch;

        audioSource1.loop = true;
        audioSource2.loop = true;

    }

    // Update is called once per frame
    void Update()
    {
        audioSource1.volume = volume;
        audioSource2.volume = volume;

        audioSource1.pitch = pitch;
        audioSource2.pitch = pitch;
    }

    public void Play()
    {
        audioSource1.Play();
        Invoke(nameof(PlaySecondClip), offsetTime);
    }

    void PlaySecondClip()
    {
        audioSource2.Play();
    }

    public IEnumerator FadeOut(float duration)
    {
        float time = 0;
        float initalVolume = volume;
        while (time < duration)
        {
            volume = Mathf.Lerp(initalVolume, 0f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        audioSource1.volume = 0;
        audioSource2.volume = 0;
    }
}
