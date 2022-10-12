using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MoveBackground : MonoBehaviour
{
	public float maxZposition;
	public float returnZPosition;
	public Volume volume;

	public Transform wallTransform;

	private void Update()
	{
		//volume.priority.

		wallTransform.position += Vector3.back * 20 * Time.deltaTime;

		if (wallTransform.position.z < maxZposition)
		{
			wallTransform.position = new Vector3(wallTransform.position.x, wallTransform.position.y, returnZPosition);
		}
	}
}
