using System.Collections;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
	public GameObject cameraPC;
	public GameObject cameraRig;
	public RectTransform canvasTransform;

	private Vector3 cameraRigPosition = Vector3.zero;

	public CountDown countDown;

	private void Awake()
	{
		cameraRigPosition = canvasTransform.GetComponent<RectTransform>().position;
	}

	private void Start()
	{
		//ChangeCamera();
		StartCoroutine(Wait());
	}

	private IEnumerator Wait()
	{
		while (!countDown.countDownEnd)
		{
			yield return null;
		}
		cameraRig.transform.Find("Camera").transform.localPosition = Vector3.zero;
		cameraRig.transform.Find("Camera").transform.localRotation = Quaternion.identity;
		ChangeCamera();
	}

	private void ChangeCamera()
	{
		if (PlayData.equipment == 1)
		{
			if (!cameraRig.activeSelf) return;

			cameraRig.SetActive(false);
			cameraPC.SetActive(true);
			canvasTransform.SetParent(cameraPC.transform);
			// 각도랑 위치 설정하기
			canvasTransform.transform.localPosition = Vector3.zero;
		}
		else
		{
			if (!cameraPC.activeSelf) return;

			cameraPC.SetActive(false);
			cameraRig.SetActive(true);
			canvasTransform.SetParent(cameraRig.transform.Find("Camera"));
			canvasTransform.position = cameraRigPosition;
		}
	}
}