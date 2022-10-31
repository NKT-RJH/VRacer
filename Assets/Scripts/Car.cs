using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputManager))]
public class Car : MonoBehaviour
{
	public enum DriveType
	{
		FrontWheelDrive,
		RearWheelDrive,
		AllWheelDrive
	}

	public DriveType driveType;

	[Header("Variables")]
	public float nomalSpeed;
	public float boostSpeed;
	public Transform handle;
	public Wheels wheels;
	public WheelMeshs wheelPaths;

	[Header("Sounds")]
	public AudioClip idle;

	private int motorMax;
	private int motorMin;
	private int motorTorque = 100;

	public float KPH = 0;
	private float KPTLimit = 150;
	private float power = 10000;
	private float brakePower = 7000;
	private float radius = 6;
	private float downForceValue = 50;
	private float limtSpeed;

	//private bool isBoost = false;
	//private bool boostFlag = false;

	private Transform checkPoint;
	private GameManager gameManager;
	private InputManager inputManager;
	private Rigidbody rigidBody;

	private void Awake()
	{
		gameManager = FindObjectOfType<GameManager>();

		if (!gameManager) enabled = false;

		inputManager = GetComponent<InputManager>();
		rigidBody = GetComponent<Rigidbody>();

		rigidBody.centerOfMass = transform.Find("Mass").localPosition;

		motorMax = motorTorque;
		motorMin = motorMax / 2;
		limtSpeed = nomalSpeed;
	}

	private void Update()
	{
		if (!gameManager.gameStart) return;

		CheckPointTeleport();

		//if (Input.GetKeyDown(KeyCode.Space))
		//{
		//	boostFlag = true;
		//}
		Drift();
		//if (audioSource.clip == null) return;
		//if (audioSource.time >= audioSource.clip.length -0.5f)
		//{
		//audioSource.clip = null;
		//}
	}

	private void FixedUpdate()
	{
		if (!gameManager.gameStart) return;

		Brake();
		AddDownForce();
		AnimateHandle();
		AnimateWheels();
		MoveVehicle();
		SteerVehicle();
		LimitMoveSpeed();
		//Boost();
	}

	private void Brake()
	{
		if (inputManager.clutch > 0) return;

		if (inputManager.brake <= 0) return;

		if (KPH < 10) return;

		rigidBody.AddRelativeForce(40000 * Vector3.back);
		wheels.backLeft.brakeTorque = brakePower;
		wheels.backRight.brakeTorque = brakePower;
	}

	private void Drift()
	{
		if (inputManager.clutch > 0) return;

		float stiffness = 0;

		if (inputManager.drift)
		{
			motorTorque = motorMin;
			stiffness = 1.4f;
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

	//private void Boost()
	//{
	//	if (boostFlag)
	//	{
	//		boostFlag = false;
	//		if (!isBoost)
	//		{
	//			limtSpeed = boostSpeed;
	//			rigidBody.AddRelativeForce(Vector3.forward * 40000);
	//			StartCoroutine(BoostStart());
	//		}
	//	}
	//}

	//private IEnumerator BoostStart()
	//{
	//	isBoost = true;
	//	// 부스트 왜 안되는지 확인하기 아마 맥스 값이 원인인 듯 왜 안되냐 
	//	yield return new WaitForSeconds(2.5f);
	//	for (float i = limtSpeed; i >= nomalSpeed; i -= 1.0f)
	//	{
	//		limtSpeed = i;
	//		yield return new WaitForSeconds(0.05f);
	//	}
	//	limtSpeed = nomalSpeed;
	//	isBoost = false;
	//}


	private void MoveVehicle()
	{
		if (inputManager.gear == 0) return;
		if (inputManager.clutch > 0) return;

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

		switch (inputManager.gear)
		{
			case 1:
				KPTLimit = 30;
				power = 3000;
				break;
			case 2:
				KPTLimit = 50;
				power = 4000;
				break;
			case 3:
				KPTLimit = 80;
				power = 6500;
				break;
			case 4:
				KPTLimit = 110;
				power = 9000;//속도 수치 조절
				break;
			case 5:
				KPTLimit = 140;
				power = 9500;
				break;
			case 6:
				KPTLimit = 160;
				power = 10000;
				break;
			case 7:
				KPTLimit = 10;
				power = 7500;
				break;
		}
		wheels.frontLeft.motorTorque = inputManager.gas * (motorTorque / startSet);
		wheels.frontRight.motorTorque = inputManager.gas * (motorTorque / startSet);
		wheels.backLeft.motorTorque = inputManager.gas * (motorTorque / endSet);
		wheels.backRight.motorTorque = inputManager.gas * (motorTorque / endSet);

		if (inputManager.gas > 0 && inputManager.gear < 7 && inputManager.brake < 0 && KPH < 150)
		{
			rigidBody.AddRelativeForce(power * inputManager.gas * Vector3.forward);
			wheels.backLeft.brakeTorque = 0;
			wheels.backRight.brakeTorque = 0;
		}
		else if (inputManager.gas < 0 && inputManager.gear < 7 && KPH < KPTLimit)
		{
			wheels.backLeft.brakeTorque = brakePower;
			wheels.backRight.brakeTorque = brakePower;
		}

		if (inputManager.gear == 7 && inputManager.gas > 0 && KPH < KPTLimit)
		{
			rigidBody.AddRelativeForce(power * inputManager.gas * Vector3.back);
			wheels.backLeft.brakeTorque = 0;
			wheels.backRight.brakeTorque = 0;
		}

		KPH = rigidBody.velocity.magnitude * 3.6f;
	}

	private void SteerVehicle()
	{
		if (inputManager.horizontal > 0)
		{
			wheels.frontLeft.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * inputManager.horizontal;
			wheels.frontRight.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * inputManager.horizontal;
		}
		else if (inputManager.horizontal < 0)
		{
			wheels.frontLeft.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * inputManager.horizontal;
			wheels.frontRight.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * inputManager.horizontal;
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
		Vector3 path = 450 * -inputManager.horizontal * Vector3.forward;

		if (path == Vector3.zero) return;

		handle.localRotation = Quaternion.Euler(path);
	}

	private void AddDownForce()
	{
		rigidBody.AddForce(-transform.up * downForceValue * rigidBody.velocity.magnitude);
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
		//if (inputManager.inputCondition == InputCondition.KeyBoard)
		//{
		//	if (!Input.GetKeyDown(KeyCode.R)) return;
		//}
		//else if (inputManager.inputCondition == InputCondition.Driving)
		//{
		//	if (!inputManager.t) return;
		//}
		if (!inputManager.respawn) return;

		rigidBody.velocity = Vector3.zero;
		KPH = 0;
		transform.position = checkPoint.position;
		transform.rotation = checkPoint.rotation;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("CheckPoint"))
		{
			checkPoint = other.transform;
		}
	}
}

[System.Serializable]
public class Wheels
{
	public WheelCollider frontLeft;
	public WheelCollider frontRight;
	public WheelCollider backLeft;
	public WheelCollider backRight;
}

[System.Serializable]
public class WheelMeshs
{
	public Transform frontLeft;
	public Transform frontRight;
	public Transform backLeft;
	public Transform backRight;
}