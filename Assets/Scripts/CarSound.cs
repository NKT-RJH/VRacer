using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{
	public AudioSource audioSource;
	public InputManager inputManager;
	public Car car;

	public Sounds sounds;

	private bool brakeFlag;

	private void Update()
	{
		if (car.KPH <= 6 && !brakeFlag)
		{
			PlayAudioClip(sounds.normal);
			AudioSetting(true, 1);
		}
		else
		{
			StopAudioClip(sounds.normal);
		}

		if (car.KPH > 6 && !brakeFlag)
		{
			PlayAudioClip(sounds.idle);
			AudioSetting(true, car.KPH / 150);
		}
		else
		{
			if (!brakeFlag)
			{
				StopAudioClip(sounds.idle);
			}
		}

		if (inputManager.brake <= 0)
		{
			brakeFlag = false;
		}
		if (inputManager.brake > 1 && !brakeFlag)
		{
			//audioSource.Stop();
			PlayAudioClip(sounds.brake);
			AudioSetting(false, 1);
		}
	}

	private void PlayAudioClip(AudioClip audioClip)
	{
		if (audioSource.clip == null)
		{
			audioSource.clip = audioClip;
			audioSource.Play();
		}
		else
		{
			if (!audioSource.clip.Equals(audioClip))
			{
				audioSource.Stop();
				audioSource.clip = audioClip;
				audioSource.Play();
			}
		}
	}

	private void AudioSetting(bool isLoop, float sound)
	{
		if (audioSource.clip == null) return;

		audioSource.loop = isLoop;

		audioSource.pitch = Mathf.Clamp(sound, 0.3f, 1.5f);
	}

	private void StopAudioClip(AudioClip audioClip)
	{
		if (audioSource.clip != audioClip) return;
		audioSource.Stop();
		audioSource.clip = null;
	}
	// 사운드 넣기!
}

[System.Serializable]
public class Sounds
{
	public AudioClip normal;
	public AudioClip idle;
	public AudioClip brake;
	public AudioClip drift;
	public AudioClip boom;
}