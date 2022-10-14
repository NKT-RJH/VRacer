using UnityEngine;

public enum InputCondition
{
	KeyBoard = 0,
	Driving = 1
}

public class InputManager : MonoBehaviour
{
	//[Header("Logitech")]
	//public float gas;
	//public float clutch;
	//public float brake;
	//public float steer;
	//public int gear;
	//public bool x;
	//public bool o;
	//public bool s;
	//public bool t;
	//public Vector3 joystick = Vector3.zero;

	//[Header("Keyboard")]
	//public float vertical;
	//public float horizontal;

	public float gas;
	public float clutch;
	public float brake;
	public float horizontal;
	public int gear = 0;
	public bool respawn;
	public bool drift;

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

					//if (LogitechInput.GetKeyTriggered(LogitechKeyCode.FirstIndex, LogitechKeyCode.RIGHTBUMPER))
					//{
					//	gear++;
					//	if (gear > 7)
					//	{
					//		gear = 1;
					//	}
					//}

					if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Shifter1))
					{
						gear = 1;
					}
                    else if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Shifter2))
                    {
                        gear = 2;
                    }
                    else if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Shifter3))
                    {
                        gear = 3;
                    }
                    else if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Shifter4))
                    {
                        gear = 4;
                    }
                    else if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Shifter5))
                    {
                        gear = 5;
                    }
                    else if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Shifter6))
                    {
                        gear = 6;
                    }
                    else if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Shifter7))
                    {
                        gear = 7;
                    }
					else
					{
						gear = 0;
					}

     //               for (int count = 12; count < 19; count++)
					//{
					//	if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, (LogitechKeyCode)count))
					//	{
					//		gear = count;
					//	}
					//	if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, (LogitechKeyCode)count))
					//	{
					//		gear = 0;
					//	}

					//}
					//x = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Cross);
					drift = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Circle);
					//s = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Square);
					respawn = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Triangle);

					//joystick = Vector3.zero;

					//if (LogitechInput.GetKeyTriggered(LogitechKeyCode.FirstIndex, LogitechKeyCode.LeftButton))
					//{
					//	joystick += Vector3.left;
					//}
					//else if (LogitechInput.GetKeyTriggered(LogitechKeyCode.FirstIndex, LogitechKeyCode.UpLeftButton))
					//{
					//	joystick += Vector3.left + Vector3.up;
					//	joystick.Normalize();
					//}
					//else if (LogitechInput.GetKeyTriggered(LogitechKeyCode.FirstIndex, LogitechKeyCode.DownLeftButton))
					//{
					//	joystick += Vector3.down + Vector3.up;
					//	joystick.Normalize();
					//}
					//else if (LogitechInput.GetKeyTriggered(LogitechKeyCode.FirstIndex, LogitechKeyCode.RightButton))
					//{
					//	joystick += Vector3.right;
					//}
					//else if (LogitechInput.GetKeyTriggered(LogitechKeyCode.FirstIndex, LogitechKeyCode.UpRightButton))
					//{
					//	joystick += Vector3.right + Vector3.up;
					//	joystick.Normalize();
					//}
					//else if (LogitechInput.GetKeyTriggered(LogitechKeyCode.FirstIndex, LogitechKeyCode.DownRightButton))
					//{
					//	joystick += Vector3.right + Vector3.down;
					//	joystick.Normalize();
					//}
					//else if (LogitechInput.GetKeyTriggered(LogitechKeyCode.FirstIndex, LogitechKeyCode.UpButton))
					//{
					//	joystick += Vector3.up;
					//}
					//else if (LogitechInput.GetKeyTriggered(LogitechKeyCode.FirstIndex, LogitechKeyCode.DownButton))
					//{
					//	joystick += Vector3.down;
					//}
					//print("joystick : " + joystick);
				}
				break;
		}
	}
}
