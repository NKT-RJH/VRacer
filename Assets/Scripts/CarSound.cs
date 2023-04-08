using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarSound : MonoBehaviour
{
	[Header("Cashing")]
	[SerializeField] private Sounds sounds;

	private Car car;
	private InputManager inputManager;
	private CountDown countDown;
	private ClearCheck clearCheck;
	private AudioSource audioSource;

	private float brakeTime;

	private void Awake()
	{
		try { clearCheck = GetComponent<ClearCheck>(); }
		catch (NullReferenceException) { clearCheck= null; }
		car = GetComponent<Car>();
		inputManager = FindObjectOfType<InputManager>();
		try { countDown = FindObjectOfType<CountDown>(); }
		catch (NullReferenceException) { countDown = null; }
		audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		audioSource.PlayOneShot(sounds.carStart);
	}

	private void Update()
	{
		if (clearCheck != null)
		{
			if (clearCheck.isClear)
			{
				StopAudioClip();
				return;
			}
		}
		if (countDown != null)
		{
			if (!countDown.CountDownEnd) return;
		}

		brakeTime -= Time.deltaTime;

		if (inputManager.brake > 0 && brakeTime <= 0)
		{
			PlayAudioClip(sounds.brake);
			AudioSetting(false, 1);
			brakeTime = sounds.brake.length;
		}
		else if (inputManager.brake <= 0 && brakeTime > 0)
		{
			StopAudioClip(sounds.brake);
			brakeTime = 0;
		}

		if (brakeTime > 0) return;

		if (car.rpm <= 500)
		{
			PlayAudioClip(sounds.normal);
			AudioSetting(true, 1);
		}
		else
		{
			StopAudioClip(sounds.normal);
			PlayAudioClip(sounds.idle);
			AudioSetting(true, car.rpm / 8000);
		}
	}

	private void PlayAudioClip(AudioClip audioClip)
	{
		if (audioSource.clip == null)
		{
			audioSource.clip = audioClip;
			audioSource.Play();
			return;
		}

		if (!audioSource.clip.Equals(audioClip))
		{
			audioSource.Stop();
			audioSource.clip = audioClip;
			audioSource.Play();
		}
	}

	private void AudioSetting(bool isLoop, float sound)
	{
		if (audioSource.clip == null) return;

		audioSource.loop = isLoop;

		audioSource.pitch = Mathf.Clamp(sound, 0.3f, 1.3f);
	}

	private void StopAudioClip()
	{
		audioSource.Stop();
		audioSource.clip = null;
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
}