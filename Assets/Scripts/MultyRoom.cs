using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultyRoom : MonoBehaviour
{
	[SerializeField] private Image[] errorBoxs;
	[SerializeField] private TextMeshProUGUI[] errorTexts;
	
	private void Start()
	{
		LockVRCamera lockVRCamera = FindObjectOfType<LockVRCamera>();

		lockVRCamera.Lock();
		lockVRCamera.SetRotation(new Vector3(9.386f, 90, 0));

		StartCoroutine(ShowError());
	}

	private IEnumerator ShowError()
	{
		for (int count = 0; count < errorTexts.Length; count++)
		{
			errorTexts[count].gameObject.SetActive(false);
		}

		for (int count1 = 0; count1 < errorBoxs.Length; count1++)
		{
			RectTransform errorBoxRectTransform = errorBoxs[count1].GetComponent<RectTransform>();
			Vector2 originSize = errorBoxRectTransform.sizeDelta;
			for (int count2 = 0; count2 <= originSize.y; count2 += 30)
			{
				errorBoxRectTransform.sizeDelta = new Vector2(originSize.x, count2);
				yield return null;
			}
			errorBoxRectTransform.sizeDelta = originSize;
		}

		for (int count = 0; count < errorTexts.Length; count++)
		{
			errorTexts[count].gameObject.SetActive(true);
		}

		yield return new WaitForSeconds(3);

		LoadScene.MoveTo("Title");
	}
}
