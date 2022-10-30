using UnityEngine;

public class CarSound : MonoBehaviour
{
	private GameManager gameManager;

	public AudioSource audioSource;
	public InputManager inputManager;
	public Car car;

	public Sounds sounds;

	private float brakeTime;
	private float driftTime;

	private void Awake()
	{
		gameManager = FindObjectOfType<GameManager>();
	}

	private void Start()
	{
		audioSource.PlayOneShot(sounds.carStart);
	}

	private void Update()
	{
		if (!gameManager.gameStart) return;

		brakeTime -= Time.deltaTime;

		if (inputManager.drift && driftTime <= 0)
		{
			PlayAudioClip(sounds.drift);
			AudioSetting(false, 1);
			driftTime = sounds.drift.length;
		}
		else if (!inputManager.drift && driftTime > 0)
		{
			StopAudioClip(sounds.drift);
			driftTime = 0;
		}

		if (driftTime > 0) return;

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