using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMirror : MonoBehaviour
{
	[SerializeField] private Transform mirrorCamera;
	[SerializeField] private Transform cameraPC;

	private void Update()
	{
		
	}

	private void CalculateRotation()
	{
		Vector3 direction = (cameraPC.position - transform.position);

		Quaternion rotation = Quaternion.LookRotation(direction);

		rotation.eulerAngles = transform.eulerAngles - rotation.eulerAngles;

		mirrorCamera.localRotation = rotation;
	}
}
