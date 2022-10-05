using UnityEngine;

public enum InputCondition
{
	KeyBoard = 0,
	Driving = 1
}

public class InputManager : MonoBehaviour
{
	[Header("Logitech")]
	public float gas;
	public float clutch;
	public float brake;
	public float steer;
	public int gear;
	public bool x;
	public bool o;
	public bool s;
	public bool t;

	[Header("Keyboard")]
	public float vertical;
	public float horizontal;

	[Header("Input Condition")]
	public InputCondition inputCondition;

	private void Update()
	{
		print(LogitechGSDK.LogiIsConnected(0));
		if (!LogitechGSDK.LogiIsConnected(0))
		{
			inputCondition = InputCondition.KeyBoard;
		}
		else
		{
			inputCondition = InputCondition.Driving;
		}

		switch (inputCondition)
		{
			case InputCondition.KeyBoard:
				vertical = Input.GetAxis("Vertical");
				horizontal = Input.GetAxis("Horizontal");
				break;
			case InputCondition.Driving:
				if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected((int)LogitechKeyCode.FirstIndex))
				{
					steer = LogitechInput.GetAxis("Steering Horizontal");
					gas = LogitechInput.GetAxis("Gas Vertical");
					brake = LogitechInput.GetAxis("Brake Vertical");
					clutch = LogitechInput.GetAxis("Clutch Vertical");

					for (int count = 12; count < 19; count++)
					{
						if (LogitechInput.GetKeyTriggered(LogitechKeyCode.FirstIndex, (LogitechKeyCode)count))
						{
							gear = count;
						}
						if (LogitechInput.GetKeyReleased(LogitechKeyCode.FirstIndex, (LogitechKeyCode)count))
						{
							gear = 0;
						}

						x = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Cross);
						o = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Circle);
						s = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Square);
						t = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Triangle);
					}
				}
				break;
		}
	}
}
