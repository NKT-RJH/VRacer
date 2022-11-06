using System.Collections.Generic;
using UnityEngine;

public class SoundSetting : DontDestroyOnLoad<SoundSetting>
{
    private List<AudioSource> audioSources;

    protected override void Awake()
    {
        base.Awake();

        audioSources = new List<AudioSource>();

        AudioSource[] audioSourceArray = FindObjectsOfType<AudioSource>();

        for (int count = 0; count < audioSourceArray.Length; count++)
        {
            audioSources.Add(audioSourceArray[count]);
        }

        SetVolume();
    }

    public void SetVolume()
    {
        for (int count = 0; count < audioSources.Count; count++)
        {
            audioSources[count].volume = OptionData.volume / 100f;
        }
    }
}
