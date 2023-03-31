using TMPro;
using UnityEngine;

public class CarSpeed : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textMeshProUGUI;

	private Car car;

	private void Awake()
	{
		car = FindObjectOfType<Car>();
	}

	private void Update()
	{
		textMeshProUGUI.text = string.Format("{0} KM/H", Mathf.RoundToInt(car.KPH)); // �������ڸ��� ����
	}
}
