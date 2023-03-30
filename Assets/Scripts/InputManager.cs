using UnityEngine;

public class InputManager : Singleton<InputManager>
{
	private float gas;
	private float clutch;
	private float brake;
	private float horizontal;
	private int gear = 1;
	private bool respawn;
	private bool drift;
	private bool cross;
	private bool circle;
	private bool square;
	private bool triangle;

	public float Gas { get { return gas; } }
	public float Clutch { get { return clutch; } }
	public float Brake { get { return brake; } }
	public float Horizontal { get { return horizontal; } }
	public int Gear { get { return gear; } }
	public bool Respawn { get { return respawn; } }
	public bool Drift { get { return drift; } }
	public bool Cross { get { return cross; } }
	public bool Circle { get { return circle; } }
	public bool Square { get { return square; } }
	public bool Triangle { get { return triangle; } }

	private void Update()
	{
		gas = Input.GetAxis("Vertical") > 0 ? Input.GetAxis("Vertical") : -1;
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

		if (!(LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected((int)LogitechKeyCode.FirstIndex))) return;

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

}
