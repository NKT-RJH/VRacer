using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    public GameObject cameraPC;
    public GameObject cameraRig;
    public Transform canvasTransform;

    private InputManager inputManager;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
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
        if (inputManager.inputCondition == InputCondition.KeyBoard)
        {
            if (!cameraRig.activeSelf) return;

            cameraRig.SetActive(false);
            cameraPC.SetActive(true);
            canvasTransform.parent = cameraPC.transform;
        }
        else if (inputManager.inputCondition == InputCondition.Driving)
        {
            if (!cameraPC.activeSelf) return;

            cameraPC.SetActive(false);
            cameraRig.SetActive(true);
            canvasTransform.parent = cameraRig.transform.Find("Camera");
        }
    }
}
