using UnityEngine;

public class MoveBackground : MonoBehaviour
{
	public float maxZposition;
	public float returnZPosition;

	public Transform[] wheelsTransform = new Transform[4];

	public Transform wallTransform;

	private float countTime;

	private void Update()
	{
		try
		{
			checked(countTime) += Time.deltaTime;
		}
		catch
		{
			countTime = 0;
		}

		for (int count = 0; count < wheelsTransform.Length; count++)
		{
			wheelsTransform[count].rotation = Quaternion.Euler(Vector3.right * countTime * 1000);
		}

		wallTransform.position += Vector3.back * 20 * Time.deltaTime;

		if (wallTransform.position.z < maxZposition)
		{
			wallTransform.position = new Vector3(wallTransform.position.x, wallTransform.position.y, returnZPosition);
		}
	}
}
