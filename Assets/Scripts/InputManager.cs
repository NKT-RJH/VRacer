using UnityEngine;

public enum InputCondition
{
	KeyBoard = 0,
	Driving = 1
}

public class InputManager : MonoBehaviour
{
	[Header("Values")]
	public float gas;
	public float clutch;
	public float brake;
	public float horizontal;
	public int gear;
	public bool respawn;
	public bool drift;
	public bool cross;
	public bool circle;
	public bool square;
	public bool triangle;

	[Header("Input Condition")]
	public InputCondition inputCondition;

	private void Update()
	{
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
				gas = Input.GetAxis("Vertical");
				horizontal = Input.GetAxis("Horizontal");
				brake = Input.GetKey(KeyCode.S) ? 1 : -1;
				clutch = Input.GetKey(KeyCode.F) ? 1 : -1;
				drift = Input.GetKey(KeyCode.LeftShift);
				respawn = Input.GetKeyDown(KeyCode.Space);
				if (Input.GetKeyDown(KeyCode.E))
				{
					gear++;
					if (gear > 7)
					{
						gear = 1;
					}
				}
				else if (Input.GetKeyDown(KeyCode.Q))
				{
					gear = 1;
				}
				break;
			case InputCondition.Driving:
				if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected((int)LogitechKeyCode.FirstIndex))
				{
					horizontal = LogitechInput.GetAxis("Steering Horizontal");
					gas = LogitechInput.GetAxis("Gas Vertical");
					brake = LogitechInput.GetAxis("Brake Vertical");
					clutch = LogitechInput.GetAxis("Clutch Vertical");

					bool isPressed = false;

					for (int count = 12; count < 19; count++)
					{
						if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, (LogitechKeyCode)count))
						{
							gear = count - 11;
							isPressed = true;
						}
					}
					
					gear = isPressed ? gear : 0;

					cross = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Cross);
					drift = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Circle);
					circle = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Circle);
					square = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Square);
					respawn = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Triangle);
					triangle = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Triangle);

				}
				break;
		}
	}
}
