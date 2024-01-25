using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerControl : MonoBehaviour
{
    public AudioMixerGroup audioMixerGroup;
    private AudioMixer audioMixer;
    private PlayerPrefManager playerPrefManager;
    // Start is called before the first frame update
    void Start()
    {
        audioMixer = audioMixerGroup.audioMixer;
        if (!TryGetComponent<PlayerPrefManager>(out playerPrefManager))
        {
            Debug.LogError("Cannot find component PlayerPrefManager in attatched GameObject.");
        }
        else
        {
            SetVolume(PlayerPrefs.GetFloat(playerPrefManager.key));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetVolume(float value)
    {
        audioMixer.SetFloat(audioMixerGroup.name, value);
    }

    void OnSettingsUpdate(float value)
    {
        SetVolume(value);
    }
}
