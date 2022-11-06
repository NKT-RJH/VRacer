using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarMove : MonoBehaviour
{
	public enum DriveType
	{
		FrontWheelDrive,
		RearWheelDrive,
		AllWheelDrive
	}

	[SerializeField] private DriveType driveType;

	[Header("Variables")]
	[SerializeField] private float nomalSpeed;
	[SerializeField] private float boostSpeed;
	[SerializeField] private Transform handle;
	[SerializeField] private Wheels wheels;
	[SerializeField] private WheelMeshs wheelPaths;

	private int motorMax;
	private int motorMin;
	private int motorTorque = 100;

	private float kph;
	private float kphLimit;
	private float power = 10000;
	private const float brakePower = 20000;
	private const float radius = 6;
	private const float downForceValue = 50;
	private float limtSpeed;

	public float KPH { get { return kph; } }
	public float KPHLimit { get { return kphLimit; } }

	private Transform checkPoint;
	private CountDown countDown;
	private InputManager inputManager;
	private Rigidbody rigidBody;
	private LogitechSteeringWheel logitechSteeringWheel;

	private void Awake()
	{
		logitechSteeringWheel = FindObjectOfType<LogitechSteeringWheel>();
		countDown = FindObjectOfType<CountDown>();
		inputManager = FindObjectOfType<InputManager>();
		rigidBody = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		rigidBody.centerOfMass = transform.Find("Mass").localPosition;

		motorMax = motorTorque;
		motorMin = motorMax / 2;
		limtSpeed = nomalSpeed;
	}

	private void FixedUpdate()
	{
		if (!countDown.CountDownEnd) return;

		LogitechWheelForce();

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

	private void LogitechWheelForce()
	{
		//logitechSteeringWheel.SetDamperForce(45 * (100 - KPH/160))
	}

	private void Brake()
	{
		if (inputManager.Clutch > 0) return;

		if (inputManager.Brake <= 0) return;

		if (KPH < 10) return;

		rigidBody.AddRelativeForce(40000 * Vector3.back);
		wheels.backLeft.brakeTorque = brakePower;
		wheels.backRight.brakeTorque = brakePower;
	}

	private void Drift()
	{
		if (inputManager.Clutch > 0) return;

		float stiffness;

		if (inputManager.Drift)
		{
			motorTorque = motorMin;
			stiffness = 1.5f;
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
		// 낮은 기어일수록 시작 속도가 빨라야함
		switch (inputManager.Gear)
		{
			case 0:
				kphLimit = 0;
				power = 0;
				break;
			case 1:
				kphLimit = 30;
				power = 3000;
				break;
			case 2:
				kphLimit = 50;
				power = 4000;
				break;
			case 3:
				kphLimit = 80;
				power = 6500;
				break;
			case 4:
				kphLimit = 110;
				power = 9000;
				break;
			case 5:
				kphLimit = 140;
				power = 11500;
				break;
			case 6:
				kphLimit = 160;
				power = 12500;
				break;
			case 7:
				kphLimit = 10;
				power = 3000;
				break;
		}
		wheels.frontLeft.motorTorque = inputManager.Gas * (motorTorque / startSet);
		wheels.frontRight.motorTorque = inputManager.Gas * (motorTorque / startSet);
		wheels.backLeft.motorTorque = inputManager.Gas * (motorTorque / endSet);
		wheels.backRight.motorTorque = inputManager.Gas * (motorTorque / endSet);

		if (inputManager.Gas > 0 && inputManager.Gear > 0 && inputManager.Gear < 7 && inputManager.Brake < 0 && KPH < KPHLimit)
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

		if (inputManager.Gear == 7 && inputManager.Gas > 0 && KPH < KPHLimit)
		{
			rigidBody.AddRelativeForce(power * inputManager.Gas * Vector3.back);
			wheels.backLeft.brakeTorque = 0;
			wheels.backRight.brakeTorque = 0;
		}
		
		kph = rigidBody.velocity.magnitude * 3.6f;

		if (kph > kphLimit && kph > 2)
		{
			if (inputManager.Gear < 7)
			{
				rigidBody.AddRelativeForce(5000 * Vector3.back);
			}
			else
			{
				rigidBody.AddRelativeForce(1000 * Vector3.forward);
			}
		}
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

		if (path == Vector3.zero) return;

		handle.localRotation = Quaternion.Euler(path);
	}

	private void AddDownForce()
	{
		rigidBody.AddForce(downForceValue * rigidBody.velocity.magnitude * -transform.up);
	}

	private void LimitMoveSpeed()
	{
		if (rigidBody.velocity.x > limtSpeed)
		{
			rigidBody.velocity = new Vector3(limtSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
		}
		if (rigidBody.velocity.x < (limtSpeed * -1))
		{
			rigidBody.velocity = new Vector3((limtSpeed * -1), rigidBody.velocity.y, rigidBody.velocity.z);
		}

		if (rigidBody.velocity.z > limtSpeed)
		{
			rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, limtSpeed);
		}
		if (rigidBody.velocity.z < (limtSpeed * -1))
		{
			rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, (limtSpeed * -1));
		}
	}

	public void CheckPointTeleport()
	{
		if (!inputManager.Respawn) return;

		rigidBody.velocity = Vector3.zero;

		kph = 0;
		
		transform.SetPositionAndRotation(checkPoint.position, checkPoint.rotation);

		StartCoroutine(ResetLogitechWheel());
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