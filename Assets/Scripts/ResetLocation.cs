using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLocation : MonoBehaviour
{
	public Transform mainPath;

	public Car car;
	public GameManager gameManager;

	private void Update()
	{
		if (!gameManager.gameStart) return;

		Ray ray = new Ray(mainPath.position, Vector3.down);
		RaycastHit hitData;
		
		if(Physics.Raycast(ray, out hitData))
		{
			print(hitData.collider.gameObject.layer);
			//print(hitData.transform.name);
		}
	}
}
