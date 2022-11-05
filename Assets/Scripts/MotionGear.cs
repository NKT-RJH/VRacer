using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MotionHouse;

public class MotionGear : DontDestroyOnLoad<MotionGear>
{
	private float roll;
	private float pitch;
	private float vibration;

	private bool isEnabled;
	private bool isVibration;

	protected override void Awake()
	{
		base.Awake();

		if (isEnabled) return;

		MotionHouseSDK.MHRun();
		MotionHouseSDK.MotionControlStart();

		isEnabled = true;
	}
	
    private void Update()
    {
		if (isVibration)
		{
			vibration *= -1;
		}

		if (roll != 0 || pitch != 0 || vibration != 0)
		{
			MotionHouseSDK.MotionTelemetry(roll, pitch, 0, 0, vibration, 0, 0);
		}
    }

	public void LeanMotionGear(float? pitch, float? roll)
	{
		if (roll != null)
		{
			this.roll = Mathf.Clamp((float)roll, -18, 18);
		}
		if (pitch != null)
		{
			this.pitch = Mathf.Clamp((float)pitch, -18, 18);
		}
	}

	public void StopLeanMotionGear()
	{
		roll = 0;
		pitch = 0;
	}

	public void Vibration(float value)
	{
		vibration = value;

		isVibration = true;
	}

	public void StopVibration()
	{
		vibration = Mathf.Abs(vibration);

		isVibration = false;
	}

	private void OnApplicationQuit()
	{
		MotionHouseSDK.MotionControlEnd();
		MotionHouseSDK.MHStop();
	}
}
