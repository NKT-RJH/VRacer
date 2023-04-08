using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Car : MonoBehaviour
{
	/// <summary>
	/// 자동차 구동방식 (전륜, 후륜, 사륜) 열거형
	/// </summary>
	public enum DriveType
	{
		FrontWheelDrive,
		RearWheelDrive,
		AllWheelDrive
	}

	// 오브젝트 캐싱
	[Header("Cashing")]
	[SerializeField] private Transform handle;
	[SerializeField] private Wheels wheels;
	[SerializeField] private WheelMeshs wheelPaths;

	[Header("Value")]
	[SerializeField] private DriveType driveType;
	public float rpm;
	public float kmPerHour;

	private const float rpmLimit = 9000;
	private const float downForceValue = 50;
	private const float differentialRatio = 4.41f;
	private readonly float[] gearRatio = { 0, 3.5f, 2.5f, 1.8f, 1.4f, 1.1f, 0.8f, -3.6f };

	/// <summary>
	/// 자동차의 각도
	/// </summary>
	public Vector3 BodyTlit { get { return transform.localEulerAngles; } }

	private int beforeGear = 0;
	private int beforeKMPerHour;
	private float wheelRadius;
	private float torque = 0; //Nm 단위

	private ClearCheck clearCheck;
	private Transform checkPoint;
	private CountDown countDown;
	private InputManager inputManager;
	private Rigidbody rigidBody;

	private void Awake()
	{
		// 외부 컴포넌트 전역변수에 할당
		
		try { clearCheck = GetComponent<ClearCheck>(); }
		catch (NullReferenceException) { clearCheck = null; }

		try { countDown = FindObjectOfType<CountDown>(); }
		catch (NullReferenceException) { countDown = null; }

		inputManager = FindObjectOfType<InputManager>();

		rigidBody = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		// 바퀴 반지름, 물체 중심 변수 초기화

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

		SetRPMAndBeforeGear(kmPerHour, beforeKMPerHour, ref rpm, ref beforeGear, inputManager.gear, inputManager.gas);

		SetKMPerHour(ref kmPerHour, ref beforeKMPerHour);

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
			// 게임이 끝나면 자동차 정지
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

	/// <summary>
	/// 1 프레임 전 기어 값을 gear에 할당, 현재 rpm 값 설정
	/// </summary>
	/// <param name="kmPerHour"></param>
	/// <param name="beforeKMPerHour"></param>
	/// <param name="rpm"></param>
	/// <param name="beforeGear"></param>
	/// <param name="gear"></param>
	/// <param name="gas"></param>
	private void SetRPMAndBeforeGear(float kmPerHour, int beforeKMPerHour, ref float rpm, ref int beforeGear, int gear, float gas)
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

	/// <summary>
	/// 1 프레임 전 KMPerHour 값을 beforeKMPerHour에 할당, 현재 KMPerHour 값 설정
	/// </summary>
	/// <param name="kmPerHour"></param>
	/// <param name="beforeKMPerHour"></param>
	private void SetKMPerHour(ref float kmPerHour, ref int beforeKMPerHour)
	{
		beforeKMPerHour = Mathf.RoundToInt(kmPerHour);

		kmPerHour = rigidBody.velocity.magnitude * 3.6f;
	}

	/// <summary>
	/// 기어와 RPM 따른 토크 값 설정
	/// </summary>
	/// <param name="torque"></param>
	/// <param name="rpm"></param>
	/// <param name="gear"></param>
	private void SetTorque(ref float torque, float rpm, int gear)
	{
		// 2차 방정식을 활용했습니다.
		switch (gear)
		{
			case 0:
				torque = 0;
				break;
			case 1:
				torque = -1 / 9000f * Mathf.Pow(rpm - 6708.204f, 2) + 5000;
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

	/// <summary>
	/// 속도에 따라 Logitech G29 핸들의 뻑뻑한 강도 조절
	/// </summary>
	/// <param name="kmPerHour"></param>
	private void LogitechWheelForce(float kmPerHour)
	{
		if (!(LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))) return;

		LogitechGSDK.LogiPlayDamperForce(0, 70 - Mathf.RoundToInt(kmPerHour / 6));
	}

	/// <summary>
	/// 브레이크 입력 시, 브레이크 토크 증가 및 RPM 감소
	/// </summary>
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

	/// <summary>
	/// 구동방식, 엑셀 입력, 토크에 따라 자동차 이동(힘을 가함)
	/// </summary>
	private void MoveVehicle()
	{
		if (inputManager.clutch > 0) return;
		if (inputManager.brake > 0) return;

		// 구동방식에 따른 값 할당
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

		// 기어에 따른 속도 제한
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

		// 시속 제한을 넘지 않고 기어 N단이 아니면 토크에 값 할당
		// WheelCollider.motorTorque에 값을 입력하면 해당 토크에 따라 바퀴가 회전합니다.
		if (kmPerHour < kphLimit && inputManager.gear != 0)
		{
			// 7단이라면 뒤로 바퀴를 회전하여 후진
			int direction = inputManager.gear == 7 ? -1 : 1;

			wheels.frontLeft.motorTorque = inputManager.gas * (torque / motorFrontSet) * direction;
			wheels.frontRight.motorTorque = inputManager.gas * (torque / motorFrontSet) * direction;
			wheels.backLeft.motorTorque = inputManager.gas * (torque / motorBackSet) * direction;
			wheels.backRight.motorTorque = inputManager.gas * (torque / motorBackSet) * direction;
		}
		else
		{
			wheels.frontLeft.motorTorque = 0;
			wheels.frontRight.motorTorque = 0;
			wheels.backLeft.motorTorque = 0;
			wheels.backRight.motorTorque = 0;
		}
		
		// 엑셀을 밟지 않으면 자동차가 서서히 멈춤
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

	/// <summary>
	/// 핸들 값에 따라 WheelCollider 각도 조절
	/// </summary>
	/// <param name="horizontal"></param>
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

	/// <summary>
	/// 실질적으로 보이는 바퀴 오브젝트의 위치, 각도를 WheelCollider와 동기화
	/// </summary>
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

	/// <summary>
	/// 핸들 값에 따라 인 게임 핸들 각도 설정
	/// </summary>
	/// <param name="handle"></param>
	/// <param name="horizontal"></param>
	private void AnimateHandle(ref Transform handle, float horizontal)
	{
		Vector3 path = 450 * -horizontal * Vector3.forward;

		handle.localRotation = Quaternion.Euler(path);
		handle.eulerAngles = new Vector3(23.253f, handle.eulerAngles.y, handle.eulerAngles.z);
	}

	/// <summary>
	/// 속도가 높아지면 자동차가 아래로 힘이 가해지는 것을 구현
	/// </summary>
	private void AddDownForce()
	{
		if (!rigidBody.useGravity) return;

		rigidBody.AddForce(downForceValue * rigidBody.velocity.magnitude * -transform.up);
	}

	/// <summary>
	/// 입력이 들어오면 체크포인트로 이동
	/// </summary>
	/// <param name="checkPoint"></param>
	/// <param name="respawn"></param>
	/// <param name="rpm"></param>
	/// <param name="kmPerHour"></param>
	/// <param name="horizontal"></param>
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

	/// <summary>
	/// 체크포인트로 이동 후 1.5초 동안 대기
	/// </summary>
	/// <returns></returns>
	private IEnumerator WatingTime()
	{
		rigidBody.useGravity = false;

		yield return new WaitForSeconds(1.5f);

		rigidBody.useGravity = true;
	}

	/// <summary>
	/// Logitech G29 핸들 원위치
	/// </summary>
	/// <param name="horizontal"></param>
	/// <returns></returns>
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
		// 체크포인트에 닿으면 Transform(각도, 위치) 저장
		if (other.CompareTag("CheckPoint"))
		{
			checkPoint = other.transform;
		}
	}
}