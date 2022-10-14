using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public TextMeshProUGUI text;
	public AudioClip drag;
	public AudioClip click;

	private float maxFontSize;

	private Coroutine biggerCoroutine;
	private Coroutine smallerCoroutine;
	
	private AudioSource audioSource;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		maxFontSize = text.fontSize + 50;
	}

	public void OnClick()
	{
		audioSource.PlayOneShot(click);
	}

	private IEnumerator Bigger()
	{
		audioSource.PlayOneShot(drag);

		float currentFontSize = text.fontSize;
		for (float count = currentFontSize; count <= maxFontSize; count += 3)
		{
			text.fontSize = count;
			yield return null;
		}
	}

	private IEnumerator Smaller()
	{
		float currentFontSize = text.fontSize;
		for (float count = currentFontSize; count >= maxFontSize - 50; count -= 5)
		{
			text.fontSize = count;
			yield return null;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (smallerCoroutine != null)
		{
			StopCoroutine(smallerCoroutine);
			smallerCoroutine = null;
		}
		biggerCoroutine = StartCoroutine(Bigger());
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (biggerCoroutine != null)
		{
			StopCoroutine(biggerCoroutine);
			biggerCoroutine = null;
		}
		smallerCoroutine = StartCoroutine(Smaller());
	}
}