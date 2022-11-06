using UnityEngine;

public class SyncronizeCamera : MonoBehaviour
{
	[Header("Cashing")]
	[SerializeField] private Transform cameraPC;
	[SerializeField] private Transform cameraRig;
	
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
				cameraPC.SetPositionAndRotation(cameraRig.position, cameraRig.rotation);
				break;
			case 1:
				cameraRig.SetPositionAndRotation(cameraPC.position, cameraPC.rotation);
				break;
		}
	}
}