using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Title : MonoBehaviour
{
	public Image fadeIn;

    public AudioClip BGM;
	public AudioClip carStart;

    private AudioSource audioSource;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		StartCoroutine(FaidOutAndBGM());
	}

	private IEnumerator FaidOutAndBGM()
	{
		audioSource.PlayOneShot(carStart);

		for (float count = 1; count >= 0; count -= 1 / 255f)
		{
			fadeIn.color = new Color(fadeIn.color.r, fadeIn.color.g, fadeIn.color.b, count);
			yield return null;
		}
		fadeIn.gameObject.SetActive(false);

		audioSource.clip = BGM;
		audioSource.Play();
	}
}
