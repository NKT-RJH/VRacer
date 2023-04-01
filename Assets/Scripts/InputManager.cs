using UnityEngine;

public class InputManager : Singleton<InputManager>
{
	public bool logitechWheelIsConnected;

	public float gas;
	public float clutch;
	public float brake;
	public float horizontal;
	public int gear = 1;
	public bool respawn;
	public bool drift;
	public bool crossButton;
	public bool circleButton;
	public bool squareButton;
	public bool triangleButton;

	private void Update()
	{
		gas = Mathf.Clamp(Input.GetAxis("Vertical"), 0, float.MaxValue);
		float getAxisHorizontal = Input.GetAxis("Horizontal");
		if (getAxisHorizontal == 0)
		{
			if (horizontal < 0)
			{
				horizontal = Mathf.Clamp(horizontal + Time.deltaTime, -1, 0);
			}
			else
			{
				horizontal = Mathf.Clamp(horizontal - Time.deltaTime, 0, 1);
			}
		}
		else
		{
			horizontal = Mathf.Clamp(horizontal + Time.deltaTime * getAxisHorizontal, -1, 1);
		}
		
		brake = Input.GetKey(KeyCode.S) ? 1 : -1;
		clutch = Input.GetKey(KeyCode.F) ? 1 : -1;
		drift = Input.GetKey(KeyCode.LeftShift);
		respawn = Input.GetKeyDown(KeyCode.Space);
		if (Input.GetKeyDown(KeyCode.E))
		{
			gear++;
			if (gear > 7)
			{
				gear = 0;
			}
		}
		else if (Input.GetKeyDown(KeyCode.Q))
		{
			gear--;
			if (gear < 0)
			{
				gear = 7;
			}
		}

		logitechWheelIsConnected = LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected((int)LogitechKeyCode.FirstIndex);

		if (!logitechWheelIsConnected) return;

		horizontal = LogitechInput.GetAxis("Steering Horizontal");
		gas = Mathf.Clamp(LogitechInput.GetAxis("Gas Vertical"), 0, float.MaxValue);
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

		crossButton = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Cross);
		drift = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Circle);
		circleButton = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Circle);
		squareButton = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Square);
		respawn = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Triangle);
		triangleButton = LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Triangle);
	}

}
