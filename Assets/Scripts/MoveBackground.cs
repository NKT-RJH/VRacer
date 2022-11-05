using UnityEngine;

public class MoveBackground : MonoBehaviour
{
	[SerializeField] private float maxZposition;
	[SerializeField] private float returnZPosition;

	[SerializeField] private Transform[] wheelsTransform = new Transform[4];

	[SerializeField] private Transform wallTransform;

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
			wheelsTransform[count].rotation = Quaternion.Euler(1000 * countTime * Vector3.right);
		}

		wallTransform.position += Vector3.back * 20 * Time.deltaTime;

		if (wallTransform.position.z < maxZposition)
		{
			wallTransform.position = new Vector3(wallTransform.position.x, wallTransform.position.y, returnZPosition);
		}
	}
}
