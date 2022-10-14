using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class CountDown : MonoBehaviour
{
	public GameManager gameManager;
    public TextMeshProUGUI countDownText;

    public AudioClip beep;

    public Transform cameraTransform;
    public Transform[] cameraPaths = new Transform[3];

    private AudioSource audioSource;

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
        cameraTransform.localPosition = cameraPaths[2].position;
        cameraTransform.localRotation = cameraPaths[2].rotation;
        yield return new WaitForSeconds(1);

        countDownText.gameObject.SetActive(true);

        for (int count = 3; count > 0; count--)
        {
            audioSource.Play();
            cameraTransform.localPosition = cameraPaths[count - 1].position;
            cameraTransform.localRotation = cameraPaths[count - 1].rotation;
            countDownText.text = count.ToString();
            yield return new WaitForSeconds(1.5f);
        }

        audioSource.pitch = 2;
        audioSource.Play();

        countDownText.text = "Go!";
        gameManager.gameStart = true;

        countDownEnd = true;

        yield return new WaitForSeconds(1.2f);

        countDownText.gameObject.SetActive(false);
    }
}
