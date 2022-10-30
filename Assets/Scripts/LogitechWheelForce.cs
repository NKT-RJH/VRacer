using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogitechWheelForce : MonoBehaviour
{
    private void Start()
    {
        SetDamperForce(70);
    }

    private static void SetDamperForce(int percentage)
    {
        LogitechGSDK.LogiPlayDamperForce(0, percentage);
    }
}
