using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{
	public GameManager gameManager;

	public AudioSource audioSource;
	public InputManager inputManager;
	public Car car;

	public Sounds sounds;

	private bool brakeFlag;
	private bool driftFlag;

	private void Start()
	{
        audioSource.PlayOneShot(sounds.carStart);
    }

	private void Update()
	{
		if (!gameManager.gameStart) return;

		//if (inputManager.drift)
		//{
		//	if (!driftFlag)
		//	{
		//		audioSource.Stop();
		//		audioSource.PlayOneShot(sounds.brake);
		//		AudioSetting(false, 1);
		//		driftFlag = true;
		//	}
		//}
		//else
		//{
		//	driftFlag = false;
		//}

		//if (driftFlag) return;

		if (inputManager.brake <= 0)
		{
			brakeFlag = false;
		}
		if (inputManager.brake > 0 && !brakeFlag)
		{
			audioSource.Stop();
			audioSource.PlayOneShot(sounds.brake);
			AudioSetting(false, 1);
			brakeFlag = true;
		}

		if (brakeFlag) return;

		if (car.KPH <= 6)
		{
			PlayAudioClip(sounds.normal);
			AudioSetting(true, 1);
		}
		else
		{
			StopAudioClip(sounds.normal);
		}

		if (car.KPH > 6)
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
	public AudioClip carStart;
	public AudioClip normal;
	public AudioClip idle;
	public AudioClip brake;
	public AudioClip drift;
	public AudioClip boom;
}