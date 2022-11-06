using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CountDown : MonoBehaviour
{
	[Header("Cashing")]
	[SerializeField] private TextMeshProUGUI[] textMeshProUGUIs = new TextMeshProUGUI[2];


	[SerializeField] private AudioClip beep;

	[SerializeField] private Transform cameraPCTransform;
	[SerializeField] private Transform cameraVRTransform;
	[SerializeField] private Transform cameraRigTransform;

	[SerializeField] private GameObject[] anotherUI;

	[Header("Value")]
	[SerializeField] private Vector3 cameraRigPath = new Vector3(-0.375f, 0, 0.11f);

	private LockVRCamera lockVRCamera;
	private AudioSource audioSource;

	private bool countDownEnd;

	public bool CountDownEnd { get { return countDownEnd; } }

	private void Awake()
	{
		lockVRCamera = FindObjectOfType<LockVRCamera>();
		audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		audioSource.clip = beep;

		StartCoroutine(CountDownStart());
	}

	private IEnumerator CountDownStart()
	{
		Transform countDownPathTransform = GameObject.Find("CountDownPath").transform;
		Transform[] cameraPaths = new Transform[3];

		for (int count = 0; count < countDownPathTransform.childCount; count++)
		{
			cameraPaths[count] = countDownPathTransform.GetChild(count);
		}

		lockVRCamera.Lock();

		for (int count = 0; count < anotherUI.Length; count++)
		{
			anotherUI[count].SetActive(false);
		}

		cameraPCTransform.localPosition = cameraPaths[0].position;
		cameraPCTransform.localRotation = cameraPaths[0].rotation;
		cameraRigTransform.localPosition = cameraPaths[0].position;
		cameraVRTransform.localRotation = cameraPaths[0].rotation;

		lockVRCamera.SetRotation(cameraVRTransform.eulerAngles);

		yield return new WaitForSeconds(1);

		for (int count = 0; count < textMeshProUGUIs.Length; count++)
		{
			textMeshProUGUIs[count].gameObject.SetActive(true);
		}

		for (int count1 = 0; count1 < 3; count1++)
		{
			audioSource.Play();
			
			cameraPCTransform.localPosition = cameraPaths[count1].position;
			cameraPCTransform.localRotation = cameraPaths[count1].rotation;
			cameraRigTransform.localPosition = cameraPaths[count1].position;
			cameraVRTransform.localRotation = cameraPaths[count1].rotation;
			
			for (int count2 = 0; count2 < textMeshProUGUIs.Length; count2++)
			{
				textMeshProUGUIs[count2].text = string.Format("{0}", 3 - count1);
			}
			
			lockVRCamera.SetRotation(cameraVRTransform.eulerAngles);
			yield return new WaitForSeconds(1.5f);
		}

		cameraRigTransform.localPosition = cameraRigPath;

		audioSource.pitch = 2;
		audioSource.Play();

		for (int count = 0; count < textMeshProUGUIs.Length; count++)
		{
			textMeshProUGUIs[count].text = "Go!";
		}

		countDownEnd = true;

		lockVRCamera.UnLock();

		for (int count = 0; count < anotherUI.Length; count++)
		{
			anotherUI[count].SetActive(true);
		}

		yield return new WaitForSeconds(1.2f);

		for (int count = 0; count < textMeshProUGUIs.Length; count++)
		{
			textMeshProUGUIs[count].gameObject.SetActive(false);
		}
	}
}