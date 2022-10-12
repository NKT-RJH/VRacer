using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPC : MonoBehaviour
{
	public float turnSpeed = 10;

	public Transform cameraPCTransform;

	public InputManager inputManager;

	private Vector3 watchDirection = Vector3.zero;

	private void Start()
	{
		if (!inputManager) enabled = false;
		if (!cameraPCTransform) enabled = false;

		cameraPCTransform.rotation = Quaternion.Euler(Vector3.zero);
	}

	private void FixedUpdate()
	{
		//if (inputManager.inputCondition == InputCondition.Driving)
		//{
		//	watchDirection += new Vector3(-Input.GetAxis("DPadHorizontal"), Input.GetAxis("DPadVertical"), 0) * turnSpeed * Time.fixedDeltaTime;
		//}
		//else if (inputManager.inputCondition == InputCondition.KeyBoard)
		//{
			watchDirection += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * turnSpeed * Time.fixedDeltaTime;
		//}
		//print(watchDirection);
		//Input.GetButton("DPad X");
			cameraPCTransform.rotation = Quaternion.Euler(watchDirection);
	}
}
