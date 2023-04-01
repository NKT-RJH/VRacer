using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundSetting : Singleton<SoundSetting>
{
	private List<AudioSource> audioSources;

	protected override void Awake()
	{
		base.Awake();

		SettingList();

		SetVolume();
	}

	//private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	//{
	//	SettingList();

	//	SetVolume();
	//}

	private void SettingList()
	{
		audioSources = new List<AudioSource>();

		AudioSource[] audioSourceArray = FindObjectsOfType<AudioSource>();

		for (int count = 0; count < audioSourceArray.Length; count++)
		{
			audioSources.Add(audioSourceArray[count]);
		}
	}

	public void SetVolume()
	{
		for (int count = 0; count < audioSources.Count; count++)
		{
			audioSources[count].volume = OptionData.volume / 100f;
		}
	}
}
