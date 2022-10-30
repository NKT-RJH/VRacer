using UnityEngine;

public class MapMark : MonoBehaviour
{
	[SerializeField] private Transform targetTransform;

	[SerializeField] private float preset;

	private void Update()
	{
		transform.position = targetTransform.position;
		transform.position = new Vector3(transform.position.x, preset, transform.position.z);

		transform.forward = targetTransform.forward;
	}
}
