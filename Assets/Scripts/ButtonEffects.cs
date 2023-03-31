using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Button))]
public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private AudioClip drag;
	[SerializeField] private AudioClip click;

	private float maxFontSize;

	private Coroutine biggerCoroutine;
	private Coroutine smallerCoroutine;

	private AudioSource audioSource;
	private TextMeshProUGUI textMeshProUGUI;

	private const int addButtonFontSize = 50;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
	}

	private void Start()
	{
		GetComponent<Button>().onClick.AddListener(delegate { PlayClickSound(); });

		maxFontSize = textMeshProUGUI.fontSize + addButtonFontSize;
	}

	private IEnumerator Bigger()
	{
		audioSource.PlayOneShot(drag);

		float currentFontSize = textMeshProUGUI.fontSize;
		for (float count = currentFontSize; count <= maxFontSize; count += 3)
		{
			textMeshProUGUI.fontSize = count;
			yield return null;
		}
	}

	private IEnumerator Smaller()
	{
		float currentFontSize = textMeshProUGUI.fontSize;
		for (float count = currentFontSize; count >= maxFontSize - addButtonFontSize; count -= 5)
		{
			textMeshProUGUI.fontSize = count;
			yield return null;
		}
	}

	public void PlayClickSound()
	{
		audioSource.PlayOneShot(click);
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