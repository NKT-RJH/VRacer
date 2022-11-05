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

    private void Update()
    {
        if (!(LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))) return;

		LogitechGSDK.LogiPlayDamperForce(0, 45);
    }

	public void SetDamperForce(float value)
	{
		damperForce = value;
	}
}