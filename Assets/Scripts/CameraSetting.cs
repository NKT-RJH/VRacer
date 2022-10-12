using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CameraSetting : MonoBehaviour
{
    public GameObject cameraPC;
    public GameObject cameraRig;
    public RectTransform canvasTransform;

	private Vector3 cameraRigPosition = Vector3.zero;

	private void Awake()
	{
		cameraRigPosition = canvasTransform.GetComponent<RectTransform>().position;
	}

	private void Start()
    {
        ChangeCamera();
    }

    private void Update()
    {
        ChangeCamera();
    }

    private void ChangeCamera()
    {
		//print(SteamVR.);
        if (!SteamVR.active)
        {
            if (!cameraRig.activeSelf) return;

            cameraRig.SetActive(false);
            cameraPC.SetActive(true);
			canvasTransform.SetParent(cameraPC.transform);
			//canvasTransform.transform.position = cameraPC.transform.position + Vector3.forward * 0.1f;
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