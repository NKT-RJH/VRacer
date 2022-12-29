using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Car : MonoBehaviour
{
	public enum DriveType
	{
		FrontWheelDrive,
		RearWheelDrive,
		AllWheelDrive
	}

	[SerializeField] private DriveType driveType;

	[Header("Variables")]
	[SerializeField] private Transform handle;
	[SerializeField] private Wheels wheels;
	[SerializeField] private WheelMeshs wheelPaths;

	private int motorMax;
	private int motorMin;
	private int motorTorque = 100;
	private int beforeGear = 0;

	private bool isCrash;
	private bool downRPM;

	private float rpm;
	private float rpmLimit;
	private float kph;
	private float kphLimit;
	private float beforekph;
	private float power = 10000;
	private const float brakePower = 20000;
	private const float radius = 6;
	private const float downForceValue = 50;

	public float RPM { get { return rpm; } }

	public bool IsCrash { get { return isCrash; } }

	public float KPH { get { return kph; } }
	public float KPHLimit { get { return kphLimit; } }

	private Vector3 bodyTlit;

	public Vector3 BodyTlit { get { return bodyTlit; } }

	private const float finalReductionGear = 3.71f;

	private float[] gearRatio = { 3.615f, 1.962f, 1.294f, 0.976f, 0.778f, 0.633f, 3.583f };

	private ClearCheck clearCheck;
	private Transform checkPoint;
	private CountDown countDown;
	private InputManager inputManager;
	private Rigidbody rigidBody;

	private void Awake()
	{
		clearCheck = GetComponent<ClearCheck>();
		countDown = FindObjectOfType<CountDown>();
		inputManager = FindObjectOfType<InputManager>();
		rigidBody = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		kphLimit = 275;

		rpmLimit = 9000;

		rigidBody.centerOfMass = transform.Find("Mass").localPosition;

		motorMax = motorTorque;
		motorMin = motorMax / 2;
	}

	private void Update()
	{
		if (!countDown.CountDownEnd) return;
		if (clearCheck.IsClear) return;

		LogitechWheelForce();

		SetIsCrash();

		SetBodyTilt();
	}

	private void FixedUpdate()
	{
		if (!countDown.CountDownEnd) return;
		if (clearCheck.IsClear)
		{
			Stop();
			if (kph > 5)
			{
				rigidBody.AddRelativeForce(10000 * Vector3.back);
			}
			return;
		}


		CheckPointTeleport();

		Drift();

		Brake();

		AddDownForce();

		AnimateHandle();

		AnimateWheels();

		MoveVehicle();

		SteerVehicle();

		LimitMoveSpeed();
	}

	private void Stop()
	{
		rigidBody.velocity = Vector3.zero;
	}

	private void SetIsCrash()
	{
		//if 
	}

	private void SetBodyTilt()
	{
		bodyTlit = transform.localEulerAngles;
	}

	private void LogitechWheelForce()
	{
		if (!(LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))) return;

		LogitechGSDK.LogiPlayDamperForce(0, 70 - Mathf.RoundToInt(kph / 6));
	}

	private void Brake()
	{
		if (inputManager.Clutch > 0) return;

		if (inputManager.Brake <= 0)
		{
			downRPM = false;
			return;
		}

		if (KPH < 10) return;

		Vector3 powerPath = Vector3.zero;

		float dot = Vector3.Dot(transform.forward, rigidBody.velocity);

		powerPath = dot > 0 ? Vector3.back : Vector3.forward;

		rigidBody.AddRelativeForce(40000 * powerPath);
		wheels.backLeft.brakeTorque = brakePower;
		wheels.backRight.brakeTorque = brakePower;

		if (!downRPM)
		{
			downRPM = true;
			rpm -= kph * 20;
		}
	}

	private void Drift()
	{
		if (inputManager.Clutch > 0) return;
		if (kph < 20) return;

		float stiffness;

		if (inputManager.Drift)
		{
			motorTorque = motorMin;
			stiffness = 1.75f;
		}
		else
		{
			motorTorque = motorMax;
			stiffness = 2;
		}

		WheelFrictionCurve wheelFrictionCurve;

		wheelFrictionCurve = wheels.frontLeft.forwardFriction;
		wheelFrictionCurve.stiffness = stiffness;
		wheels.frontLeft.forwardFriction = wheelFrictionCurve;

		wheelFrictionCurve = wheels.frontRight.forwardFriction;
		wheelFrictionCurve.stiffness = stiffness;
		wheels.frontRight.forwardFriction = wheelFrictionCurve;

		wheelFrictionCurve = wheels.backLeft.sidewaysFriction;
		wheelFrictionCurve.stiffness = stiffness;
		wheels.backLeft.sidewaysFriction = wheelFrictionCurve;

		wheelFrictionCurve = wheels.backRight.sidewaysFriction;
		wheelFrictionCurve.stiffness = stiffness;
		wheels.backRight.sidewaysFriction = wheelFrictionCurve;
	}

	private void MoveVehicle()
	{
		if (inputManager.Clutch > 0) return;

		int startSet = 0;
		int endSet = 0;

		switch (driveType)
		{
			case DriveType.AllWheelDrive:
				startSet = 4;
				endSet = 4;
				break;
			case DriveType.RearWheelDrive:
				startSet = 2;
				endSet = 1;
				break;
			case DriveType.FrontWheelDrive:
				startSet = 1;
				endSet = 2;
				break;
		}

		if (inputManager.Gas > 0)
		{
			float value = 1;
			if (inputManager.Gear > 0 && inputManager.Gear < 7)
			{
				value = (7 - inputManager.Gear) * 100;
			}
			rpm = Mathf.Clamp(rpm + Time.deltaTime * inputManager.Gas * value, 0, rpmLimit);
		}
		else if (inputManager.Gear <= 0 && inputManager.Gear >= 7)
		{
			rpm = Mathf.Clamp(rpm + Time.deltaTime * inputManager.Gas * 300, 0, rpmLimit);
		}
		else
		{
			rpm = Mathf.Clamp(rpm + Time.deltaTime * inputManager.Gas * 1000, 0, rpmLimit);
		}

		if ((int)beforekph != (int)kph && (int)kph == 0)
		{
			print("Àû¿ë");
			rpm = 0;
		}

		beforekph = kph;

		//float[] toque = { 1, 3f, 7f, 13f, 19f, 25f };

		if (beforeGear < inputManager.Gear)
		{
			if (beforeGear != 0)
			{
				rpm = Mathf.Clamp(rpm - 2000, 0, rpmLimit);
			}
			beforeGear = inputManager.Gear;
		}
		else if (beforeGear > inputManager.Gear)
		{
			if (beforeGear != 0)
			{
				rpm = Mathf.Clamp(rpm + 2000, 0, rpmLimit);
			}
			beforeGear = inputManager.Gear;
		}

		switch (inputManager.Gear)
		{
			case 0:
				power = 0;
				break;
			case 1:
				power = rpm / (gearRatio[0]) * (kphLimit / 4 + 1 - Mathf.Clamp(kph, kph, kphLimit / 4));
				break;
			case 2:
				power = rpm / (gearRatio[1]) * (kphLimit / 7 + 1 - Mathf.Clamp(kph, kph, kphLimit / 7));
				break;
			case 3:
				power = rpm / (gearRatio[2]) * (kphLimit / 13 + 1 - Mathf.Clamp(kph, kph, kphLimit / 13));
				break;
			case 4:
				power = rpm / (gearRatio[3]) * (kphLimit / 19 + 1 - Mathf.Clamp(kph, kph, kphLimit / 19));
				break;
			case 5:
				power = rpm / (gearRatio[4]) * (kphLimit / 25 + 1 - Mathf.Clamp(kph, kph, kphLimit / 25));
				break;
			case 6:
				power = rpm / (gearRatio[5]);
				break;
			case 7:
				power = rpm / gearRatio[6] * 2;
				break;
		}
		wheels.frontLeft.motorTorque = inputManager.Gas * (motorTorque / startSet);
		wheels.frontRight.motorTorque = inputManager.Gas * (motorTorque / startSet);
		wheels.backLeft.motorTorque = inputManager.Gas * (motorTorque / endSet);
		wheels.backRight.motorTorque = inputManager.Gas * (motorTorque / endSet);


		if (inputManager.Gas > 0 && inputManager.Gear > 0 && inputManager.Gear < 7 && inputManager.Brake < 0 && kph < kphLimit)
		{
			rigidBody.AddRelativeForce(power * inputManager.Gas * Vector3.forward);
			wheels.backLeft.brakeTorque = 0;
			wheels.backRight.brakeTorque = 0;
		}
		else if (inputManager.Gear <= 0)
		{
			wheels.backLeft.brakeTorque = brakePower;
			wheels.backRight.brakeTorque = brakePower;
		}

		if (inputManager.Gear == 7 && inputManager.Gas > 0 && kph < kphLimit)
		{
			rigidBody.AddRelativeForce(power * inputManager.Gas * Vector3.back);
			wheels.backLeft.brakeTorque = 0;
			wheels.backRight.brakeTorque = 0;
		}

		kph = rigidBody.velocity.magnitude * 3.6f;
	}

	private void SteerVehicle()
	{
		if (inputManager.Horizontal != 0)
		{
			int symbol = inputManager.Horizontal > 0 ? 1 : -1;

			wheels.frontLeft.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2) * symbol)) * inputManager.Horizontal;
			wheels.frontRight.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2) * symbol)) * inputManager.Horizontal;
		}
		else
		{
			wheels.frontLeft.steerAngle = 0;
			wheels.frontRight.steerAngle = 0;
		}
	}

	private void AnimateWheels()
	{
		wheels.frontLeft.GetWorldPose(out Vector3 position1, out Quaternion rotation1);
		wheelPaths.frontLeft.position = position1;
		wheelPaths.frontLeft.rotation = rotation1;

		wheels.frontRight.GetWorldPose(out Vector3 position2, out Quaternion rotation2);
		wheelPaths.frontRight.position = position2;
		wheelPaths.frontRight.rotation = rotation2;

		wheels.backLeft.GetWorldPose(out Vector3 position3, out Quaternion rotation3);
		wheelPaths.backLeft.position = position3;
		wheelPaths.backLeft.rotation = rotation3;

		wheels.backRight.GetWorldPose(out Vector3 position4, out Quaternion rotation4);
		wheelPaths.backRight.position = position4;
		wheelPaths.backRight.rotation = rotation4;
	}

	private void AnimateHandle()
	{
		Vector3 path = 450 * -inputManager.Horizontal * Vector3.forward;

		//if (path == Vector3.zero) return;

		handle.localRotation = Quaternion.Euler(path);
		handle.eulerAngles = new Vector3(23.253f, handle.eulerAngles.y, handle.eulerAngles.z);
	}

	private void AddDownForce()
	{
		rigidBody.AddForce(downForceValue * rigidBody.velocity.magnitude * -transform.up);
	}

	private void LimitMoveSpeed()
	{
		if (rigidBody.velocity.x > kphLimit)
		{
			rigidBody.velocity = new Vector3(kphLimit, rigidBody.velocity.y, rigidBody.velocity.z);
		}
		if (rigidBody.velocity.x < (kphLimit * -1))
		{
			rigidBody.velocity = new Vector3((kphLimit * -1), rigidBody.velocity.y, rigidBody.velocity.z);
		}

		if (rigidBody.velocity.z > kphLimit)
		{
			rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, kphLimit);
		}
		if (rigidBody.velocity.z < (kphLimit * -1))
		{
			rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, (kphLimit * -1));
		}
	}

	public void CheckPointTeleport()
	{
		if (!inputManager.Respawn) return;

		StartCoroutine(WatingTime());

		rpm = 0;

		rigidBody.velocity = Vector3.zero;

		kph = 0;

		transform.SetPositionAndRotation(checkPoint.position, checkPoint.rotation);

		StartCoroutine(ResetLogitechWheel());
	}

	private IEnumerator WatingTime()
	{
		rigidBody.useGravity = false;
		GetComponentInChildren<MeshCollider>().enabled = false;

		yield return new WaitForSeconds(1.5f);

		GetComponentInChildren<MeshCollider>().enabled = true;
		rigidBody.useGravity = true;
	}

	private IEnumerator ResetLogitechWheel()
	{
		LogitechGSDK.LogiPlaySpringForce(0, 0, 100, 100);

		while (inputManager.Horizontal > 0.05f || inputManager.Horizontal < -0.05f)
		{
			yield return null;
		}

		LogitechGSDK.LogiStopSpringForce(0);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("CheckPoint"))
		{
			checkPoint = other.transform;
		}
	}
}