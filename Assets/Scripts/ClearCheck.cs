using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ClearCheck : MonoBehaviour
{
	[Header("Cashing")]
	[SerializeField] private Transform clearPath;
	[SerializeField] private GameObject[] clearText = new GameObject[2];
	[SerializeField] private AudioClip clearSound;
	[SerializeField] private TextMeshProUGUI[] clearCountText = new TextMeshProUGUI[2];
	[SerializeField] private Transform cameraPCTransform;
	[SerializeField] private Transform cameraRigTransform;

	private bool isClear;
	public bool IsClear { get { return isClear; } }

	private const int maxClear = 2;

	private int clearCount = 0;

	private AudioSource audioSource;

	private bool[] clears;

	private void Awake()
	{
		GameObject audioObject = new GameObject();
		Instantiate(audioObject, transform);
		audioObject.AddComponent<AudioSource>();
		audioSource = audioObject.GetComponent<AudioSource>();

		audioSource.clip = clearSound;
	}

	private void Start()
	{
		GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");

		int checkPointLength = 0;

		for (int count = 0; count < checkPoints.Length; count++)
		{
			int temporary = int.Parse(checkPoints[count].name);

			if (temporary > checkPointLength)
			{
				checkPointLength = temporary;
			}
		}

		clears = new bool[checkPointLength + 1];
	}

	private void Update()
	{
		for (int count = 0; count < clearCountText.Length; count++)
		{
			clearCountText[count].text = string.Format("{0}/{1}", clearCount, maxClear);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("CheckPoint")) return;

		int number = int.Parse(other.gameObject.name);

		clears[number] = true;

		if (number != 0) return;

		int counter = 0;

		for (int count = 0; count < clears.Length; count++)
		{
			if (clears[count])
			{
				counter++;
			}
		}

		if (counter == clears.Length)
		{
			audioSource.Play();

			for (int count = 0; count < clears.Length; count++)
			{
				clears[count] = false;
			}

			clearCount++;
		}

		if (clearCount == 2)
		{
			StartCoroutine(Clear());
		}
	}

	private IEnumerator Clear()
	{
		isClear = true;

		LockVRCamera lockVRCamera = FindObjectOfType<LockVRCamera>();

		GetComponentInChildren<RotateByMouse>().enabled = false;

		lockVRCamera.Lock();
		cameraPCTransform.localPosition = clearPath.localPosition;
		cameraPCTransform.localRotation = clearPath.localRotation;
		cameraRigTransform.localPosition = clearPath.localPosition;
		lockVRCamera.SetRotation(clearPath.eulerAngles);

		for (int count = 0; count < clearText.Length; count++)
		{
			clearText[count].SetActive(true);
		}

		audioSource.pitch = 2;
		audioSource.Play();

		yield return new WaitForSeconds(5);

		FindObjectOfType<GameManager>().GoTitle();
	}
}
