public class LogitechInput
{
	/// <summary>
	/// Logitech ��� �Է� ��
	/// </summary>
	static LogitechGSDK.DIJOYSTATE2ENGINES rec;

	// Steering = Steering Horizontal, GasInput / Accelerator = Gas Vertical, CluthInput = Clutch Vertical and BrakeInput = Brake Vertical

	/// <summary>
	/// Axis�� �̸��� ���� ������ ��� �Է� �� ��ȯ (Steering Horizontal, Gas Vertical, Clutch Vertical, Brake Vertical)
	/// </summary>
	/// <param name="axisName"></param>
	/// <returns></returns>
	public static float GetAxis(string axisName)
	{
		// rec�� ���� ������ ��� �� �Ҵ�
		rec = LogitechGSDK.LogiGetStateUnity(0);

		// Axis �̸� Ȯ�� �� �� ���� �� ��ȯ
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
	/// ������ ����� �ش� ��ư�� ������ �ִ� �������� Bool ������ ��ȯ
	/// </summary>
	/// <param name="gameController"></param>
	/// <param name="keyCode"></param>
	/// <returns></returns>
	public static bool GetKeyTriggered(LogitechKeyCode gameController, LogitechKeyCode keyCode)
	{
		return LogitechGSDK.LogiButtonTriggered((int)gameController, (int)keyCode);
	}

	/// <summary>
	/// ������ ����� �ش� ��ư�� �������� Bool ������ ��ȯ
	/// </summary>
	/// <param name="gameController"></param>
	/// <param name="keyCode"></param>
	/// <returns></returns>
	public static bool GetKeyPresssed(LogitechKeyCode gameController, LogitechKeyCode keyCode)
	{
		return LogitechGSDK.LogiButtonIsPressed((int)gameController, (int)keyCode);
	}

	/// <summary>
	/// ������ ����� �ش� ��ư�� �������� Bool ������ ��ȯ
	/// </summary>
	/// <param name="gameController"></param>
	/// <param name="keyCode"></param>
	/// <returns></returns>
	public static bool GetKeyReleased(LogitechKeyCode gameController, LogitechKeyCode keyCode)
	{
		return LogitechGSDK.LogiButtonReleased((int)gameController, (int)keyCode);
	}
}