using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarSound : MonoBehaviour
{
	[Header("Cashing")]
	[SerializeField] private Sounds sounds;
	
	private Car carMove;
	private InputManager inputManager;
	private CountDown countDown;
	private ClearCheck clearCheck;
	private AudioSource audioSource;

	private float brakeTime;
	//private float driftTime;

	private void Awake()
	{
		clearCheck = GetComponent<ClearCheck>();
		carMove = GetComponent<Car>();
		inputManager = FindObjectOfType<InputManager>();
		countDown = FindObjectOfType<CountDown>();
		audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		audioSource.PlayOneShot(sounds.carStart);
	}

	private void Update()
	{
		if (clearCheck.IsClear)
		{
			StopAudioClip();
			return;
		}
		if (!countDown.CountDownEnd) return;

		brakeTime -= Time.deltaTime;

		//if (inputManager.Drift && driftTime <= 0)
		//{
		//	PlayAudioClip(sounds.drift);
		//	AudioSetting(false, 1);
		//	driftTime = sounds.drift.length;
		//}
		//else if (!inputManager.Drift && driftTime > 0)
		//{
		//	StopAudioClip(sounds.drift);
		//	driftTime = 0;
		//}

		//if (driftTime > 0) return;

		if (inputManager.Brake > 0 && brakeTime <= 0)
		{
			PlayAudioClip(sounds.brake);
			AudioSetting(false, 1);
			brakeTime = sounds.brake.length;
		}
		else if (inputManager.Brake <= 0 && brakeTime > 0)
		{
			StopAudioClip(sounds.brake);
			brakeTime = 0;
		}

		if (brakeTime > 0) return;

		if (carMove.RPM <= 500)
		{
			PlayAudioClip(sounds.normal);
			AudioSetting(true, 1);
		}
		else
		{
			StopAudioClip(sounds.normal);
		}

		if (carMove.KPH > 6)
		{
			PlayAudioClip(sounds.idle);
			AudioSetting(true, carMove.RPM / 5000);
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

		audioSource.pitch = Mathf.Clamp(sound, 0.3f, 1.75f);
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
	// ���� �ֱ�!
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