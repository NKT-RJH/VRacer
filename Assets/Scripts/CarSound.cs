using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{
	public AudioSource audioSource;
	public InputManager inputManager;
	public Car car;

	public Sound[] sounds;

	private void Update()
	{
		/*if (car.KPH > 50)
		{
			PlayAudioClip(sounds[1].audioClip);
			AudioSetting(true, 1);
		}
		else */if (car.KPH > 0)
		{
			PlayAudioClip(sounds[0].audioClip);
			AudioSetting(true, car.KPH / 100);
			
		}
		//switch (inputManager.inputCondition)
		//{
			
		//	case InputCondition.KeyBoard:
		//		if (inputManager.vertical > 0)
		//		{
		//			PlayAudioClip(sounds.idle);
		//		}
		//		else if (inputManager.vertical == 0)
		//		{
		//			StopAudioClip();
		//		}
		//		break;
		//	case InputCondition.Driving:
		//		if (inputManager.gas > 0)
		//		{
		//			PlayAudioClip(sounds.idle);
		//		}
		//		else if (inputManager.steer <= 0)
		//		{
		//			StopAudioClip();
		//		}
		//		break;
		//}
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

	private void StopAudioClip()
	{
		audioSource.Stop();
		audioSource.clip = null;
	}
	// 사운드 넣기!
}

[System.Serializable]
public class Sound
{
	public string name;
	public AudioClip audioClip;
}