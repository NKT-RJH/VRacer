using UnityEngine;

public class UIGear : MonoBehaviour
{
	[SerializeField] private GameObject[] gearCircles = new GameObject[7];

	private InputManager inputManager;

	private void Awake()
	{
		inputManager = FindObjectOfType<InputManager>();
	}

	private void Update()
	{
		for (int count = 0; count < gearCircles.Length; count++)
		{
			gearCircles[count].SetActive(false);
		}

		if (inputManager.Gear <= 0) return;

		gearCircles[inputManager.Gear - 1].SetActive(true);
	}
}
