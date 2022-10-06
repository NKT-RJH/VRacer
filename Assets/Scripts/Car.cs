using System.Collections;
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

	public DriveType drive;

	[Header("Variables")]
	public float nomalSpeed;
	public float boostSpeed;
	public Transform handle;
	public Wheels wheels;
	public WheelMeshs wheelPaths;

	private int motorMax;
	private int motorMin;
	private int motorTorque = 100;

	public float KPH = 0;
	private float breakPower = 7000;
	private float radius = 6;
	private float downForceValue = 50;
	private float limtSpeed;

	private bool isBoost = false;
	private bool boostFlag = false;

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
	}

	private void FixedUpdate()
	{
		if (!gameManager.gameStart) return;

		AddDownForce();
		AnimateHandle();
		AnimateWheels();
		MoveVehicle();
		SteerVehicle();
		LimitMoveSpeed();
		//Boost();
	}

	private void Drift()
	{
		float stiffness = 0;

		if (inputManager.inputCondition == InputCondition.Driving)
		{
			if (inputManager.o)
			{
				motorTorque = motorMin;
				stiffness = 1;
			}
			else
			{
				motorTorque = motorMax;
				stiffness = 2;
			}
		}
		else if (inputManager.inputCondition == InputCondition.KeyBoard)
		{
			if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftShift))) return;
			
			if (Input.GetKey(KeyCode.LeftShift))
			{
				motorTorque = motorMin;
				stiffness = 1;
			}
			if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				motorTorque = motorMax;
				stiffness = 2;
			}
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

	private void Boost()
	{
		if (boostFlag)
		{
			boostFlag = false;
			if (!isBoost)
			{
				limtSpeed = boostSpeed;
				rigidBody.AddRelativeForce(Vector3.forward * 40000);
				StartCoroutine(BoostStart());
			}
		}
	}

	private IEnumerator BoostStart()
	{
		isBoost = true;
		// 부스트 왜 안되는지 확인하기 아마 맥스 값이 원인인 듯 왜 안되냐 
		yield return new WaitForSeconds(2.5f);
		for (float i = limtSpeed; i >= nomalSpeed; i -= 1.0f)
		{
			limtSpeed = i;
			yield return new WaitForSeconds(0.05f);
		}
		limtSpeed = nomalSpeed;
		isBoost = false;
	}


	private void MoveVehicle()
	{
		/// 이건 전륜 후륜 조절  기능 넣기!
		//      int startSet = 0;
		//      int endSet = 0;

		//      switch (drive)
		//      {
		//          case driveType.allWheelDrive:
		//              startSet = 0;
		//              endSet = 0;
		//              break;
		//	case driveType.rearWheelDrive:
		//		startSet = 2;
		//		endSet = 0;
		//		break;
		//	case driveType.frontWheelDrive:
		//		startSet = 0;
		//		endSet = -2;
		//		break;
		//}


		if (inputManager.inputCondition == InputCondition.KeyBoard)
		{
			wheels.frontLeft.motorTorque = inputManager.vertical * (motorTorque / 4);
			wheels.frontRight.motorTorque = inputManager.vertical * (motorTorque / 4);
			wheels.backLeft.motorTorque = inputManager.vertical * (motorTorque / 4);
			wheels.backRight.motorTorque = inputManager.vertical * (motorTorque / 4);

			if (inputManager.vertical > 0 && (KPH) < 150)
			{
				rigidBody.AddRelativeForce(Vector3.forward * 10000);
				wheels.backLeft.brakeTorque = 0;
				wheels.backRight.brakeTorque = 0;
			}
			else if (inputManager.vertical < 0)
			{
				rigidBody.AddRelativeForce(Vector3.back * 10000);
				wheels.backLeft.brakeTorque = 0;
				wheels.backRight.brakeTorque = 0;
			}
			else if (inputManager.vertical == 0)
			{
				wheels.backLeft.brakeTorque = breakPower;
				wheels.backRight.brakeTorque = breakPower;
			}
		}
		else if (inputManager.inputCondition == InputCondition.Driving)
		{
			wheels.frontLeft.motorTorque = (inputManager.gas + inputManager.brake) * (motorTorque / 4);
			wheels.frontRight.motorTorque = (inputManager.gas + inputManager.brake) * (motorTorque / 4);
			wheels.backLeft.motorTorque = (inputManager.gas + inputManager.brake) * (motorTorque / 4);
			wheels.backRight.motorTorque = (inputManager.gas + inputManager.brake) * (motorTorque / 4);

			if (inputManager.gas > 0 && KPH < 150)
			{
				rigidBody.AddRelativeForce(Vector3.forward * 10000 * inputManager.gas);
				wheels.backLeft.brakeTorque = 0;
				wheels.backRight.brakeTorque = 0;
			}
			else if (inputManager.gas < 0 && KPH < 150)
			{
				wheels.backLeft.brakeTorque = breakPower;
				wheels.backRight.brakeTorque = breakPower;
			}
			if (inputManager.brake > 0)
			{
				rigidBody.AddRelativeForce(Vector3.back * 10000 * inputManager.brake);
				wheels.backLeft.brakeTorque = 0;
				wheels.backRight.brakeTorque = 0;
			}
		}

		KPH = rigidBody.velocity.magnitude * 3.6f;
	}

	private void SteerVehicle()
	{
		if (inputManager.inputCondition == InputCondition.KeyBoard)
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
		else if (inputManager.inputCondition == InputCondition.Driving)
		{
			if (inputManager.steer > 0)
			{
				wheels.frontLeft.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * inputManager.steer;
				wheels.frontRight.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * inputManager.steer;
			}
			else if (inputManager.steer < 0)
			{
				wheels.frontLeft.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * inputManager.steer;
				wheels.frontRight.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * inputManager.steer;
			}
			else
			{
				wheels.frontLeft.steerAngle = 0;
				wheels.frontRight.steerAngle = 0;
			}
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
		Vector3 path = Vector3.zero;
		if (inputManager.inputCondition == InputCondition.KeyBoard)
		{
			path = Vector3.up * inputManager.horizontal;
		}
		else if (inputManager.inputCondition == InputCondition.Driving)
		{
			path = Vector3.up * inputManager.steer;
		}
		if (path == Vector3.zero) return;

		handle.rotation = Quaternion.LookRotation(path);
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
		if (inputManager.inputCondition == InputCondition.KeyBoard)
		{
			if (!Input.GetKeyDown(KeyCode.R)) return;
		}
		else if (inputManager.inputCondition == InputCondition.Driving)
		{
			if (!inputManager.t) return;
		}

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