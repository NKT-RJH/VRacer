using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClearCheck : MonoBehaviour
{
	private List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();

	public bool isClear;

	private const int maxClear = 2;

	private int clearCount = 0;

	private bool[] clears =
	{
		false,
		false,
		false,
		false,
		false,
		false,
		false
	};

	private void Start()
	{
		GameObject[] textObjects = GameObject.FindGameObjectsWithTag("ClearCount");

		for (int count = 0; count < texts.Count; count++)
		{
			texts.Add(textObjects[count].GetComponent<TextMeshProUGUI>());
		}
	}

	private void Update()
	{
		for (int count = 0; count < texts.Count; count++)
		{
			texts[count].text = string.Format("{0}/{0}", clearCount, maxClear);
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

		if (counter + 1 == clears.Length)
		{
			for (int count = 0; count < clears.Length; count++)
			{
				clears[count] = false;
			}

			clearCount++;
		}

		if (clearCount == maxClear)
		{
			isClear = true;
		}
	}
}
