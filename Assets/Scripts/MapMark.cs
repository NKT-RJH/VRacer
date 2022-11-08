using UnityEngine;

public class MapMark : MonoBehaviour
{
	[SerializeField] private Transform mark;

	[SerializeField] private float preset = -200;

	private void Start()
	{
		Instantiate(mark.gameObject);
	}

	private void Update()
	{
		mark.position = transform.position;
		mark.position = new Vector3(transform.position.x, preset, transform.position.z);

		mark.forward = transform.forward;
	}
}