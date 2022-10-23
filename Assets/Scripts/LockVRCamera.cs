using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockVRCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private Vector3 lockPosition;
    [SerializeField] private Quaternion lockRotation;

    public bool cameraLock = true;

    private Camera camera;

    private void Start()
    {
        camera = cameraTransform.GetComponent<Camera>();

        StartCoroutine(Updating());
    }

    private IEnumerator Updating()
    {
        while(true)
        {
            camera.fieldOfView = 60;
            if (cameraLock)
            {
                Lock();
            }
            else
            {
                UnLock();
            }

            yield return null;
        }
    }

    public void Lock()
    {
        cameraTransform.localPosition = Vector3.zero;
        cameraTransform.rotation = lockRotation;
        cameraLock = true;
    }

    public void UnLock()
    {
        cameraLock = false;
    }
}
