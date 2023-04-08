public class LogitechInput
{
	/// <summary>
	/// Logitech 기기 입력 값
	/// </summary>
	static LogitechGSDK.DIJOYSTATE2ENGINES rec;

	// Steering = Steering Horizontal, GasInput / Accelerator = Gas Vertical, CluthInput = Clutch Vertical and BrakeInput = Brake Vertical

	/// <summary>
	/// Axis의 이름에 따른 로지텍 기기 입력 값 반환 (Steering Horizontal, Gas Vertical, Clutch Vertical, Brake Vertical)
	/// </summary>
	/// <param name="axisName"></param>
	/// <returns></returns>
	public static float GetAxis(string axisName)
	{
		// rec에 현재 로지텍 기기 값 할당
		rec = LogitechGSDK.LogiGetStateUnity(0);

		// Axis 이름 확인 및 값 조정 후 반환
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
			default:
				return 0f;
		}
	}

	/// <summary>
	/// 로지텍 기기의 해당 버튼을 누르고 있는 상태인지 Bool 값으로 반환
	/// </summary>
	/// <param name="gameController"></param>
	/// <param name="keyCode"></param>
	/// <returns></returns>
	public static bool GetKeyTriggered(LogitechKeyCode gameController, LogitechKeyCode keyCode)
	{
		return LogitechGSDK.LogiButtonTriggered((int)gameController, (int)keyCode);
	}

	/// <summary>
	/// 로지텍 기기의 해당 버튼을 눌렀는지 Bool 값으로 반환
	/// </summary>
	/// <param name="gameController"></param>
	/// <param name="keyCode"></param>
	/// <returns></returns>
	public static bool GetKeyPresssed(LogitechKeyCode gameController, LogitechKeyCode keyCode)
	{
		return LogitechGSDK.LogiButtonIsPressed((int)gameController, (int)keyCode);
	}

	/// <summary>
	/// 로지텍 기기의 해당 버튼을 떼었는지 Bool 값으로 반환
	/// </summary>
	/// <param name="gameController"></param>
	/// <param name="keyCode"></param>
	/// <returns></returns>
	public static bool GetKeyReleased(LogitechKeyCode gameController, LogitechKeyCode keyCode)
	{
		return LogitechGSDK.LogiButtonReleased((int)gameController, (int)keyCode);
	}
}