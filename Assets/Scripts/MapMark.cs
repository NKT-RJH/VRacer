using UnityEngine;
using UnityEngine.UI;

public class MapMark : MonoBehaviour
{
	[SerializeField] private GameObject mark;

	[SerializeField] private float preset = -200;

	private Transform markTransform;

	private void Start()
	{
		markTransform = Instantiate(mark).transform;
		if (PlayData.map == 1)
		{
			markTransform.localScale = Vector3.one * 0.5f;
		}
	}

	private void Update()
	{
		markTransform.position = transform.position;
		markTransform.position = new Vector3(transform.position.x, preset, transform.position.z);

		markTransform.forward = transform.forward;
	}
}