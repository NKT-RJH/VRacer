using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CountDown : MonoBehaviour
{
	[Header("Cashing")]
	[SerializeField] private TextMeshProUGUI[] texts;

	[SerializeField] private LockVRCamera lockVRCamera;

	[SerializeField] private AudioClip beep;

	[SerializeField] private Transform cameraPCTransform;
	[SerializeField] private Transform cameraVRTransform;
	[SerializeField] private Transform cameraRigTransform;
	[SerializeField] private Transform[] cameraPaths = new Transform[3];

	[SerializeField] private GameObject[] objectsToTurnOff;

	[Header("Value")]
	[SerializeField] private Vector3 cameraRigPath = new Vector3(-0.375f, 0, 0.11f);
	
	private AudioSource audioSource;

	private bool countDownEnd;

	public bool CountDownEnd { get { return countDownEnd; } }

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		audioSource.clip = beep;

		StartCoroutine(CountDownStart());
	}

	private IEnumerator CountDownStart()
	{
		lockVRCamera.Lock();

		for (int count = 0; count < objectsToTurnOff.Length; count++)
		{
			objectsToTurnOff[count].SetActive(false);
		}

		cameraPCTransform.localPosition = cameraPaths[2].position;
		cameraPCTransform.localRotation = cameraPaths[2].rotation;
		cameraRigTransform.localPosition = cameraPaths[2].position;
		cameraVRTransform.localRotation = cameraPaths[2].rotation;
		yield return new WaitForSeconds(1);

		for (int count = 0; count < texts.Length; count++)
		{
			texts[count].gameObject.SetActive(true);
		}

		for (int count1 = 3; count1 > 0; count1--)
		{
			audioSource.Play();
			cameraPCTransform.localPosition = cameraPaths[count1 - 1].position;
			cameraPCTransform.localRotation = cameraPaths[count1 - 1].rotation;
			cameraRigTransform.localPosition = cameraPaths[count1 - 1].position;
			cameraVRTransform.localRotation = cameraPaths[count1 - 1].rotation;
			for (int count2 = 0; count2 < texts.Length; count2++)
			{
				texts[count2].text = count2.ToString();
			}
			yield return new WaitForSeconds(1.5f);
		}

		cameraRigTransform.localPosition = cameraRigPath;

		audioSource.pitch = 2;
		audioSource.Play();

		for (int count = 0; count < texts.Length; count++)
		{
			texts[count].text = "Go!";
		}

		countDownEnd = true;

		lockVRCamera.UnLock();

		for (int count = 0; count < objectsToTurnOff.Length; count++)
		{
			objectsToTurnOff[count].SetActive(true);
		}

		yield return new WaitForSeconds(1.2f);

		for (int count = 0; count < texts.Length; count++)
		{
			texts[count].gameObject.SetActive(false);
		}
	}
}