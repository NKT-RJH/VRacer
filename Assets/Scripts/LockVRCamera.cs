using UnityEngine;
using UnityEngine.SpatialTracking;

public class LockVRCamera : MonoBehaviour
{
	private Quaternion lockRotation;

	private bool cameraLock = true;

	public bool CameraLock { get { return cameraLock; } }

	private float fieldOfView = 60;

	public float FieldOfView { get { return fieldOfView; } }

#pragma warning disable CS0108 // ����� ��ӵ� ����� ����ϴ�. new Ű���尡 �����ϴ�.
	private Camera camera;
#pragma warning restore CS0108 // ����� ��ӵ� ����� ����ϴ�. new Ű���尡 �����ϴ�.

	private void Start()
	{ // �̰͵� �浹 ���� Ȥ�� �� ��Ʈ��ũ���� 
		camera = GetComponent<Camera>();
	}

	private void Update()
	{
		if (cameraLock)
		{
			if (GetComponent<TrackedPoseDriver>())
			{
				GetComponent<TrackedPoseDriver>().enabled = false;
			}
			camera.fieldOfView = fieldOfView;
			transform.localPosition = Vector3.zero;
			transform.rotation = lockRotation;
		}
		else
		{
			if (GetComponent<TrackedPoseDriver>())
			{
				GetComponent<TrackedPoseDriver>().enabled = true;
			}
		}
	}

	public void Lock()
	{
		cameraLock = true;
	}

	public void UnLock()
	{
		cameraLock = false;
	}

	public void SetFieldOfView(float value)
	{
		fieldOfView = value;
	}

	public void SetRotation(Vector3 vector3)
	{
		lockRotation.eulerAngles = vector3;
	}

	public void SetRotation(Quaternion quaternion)
	{
		lockRotation = quaternion;
	}
}
