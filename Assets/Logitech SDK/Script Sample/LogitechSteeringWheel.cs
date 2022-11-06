using UnityEngine;

public class LogitechSteeringWheel : MonoBehaviour
{
    private float damperForce = 45;

    public float DamperForce { get { return damperForce; } }

    private bool isEnabled = false;

    private void Start()
    {
        if (isEnabled) return;

        LogitechGSDK.LogiSteeringInitialize(false);

        isEnabled = true;
    }

    private void OnApplicationQuit()
    {
        LogitechGSDK.LogiSteeringShutdown();
    }

    public void SetDamperForce(float value)
    {
        damperForce = value;
    }
}