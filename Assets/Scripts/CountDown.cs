using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class CountDown : MonoBehaviour
{
	public GameManager gameManager;
    public TextMeshProUGUI textPC;
	public TextMeshProUGUI textVR;

	public LockVRCamera lockVRCamera;

    public AudioClip beep;

    public Transform cameraPCTransform;
	public Transform cameraVRTransform;
	public Transform cameraRigTransform;
    public Transform[] cameraPaths = new Transform[3];

	public GameObject[] objectsToTurnOff;

    private AudioSource audioSource;

	private Vector3 cameraRigPath = new Vector3(-0.375f, 0, 0.11f);

	public bool countDownEnd;

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
		lockVRCamera.cameraLock = true;

		for (int count = 0; count < objectsToTurnOff.Length; count++)
		{
			objectsToTurnOff[count].SetActive(false);
		}

        cameraPCTransform.localPosition = cameraPaths[2].position;
        cameraPCTransform.localRotation = cameraPaths[2].rotation;
		cameraRigTransform.localPosition = cameraPaths[2].position;
		cameraVRTransform.localRotation = cameraPaths[2].rotation;
		yield return new WaitForSeconds(1);

        textPC.gameObject.SetActive(true);
        textVR.gameObject.SetActive(true);

        for (int count = 3; count > 0; count--)
        {
            audioSource.Play();
            cameraPCTransform.localPosition = cameraPaths[count - 1].position;
            cameraPCTransform.localRotation = cameraPaths[count - 1].rotation;
			cameraRigTransform.localPosition = cameraPaths[count - 1].position;
			cameraVRTransform.localRotation = cameraPaths[count - 1].rotation;
            textPC.text = count.ToString();
            textVR.text = count.ToString();
            yield return new WaitForSeconds(1.5f);
        }

		cameraRigTransform.position = cameraRigPath;

        audioSource.pitch = 2;
        audioSource.Play();

        textPC.text = "Go!";
        textVR.text = "Go!";
        gameManager.gameStart = true;

        countDownEnd = true;

		lockVRCamera.cameraLock = false;

		for (int count = 0; count < objectsToTurnOff.Length; count++)
		{
			objectsToTurnOff[count].SetActive(true);
		}

		yield return new WaitForSeconds(1.2f);

        textPC.gameObject.SetActive(false);
        textVR.gameObject.SetActive(false);
    }
}