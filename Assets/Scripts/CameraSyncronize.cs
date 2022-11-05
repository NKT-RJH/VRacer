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
		// 카메라 위치 서로 동기화하기
	}
}