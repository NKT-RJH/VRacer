using TMPro;
using UnityEngine;

public class CarSpeed : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textMeshProUGUI;

	private int speed;

	private Car car;

	private void Awake()
	{
		car = FindObjectOfType<Car>();
	}

	private void Update()
	{
		if (Mathf.Abs(car.KPH - speed) >= 1)
		{
			speed = (int)car.KPH;
		}

		textMeshProUGUI.text = string.Format("{0} KM/H", speed);
	}
}
