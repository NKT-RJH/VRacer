using System;
using TMPro;
using UnityEngine;

public class CarSpeed : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI kmPerHourText;

	private Car car;

	private void Awake()
	{
		try
		{
			car = FindObjectOfType<Car>();
		}
		catch (NullReferenceException)
		{
			car = null;
		}
	}

	private void Update()
	{
		if (car == null) return;

		kmPerHourText.text = string.Format("{0} KM/H", Mathf.RoundToInt(car.kmPerHour));
	}
}
