using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearUI : MonoBehaviour
{
	[SerializeField] private GameObject[] gearUI = new GameObject[7];

	[SerializeField] private InputManager inputManager;

	private void Update()
	{
		for (int count = 0; count < gearUI.Length; count++)
		{
			gearUI[count].SetActive(false);
		}

		if (inputManager.gear <= 0) return;

		gearUI[inputManager.gear - 1].SetActive(true);
	}
}
