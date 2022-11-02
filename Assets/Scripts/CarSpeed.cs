using TMPro;
using UnityEngine;

public class CarSpeed : MonoBehaviour
{
	public TextMeshProUGUI speedText;
	public Car car;

	private int speed;
	private void Update()
	{
		if (Mathf.Abs(car.KPH - speed) >= 1)
		{
			speed = (int)car.KPH;
		}

		speedText.text = string.Format("{0} KM/H", speed);
	}
}
