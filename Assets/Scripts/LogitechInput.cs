using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogitechInput
{
	static LogitechGSDK.DIJOYSTATE2ENGINES rec;

	// Steering = Steering Horizontal, GasInput / Accelerator = Gas Vertical, CluthInput = Clutch Vertical and BrakeInput = Brake Vertical

	public static float GetAxis(string axisName)
	{
		rec = LogitechGSDK.LogiGetStateUnity(0);

		switch (axisName)
		{
			case "Steering Horizontal": 
				return rec.lX / 32760f;
			case "Gas Vertical":
				return rec.lY / -32760f;
			case "Clutch Vertical":
				return rec.rglSlider[0] / -32760f;
			case "Brake Vertical":
				return rec.lRz / -32760f;
		}

		return 0f;
	}

	public static bool GetKeyTriggered(LogitechKeyCode gameController, LogitechKeyCode keyCode)
	{
		return LogitechGSDK.LogiButtonTriggered((int)gameController, (int)keyCode);
	}

	public static bool GetKeyPresssed(LogitechKeyCode gameController, LogitechKeyCode keyCode)
	{
		return LogitechGSDK.LogiButtonIsPressed((int)gameController, (int)keyCode);
	}

	public static bool GetKeyReleased(LogitechKeyCode gameController, LogitechKeyCode keyCode)
	{
		return LogitechGSDK.LogiButtonReleased((int)gameController, (int)keyCode);
	}

	public static uint GetKeyDirectional()
	{
		return rec.rgdwPOV[0];
	}
}
