# VRacer

<h4>나르샤 팀 Front Runner 프로젝트 (모션기어, VR, 레이싱 휠을 사용한 3D 레이싱 게임)</h4>

<hr class='hr-solid'/>

<h4>게임 시연</h4>

<A href="https://youtu.be/73nwsgYFb-k"> 베타테스트 게임 시연 영상 </A><br><p>

<h4>외부 자료</h4>

<A href="https://www.youtube.com/watch?v=0z_Cq4jISbE"> 2022 ICT 융합 엑스포 인터뷰 1 </A><br>
<A href="https://youtu.be/mk3mnzRFsNI?t=18893"> 2022 ICT 융합 엑스포 인터뷰 2 </A><br>
<A href="https://youtu.be/SjQXdJujxlM?t=279"> 2022 SoftWave </A>

<hr class='hr-solid'/>

<h3>시스템 구조</h3>

<details>
<summary><i>인 게임 이미지</i></summary>
<br>
 - 타이틀<br>
  <img width="640" alt="image" src="https://user-images.githubusercontent.com/80941288/230429856-b15159f7-1159-4335-9263-c53544391f06.png"><br>
  <img width="640" alt="image" src="https://user-images.githubusercontent.com/80941288/230729470-6c663521-b417-43b6-8258-0b73f9a43567.png"><br>
  <img width="640" alt="image" src="https://user-images.githubusercontent.com/80941288/230729507-45643499-dd69-427f-8e42-94bd80c126d2.png"><br>
  <img width="640" alt="image" src="https://user-images.githubusercontent.com/80941288/230729533-ba151e3a-f15d-4bdd-bd15-e608fa64073a.png"><br>
  <img width="640" alt="image" src="https://user-images.githubusercontent.com/80941288/230729575-fa266bbd-a9b7-4bfd-aae2-188df986d02b.png"><br>
  <br>
 - 설정<br>
  <img width="640" alt="image" src="https://user-images.githubusercontent.com/80941288/230704946-42d8ad15-3b5d-44ab-9917-3793fe37806d.png"><br>
  <br>
 - 튜토리얼<br>
  <img width="640" alt="image" src="https://user-images.githubusercontent.com/80941288/230704993-294eeb6f-8402-44c7-9000-888c8ee65a09.png"><br>
  <br>
 - 일반 플레이<br>
  <img width="640" alt="image" src="https://user-images.githubusercontent.com/80941288/230705063-c2d5aba9-e2db-486d-8400-afb959e8728a.png"><br>
  <img width="640" alt="image" src="https://user-images.githubusercontent.com/80941288/230705116-e7e63212-1f49-43e6-b4dc-e6edaf388dca.png"><br>
  <br>
</details>

<img width="640" alt="image" src="https://user-images.githubusercontent.com/80941288/230706345-be430afc-eede-4a3e-89e6-978c97f3518f.png"><br>
<img width="800" alt="image" src="https://user-images.githubusercontent.com/80941288/230706701-d20c93b6-2893-4599-8b4d-d378113a74c3.png">

<hr class='hr-solid'/>

<h3>주요 코드</h3>
<b>Car</b><br>
&nbsp;&nbsp;&nbsp;&nbsp;● 자동차를 움직이고 체크포인트로 이동하는 레이싱 게임의 메인클래스입니다.<br>
&nbsp;&nbsp;&nbsp;&nbsp;● 실제와 비슷한 작동을 위해서 RPM의 따른 토크 값을 기어별로 2차방정식을 통해 계산했습니다.<br>
<img width="640" alt="image" src="https://user-images.githubusercontent.com/80941288/230710776-f164bf76-d634-4624-9a74-3ae619b18d05.png">
<details>
    <summary><i>자세한 코드</i></summary>
    
  ```C#
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Car : MonoBehaviour
{
	// 자동차 구동방식 (전륜, 후륜, 사륜) 열거형
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

	// 자동차의 각도
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

	// 1 프레임 전 기어 값을 gear에 할당, 현재 rpm 값 설정
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

	// 1 프레임 전 KMPerHour 값을 beforeKMPerHour에 할당, 현재 KMPerHour 값 설정
	private void SetKMPerHour(ref float kmPerHour, ref int beforeKMPerHour)
	{
		beforeKMPerHour = Mathf.RoundToInt(kmPerHour);

		kmPerHour = rigidBody.velocity.magnitude * 3.6f;
	}

	// 기어와 RPM 따른 토크 값 설정
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

	// 속도에 따라 Logitech G29 핸들의 뻑뻑한 강도 조절
	private void LogitechWheelForce(float kmPerHour)
	{
		if (!(LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))) return;

		LogitechGSDK.LogiPlayDamperForce(0, 70 - Mathf.RoundToInt(kmPerHour / 6));
	}

	// 브레이크 입력 시, 브레이크 토크 증가 및 RPM 감소
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

	// 구동방식, 엑셀 입력, 토크에 따라 자동차 이동(힘을 가함)
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

	// 핸들 값에 따라 WheelCollider 각도 조절
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

	// 실질적으로 보이는 바퀴 오브젝트의 위치, 각도를 WheelCollider와 동기화
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

	// 핸들 값에 따라 인 게임 핸들 각도 설정
	private void AnimateHandle(ref Transform handle, float horizontal)
	{
		Vector3 path = 450 * -horizontal * Vector3.forward;

		handle.localRotation = Quaternion.Euler(path);
		handle.eulerAngles = new Vector3(23.253f, handle.eulerAngles.y, handle.eulerAngles.z);
	}

	// 속도가 높아지면 자동차가 아래로 힘이 가해지는 것을 구현
	private void AddDownForce()
	{
		if (!rigidBody.useGravity) return;

		rigidBody.AddForce(downForceValue * rigidBody.velocity.magnitude * -transform.up);
	}

	// 입력이 들어오면 체크포인트로 이동
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

	// 체크포인트로 이동 후 1.5초 동안 대기
	private IEnumerator WatingTime()
	{
		rigidBody.useGravity = false;

		yield return new WaitForSeconds(1.5f);

		rigidBody.useGravity = true;
	}

	// Logitech G29 핸들 원위치
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
  ```
</details><br>

<b>MotionGear</b><br>
&nbsp;&nbsp;&nbsp;&nbsp;● 모션하우스 모션기어의 유니티 에셋을 본 게임에 최적화 시킨 클래스입니다.<br>
&nbsp;&nbsp;&nbsp;&nbsp;● 모션기어를 움직입니다.
<details>
    <summary><i>자세한 코드</i></summary>
    
  ```C#
using MotionHouse;
using UnityEngine;

// 유니티의 모든 씬에서 사용할 수 있게 제가 만든 싱글톤 클래스를 상속했습니다.
public class MotionGear : Singleton<MotionGear>
{
	// 모션기어 앞, 뒤 기울임 값
	private float roll;
	// 모션기어 좌, 우 기울임 값
	private float pitch;
	// 모션기어 위, 아래 이동 값
	private float vibration;

	private bool isEnabled;
	private bool isVibration;

	protected override void Awake()
	{
		base.Awake();

		// 모션기어 실행 함수 한 번만 활성화
		if (isEnabled) return;

		MotionHouseSDK.MHRun();
		MotionHouseSDK.MotionControlStart();

		isEnabled = true;
	}

	private void Update()
	{
		// 모션기어의 위, 아래 이동 방향 변경
		if (isVibration)
		{
			vibration *= -1;
		}

		// 값을 실질적으로 모션기어를 움직이는 함수에 전달
		// 한 변수라도 값이 0이라면 전달하지 않음
		if (roll != 0 || pitch != 0 || vibration != 0)
		{
			MotionHouseSDK.MotionTelemetry(roll, pitch, 0, 0, vibration, 0, 0);
		}
	}

	// Roll, Pitch에 범위 내의 값 할당
	public void LeanMotionGear(float? pitch, float? roll)
	{
		if (roll != null)
		{
			this.roll = Mathf.Clamp((float)roll, -18, 18);
		}
		if (pitch != null)
		{
			this.pitch = Mathf.Clamp((float)pitch, -18, 18);
		}
	}

	// Roll, Pitch 값 초기화
	public void StopLeanMotionGear()
	{
		roll = 0;
		pitch = 0;
	}

	// Vibration 값 할당 및 모션기어의 위, 아래 이동 방향 변경 활성화
	public void Vibration(float value)
	{
		vibration = value;

		isVibration = true;
	}

	// Vibration 초기화 및 모션기어의 위, 아래 이동 방향 변경 비활성화
	public void StopVibration()
	{
		vibration = Mathf.Abs(vibration);

		isVibration = false;
	}

	// 프로그램 종료 시 모션기어 종료
	private void OnApplicationQuit()
	{
		MotionHouseSDK.MotionControlEnd();
		MotionHouseSDK.MHStop();
	}
}
  ```
</details><br>

<b>CarMotionGearMove</b><br>
&nbsp;&nbsp;&nbsp;&nbsp;● 인 게임 내의 값을 MotionGear 클래스에게 전달하는 클래스입니다.<br>
<details>
    <summary><i>자세한 코드</i></summary>
    
  ```C#
using UnityEngine;

public class CarMotionGearMove : MonoBehaviour
{
	// 자동차가 좌우로 떨리는 것을 표현하는 변수
	private float vibrationRollValue;

	private InputManager inputManager;
	private MotionGear motionGear;
	private Car car;

	private void Awake()
	{
		// 외부 컴포넌트 전역변수에 할당

		inputManager = FindObjectOfType<InputManager>();
		motionGear = FindObjectOfType<MotionGear>();
		car = GetComponent<Car>();
	}

	private void Update()
	{
		// 각도에 따른 Roll, Pitch 값 계산
		float bodyTlitPitch = (car.BodyTlit.x <= 180 ? -car.BodyTlit.x : 360 - car.BodyTlit.x) * 0.8f;
		float bodyTlitRoll = car.BodyTlit.z <= 180 ? car.BodyTlit.z : car.BodyTlit.z - 360;
		
		// RPM에 따른 진동
		motionGear.Vibration(Mathf.Clamp(car.rpm / 50, 0.5f, car.rpm / 50));

		// 브레이크 시, 모션기어의 각도가 앞으로 쏠림
		float brakeValue;
		if (inputManager.brake > 0 && car.rpm > 300)
		{
			// 음수 값을 할당 시, 앞으로 각도가 바뀜
			brakeValue = -6 * inputManager.brake + bodyTlitPitch;
		}
		else
		{
			brakeValue = 0;
		}

		// RPM에 기반하여 좌, 우로 떨리는 것을 표현
		// vibrationRollValue = (방향값) * (진동 세기)
		vibrationRollValue = (vibrationRollValue > 0 ? -1 : 1) * (Random.Range(0.05f, 0.2f) + car.rpm / 3500);

		motionGear.LeanMotionGear(bodyTlitPitch + brakeValue, vibrationRollValue + bodyTlitRoll);
	}
}
  ```
</details><br>

<b>LogitechInput</b><br>
&nbsp;&nbsp;&nbsp;&nbsp;● Logitech 기기의 입력을 인식하는 클래스입니다.<br>
<details>
    <summary><i>자세한 코드</i></summary>
    
  ```C#
public class LogitechInput
{
	// Logitech 기기 입력 값
	static LogitechGSDK.DIJOYSTATE2ENGINES rec;

	// Steering = Steering Horizontal, GasInput / Accelerator = Gas Vertical, CluthInput = Clutch Vertical and BrakeInput = Brake Vertical

	// Axis의 이름에 따른 로지텍 기기 입력 값 반환 (Steering Horizontal, Gas Vertical, Clutch Vertical, Brake Vertical)
	public static float GetAxis(string axisName)
	{
		// rec에 현재 로지텍 기기 값 할당
		rec = LogitechGSDK.LogiGetStateUnity(0);

		// Axis 이름 확인 및 값 조정 후 반환
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

	// 로지텍 기기의 해당 버튼을 누르고 있는 상태인지 Bool 값으로 반환
	public static bool GetKeyTriggered(LogitechKeyCode gameController, LogitechKeyCode keyCode)
	{
		return LogitechGSDK.LogiButtonTriggered((int)gameController, (int)keyCode);
	}

	// 로지텍 기기의 해당 버튼을 눌렀는지 Bool 값으로 반환
	public static bool GetKeyPresssed(LogitechKeyCode gameController, LogitechKeyCode keyCode)
	{
		return LogitechGSDK.LogiButtonIsPressed((int)gameController, (int)keyCode);
	}

	// 로지텍 기기의 해당 버튼을 떼었는지 Bool 값으로 반환
	public static bool GetKeyReleased(LogitechKeyCode gameController, LogitechKeyCode keyCode)
	{
		return LogitechGSDK.LogiButtonReleased((int)gameController, (int)keyCode);
	}
}
  ```
</details><br>

<b>GraphicSetting</b><br>
&nbsp;&nbsp;&nbsp;&nbsp;● 플레이어가 저장한 그래픽 설정을 세이브파일에서 가져와 씬에 적용하는 클래스입니다.<br>
<details>
    <summary><i>자세한 코드</i></summary>
    
  ```C#
  using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

// 유니티의 모든 씬에서 활용할 수 있도록 싱글톤 클래스를 상속했습니다.
public class GraphicSetting : Singleton<GraphicSetting>
{
	// 씬이 로딩되자마자 실행
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// HDRP 그래픽 설정을 담당하는 Volume과 Camera 정보 지역변수에 할당
		Volume volume = FindObjectOfType<Volume>();
		HDAdditionalCameraData[] hdAdditionalCameraDatas = FindObjectsOfType<HDAdditionalCameraData>();
		
		// 안개(Fog) 설정
		// VolumeProfile에서 Fog 클래스 가져오기
		if (volume.profile.TryGet(out Fog fog))
		{
			// 설정 덮어쓰기 활성화
			fog.enabled.overrideState = true;
			// 세이브파일에서 Bool 형태로 불러온 안개 활성 여부 입력
			fog.enabled.value = OptionData.fog;
		}

		// 모션 블러(Motion Blur) 설정
		// VolumeProfile에서 MotionBlur 클래스 가져오기
		if (volume.profile.TryGet(out MotionBlur motionBlur))
		{
			motionBlur.intensity.overrideState = true;
			// 세이브파일 Bool 값에 따라 모션 블러 수치 조정
			if (OptionData.motionBlur)
			{
				motionBlur.intensity.value = 1.5f;
			}
			else
			{
				motionBlur.intensity.value = 0;
			}
		}

		// 블룸(Bloom) 설정
		// VolumeProfile에서 Bloom 클래스 가져오기
		if (volume.profile.TryGet(out Bloom bloom))
		{
			bloom.intensity.overrideState = true;
			// 세이브파일 Bool 값에 따라 블룸 수치 조정
			if (OptionData.bloom)
			{
				bloom.intensity.value = 0.3f;
			}
			else
			{
				bloom.intensity.value = 0;
			}
		}

		// 안티앨리어싱 설정
		for (int count = 0; count < hdAdditionalCameraDatas.Length; count++)
		{
			// 씬 내 모든 카메라에 세이브파일 Bool 값에 따라 활성화 여부 입력
			HDAdditionalCameraData.AntialiasingMode antialiasingMode = OptionData.antiAliasing ? HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing : HDAdditionalCameraData.AntialiasingMode.None;
			hdAdditionalCameraDatas[count].antialiasing = antialiasingMode;
		}
	}
}
  ```
</details>
