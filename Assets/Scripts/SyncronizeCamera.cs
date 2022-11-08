using UnityEngine;

public class SyncronizeCamera : MonoBehaviour
{
	[Header("Cashing")]
	[SerializeField] private Transform cameraPC;
	[SerializeField] private Transform cameraVR;
	
	private LockVRCamera lockVRCamera;

	private void Awake()
	{
		lockVRCamera = FindObjectOfType<LockVRCamera>();
	}

	private void Start()
	{
		if (PlayData.equipment == 1)
		{
			lockVRCamera.Lock();
		}
	}

	private void Update()
	{
		switch (PlayData.equipment)
		{
			case 0:
				cameraPC.SetPositionAndRotation(cameraVR.position, cameraVR.rotation);
				break;
			case 1:
				if (!lockVRCamera.CameraLock)
				{
					cameraVR.SetPositionAndRotation(cameraPC.position, cameraPC.rotation);
				}
				break;
		}
	}
}