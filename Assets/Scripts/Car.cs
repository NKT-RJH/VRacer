using System;
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

	private int beforeGear = 0;

	public float rpm;
	private const float rpmLimit = 9000;
	public float kmPerHour;
	private int beforeKMPerHour;
	private float torque = 0; //Nm

	private const float downForceValue = 50;

	public Vector3 BodyTlit { get { return transform.localEulerAngles; } }

	private const float differentialRatio = 4.41f;
	private float wheelRadius;

	private readonly float[] gearRatio = { 0, 3.5f, 2.5f, 1.8f, 1.4f, 1.1f, 0.8f, -3.6f };

	private ClearCheck clearCheck;
	private Transform checkPoint;
	private CountDown countDown;
	private InputManager inputManager;
	private Rigidbody rigidBody;

	private void Awake()
	{
		try { clearCheck = GetComponent<ClearCheck>(); }
		catch (NullReferenceException) { clearCheck = null; }

		try { countDown = FindObjectOfType<CountDown>(); }
		catch (NullReferenceException) { countDown = null; }
		
		inputManager = FindObjectOfType<InputManager>();
		
		rigidBody = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		wheelRadius = wheels.frontRight.radius;
		rigidBody.centerOfMass = transform.Find("Mass").localPosition;
	}

	private void Update()
	{
		if (countDown != null)
		{
			if (!countDown.CountDownEnd) return;
		}
		if (clearCheck != null)
		{
			if (clearCheck.isClear) return;
		}

		SetRPM(kmPerHour, beforeKMPerHour, ref rpm, ref beforeGear, inputManager.gear, inputManager.gas);

		SetKPH(ref kmPerHour, ref beforeKMPerHour);

		SetTorque(ref torque, rpm, inputManager.gear);

		LogitechWheelForce(kmPerHour);

		SteerVehicle(inputManager.horizontal);
	}

	private void FixedUpdate()
	{
		if (countDown != null)
		{
			if (!countDown.CountDownEnd) return;
		}
		if (clearCheck != null)
		{
			if (clearCheck.isClear)
			{
				wheels.frontLeft.motorTorque = 0;
				wheels.frontRight.motorTorque = 0;
				wheels.backLeft.motorTorque = 0;
				wheels.backRight.motorTorque = 0;
				wheels.frontLeft.brakeTorque = 40000;
				wheels.frontRight.brakeTorque = 40000;
				wheels.backLeft.brakeTorque = 40000;
				wheels.backRight.brakeTorque = 40000;
				rpm = Mathf.Clamp(rpm - Time.deltaTime * 10000, 0, rpmLimit);

				return;
			}
		}

		CheckPointTeleport(checkPoint, inputManager.respawn, ref rpm, ref kmPerHour, inputManager.horizontal);

		AddDownForce();

		AnimateHandle(ref handle, inputManager.horizontal);

		AnimateWheels();

		MoveVehicle();

		Brake();
	}

	private void SetRPM(float kmPerHour, int beforeKMPerHour, ref float rpm, ref int beforeGear, int gear, float gas)
	{
		if (Mathf.RoundToInt(kmPerHour) == 0 && beforeKMPerHour != 0)
		{
			rpm = 0;
			return;
		}

		float rpmVariance = Time.deltaTime * gas * gearRatio[gear] * differentialRatio * 50;
		
		rpm = Mathf.Clamp(rpm + (rpmVariance * (gas > 0 ? 1 : -1 / 2f)), 0, rpmLimit);

		if (gear != 0)
		{
			if (beforeGear != gear && rpm > 0)
			{
				rpm = Mathf.Clamp(rpm + (beforeGear - gear) * 500f, 0, rpmLimit);
			}

			beforeGear = gear;
		}
	}

	private void SetKPH(ref float kmPerHour, ref int beforeKMPerHour)
	{
		beforeKMPerHour = Mathf.RoundToInt(kmPerHour);

		kmPerHour = rigidBody.velocity.magnitude * 3.6f;
	}

	private void SetTorque(ref float torque, float rpm, int gear)
	{
		switch(gear)
		{
			case 1:
				torque = -1 / 9000f * Mathf.Pow(rpm - 6708.204f, 2)+ 5000;
				break;
			case 2:
				torque = -1 / 12856f * Mathf.Pow(rpm - 6707.906f, 2) + 3500;
				break;
			case 3:
				torque = -1 / 18000f * Mathf.Pow(rpm - 6708.204f, 2) + 2500;
				break;
			case 4:
				torque = -1 / 25000f * Mathf.Pow(rpm - 6708.204f, 2) + 1800;
				break;
			case 5:
				torque = -1 / 30000f * Mathf.Pow(rpm - 6708.204f, 2) + 1500;
				break;
			case 6:
				torque = -1 / 34615f * Mathf.Pow(rpm - 6708.167f, 2) + 1300;
				break;
			case 7:
				torque = -1 / 9782f * Mathf.Pow(rpm - 6708f, 2) + 4600;
				break;
		}
	}

	private void LogitechWheelForce(float kmPerHour)
	{
		if (!(LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))) return;

		LogitechGSDK.LogiPlayDamperForce(0, 70 - Mathf.RoundToInt(kmPerHour / 6));
	}

	private void Brake()
	{
		if (inputManager.clutch > 0) return;

		if (inputManager.brake > 0)
		{
			wheels.frontLeft.motorTorque = 0;
			wheels.frontRight.motorTorque = 0;
			wheels.backLeft.motorTorque = 0;
			wheels.backRight.motorTorque = 0;
			wheels.frontLeft.brakeTorque = 40000;
			wheels.frontRight.brakeTorque = 40000;
			wheels.backLeft.brakeTorque = 40000;
			wheels.backRight.brakeTorque = 40000;
			rpm = Mathf.Clamp(rpm - Time.deltaTime * 10000, 0, rpmLimit);
		}
	}

	private void MoveVehicle()
	{
		if (inputManager.clutch > 0) return;
		if (inputManager.brake > 0) return;

		int motorFrontSet = 0;
		int motorBackSet = 0;
		int brakeFrontSet = 0;
		int brakeBackSet = 0;

		switch (driveType)
		{
			case DriveType.AllWheelDrive:
				motorFrontSet = 4;
				motorBackSet = 4;
				brakeFrontSet = 4;
				brakeBackSet = 4;
				break;
			case DriveType.RearWheelDrive:
				motorFrontSet = 2;
				motorBackSet = 1;
				brakeFrontSet = 2;
				brakeBackSet = 1;
				break;
			case DriveType.FrontWheelDrive:
				motorFrontSet = 1;
				motorBackSet = 2;
				brakeFrontSet = 1;
				brakeBackSet = 2;
				break;
		}

		int kphLimit = 0;

		switch (inputManager.gear)
		{
			case 1:
				kphLimit = 30;
				break;
			case 2:
				kphLimit = 60;
				break;
			case 3:
				kphLimit = 120;
				break;
			case 4:
				kphLimit = 150;
				break;
			case 5:
				kphLimit = 190;
				break;
			case 6:
				kphLimit = 240;
				break;
			case 7:
				kphLimit = 30;
				break;
		}

		if (kmPerHour < kphLimit)
		{
			wheels.frontLeft.motorTorque = inputManager.gas * (torque / motorFrontSet);
			wheels.frontRight.motorTorque = inputManager.gas * (torque / motorFrontSet);
			wheels.backLeft.motorTorque = inputManager.gas * (torque / motorBackSet);
			wheels.backRight.motorTorque = inputManager.gas * (torque / motorBackSet);
		}
		else
		{
			wheels.frontLeft.motorTorque = 0;
			wheels.frontRight.motorTorque = 0;
			wheels.backLeft.motorTorque = 0;
			wheels.backRight.motorTorque = 0;
		}
		
		if (inputManager.gas == 0)
		{
			wheels.frontLeft.brakeTorque = inputManager.gas * (torque / brakeFrontSet);
			wheels.frontRight.brakeTorque = inputManager.gas * (torque / brakeFrontSet);
			wheels.backLeft.brakeTorque = inputManager.gas * (torque / brakeBackSet);
			wheels.backRight.brakeTorque = inputManager.gas * (torque / brakeBackSet);
		}
		else
		{
			wheels.frontLeft.brakeTorque = 0;
			wheels.frontRight.brakeTorque = 0;
			wheels.backLeft.brakeTorque = 0;
			wheels.backRight.brakeTorque = 0;
		}
	}

	private void SteerVehicle(float horizontal)
	{
		if (horizontal != 0)
		{
			wheels.frontLeft.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (wheelRadius + (1.5f / 2) * horizontal)) * horizontal;
			wheels.frontRight.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (wheelRadius - (1.5f / 2) * horizontal)) * horizontal;
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

	private void AnimateHandle(ref Transform handle, float horizontal)
	{
		Vector3 path = 450 * -horizontal * Vector3.forward;

		handle.localRotation = Quaternion.Euler(path);
		handle.eulerAngles = new Vector3(23.253f, handle.eulerAngles.y, handle.eulerAngles.z);
	}

	private void AddDownForce()
	{
		if (!rigidBody.useGravity) return;

		rigidBody.AddForce(downForceValue * rigidBody.velocity.magnitude * -transform.up);
	}

	public void CheckPointTeleport(Transform checkPoint, bool respawn, ref float rpm, ref float kmPerHour, float horizontal)
	{
		if (checkPoint == null) return;
		if (!respawn) return;

		StartCoroutine(WatingTime());

		rpm = 0;

		rigidBody.velocity = Vector3.zero;

		kmPerHour = 0;

		transform.SetPositionAndRotation(checkPoint.position, checkPoint.rotation);

		StartCoroutine(ResetLogitechWheel(horizontal));
	}

	private IEnumerator WatingTime()
	{
		rigidBody.useGravity = false;

		yield return new WaitForSeconds(1.5f);

		rigidBody.useGravity = true;
	}

	private IEnumerator ResetLogitechWheel(float horizontal)
	{
		LogitechGSDK.LogiPlaySpringForce(0, 0, 100, 100);

		while (horizontal > 0.05f || horizontal < -0.05f)
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