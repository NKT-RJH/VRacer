using UnityEngine;

public class GearUI : MonoBehaviour
{
	[SerializeField] private GameObject[] gearUI = new GameObject[7];

	private InputManager inputManager;

	private void Awake()
	{
		inputManager = FindObjectOfType<InputManager>();
	}

	private void Update()
	{
		for (int count = 0; count < gearUI.Length; count++)
		{
			gearUI[count].SetActive(false);
		}

		if (inputManager.Gear <= 0) return;

		gearUI[inputManager.Gear - 1].SetActive(true);
	}
}
