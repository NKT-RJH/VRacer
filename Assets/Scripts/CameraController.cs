using UnityEngine;

public class CameraController : MonoBehaviour
{
	public CarController carController;
	public Transform path;
	public Transform focus;

	private float speed;

	private void Awake()
	{
		Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

		path = playerTransform.Find("CameraPath").transform;
		focus = playerTransform.Find("CameraFocus").transform;
		carController = playerTransform.GetComponent<CarController>();
	}

	private void Start()
	{
		transform.position = path.position;
		transform.LookAt(focus);
	}

	private void FixedUpdate()
	{
		speed = Mathf.Lerp(speed, carController.KPH / 4, Time.deltaTime);

		transform.position = Vector3.Lerp(transform.position, path.position, Time.deltaTime * speed);
		transform.LookAt(focus);
	}
}
