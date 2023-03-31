using UnityEngine;
using UnityEngine.UI;

public class SyncronizeUI : MonoBehaviour
{
	[SerializeField] private Twins<RectTransform>[] rectTransform;
	[SerializeField] private Twins<Image>[] image;

	private Twins<RectTransform>[] originRectTransform;

	private void Awake()
	{
		originRectTransform = new Twins<RectTransform>[rectTransform.Length];

		for (int count = 0; count < rectTransform.Length; count++)
		{
			originRectTransform[count] = new Twins<RectTransform>();
			originRectTransform[count].SetValue1(rectTransform[count].GetValue()[0]);
			originRectTransform[count].SetValue2(rectTransform[count].GetValue()[1]);
		}
	}

	private void Update()
	{
		SyncronizeRectTransform(true);

		SyncronizeImage(true);
	}

	private void SyncronizeRectTransform(bool benchmarkIsFirst)
	{
		for (int count = 0; count < rectTransform.Length; count++)
		{
			if (benchmarkIsFirst)
			{
				Vector3 abmodality = rectTransform[count].GetValue()[0].position - originRectTransform[count].GetValue()[0].position;

				RectTransform secondValue = rectTransform[count].GetValue()[1];

				secondValue.position += abmodality;

				rectTransform[count].SetValue2(secondValue);
			}
			else
			{
				Vector3 abmodality = rectTransform[count].GetValue()[1].position - originRectTransform[count].GetValue()[1].position;

				RectTransform firstValue = rectTransform[count].GetValue()[0];

				firstValue.position += abmodality;

				rectTransform[count].SetValue2(firstValue);
			}
		}
	}

	private void SyncronizeImage(bool benchmarkIsFirst)
	{
		for (int count = 0; count < image.Length; count++)
		{
			if (benchmarkIsFirst)
			{
				image[count].SetValue2(image[count].GetValue()[0]);
			}
			else
			{
				image[count].SetValue1(image[count].GetValue()[1]);
			}
		}
	}
}
