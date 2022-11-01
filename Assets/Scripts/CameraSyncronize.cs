using System.Collections;
using UnityEngine;

public class CameraSyncronize : MonoBehaviour
{
	[SerializeField] private Transform cameraPC;
	[SerializeField] private Transform cameraRig;

	private InputManager inputManager;

	private void Awake()
	{
		inputManager = FindObjectOfType<InputManager>();
	}

	private void Update()
	{
		switch (inputManager.inputCondition)
		{
			case InputCondition.Driving:
				
				break;
			case InputCondition.KeyBoard:
				break;
		}
	}
}