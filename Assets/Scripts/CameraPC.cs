using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPC : MonoBehaviour
{
	public float turnSpeed = 3;

	public Transform cameraPCTransform;

	private Vector3 watchDirection = Vector3.zero;

	private void Start()
	{
		if (!cameraPCTransform) enabled = false;
	}

	private void Update()
	{
		watchDirection += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * turnSpeed;
		cameraPCTransform.rotation = Quaternion.Euler(watchDirection);
	}
}
