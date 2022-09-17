using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
	public enum driveType
    {
        FrontWheelDrive,
        RearWheelDrive,
        AllWheelDrive
    }

    public driveType drive;

    [Header("Variables")]
    public float nomalSpeed;
    public float boostSpeed;
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
    public GameManager gameManager;
    public InputManager inputManager;
    public Rigidbody rigid;

    private void Awake()
    {
		inputManager = GetComponent<InputManager>();
		rigid = GetComponent<Rigidbody>();

		rigid.centerOfMass = transform.Find("Mass").localPosition;

		motorMax = motorTorque;
		motorMin = motorMax / 2;
		limtSpeed = nomalSpeed;
	}

	private void Update()
	{
		if (!gameManager.gameStart) return;

		CheckPointTeleport();

		if (Input.GetKeyDown(KeyCode.Space))
		{
			boostFlag = true;
		}

		Drift();
	}

    private void FixedUpdate()
    {
        if (!gameManager.gameStart) return;
        
        AddDownForce();
        AnimateWheels();
        MoveVehicle();
        SteerVehicle();
        LimitMoveSpeed();
        Boost();
    }

	private void Drift()
	{
		if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftShift))) return;

		float stiffness = 0;

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
				print("AAAAAA");
				limtSpeed = boostSpeed;
				rigid.AddRelativeForce(Vector3.forward * 40000);
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

		wheels.frontLeft.motorTorque = inputManager.vertical * (motorTorque / 4);
		wheels.frontRight.motorTorque = inputManager.vertical * (motorTorque / 4);
		wheels.backLeft.motorTorque = inputManager.vertical * (motorTorque / 4);
		wheels.backRight.motorTorque = inputManager.vertical * (motorTorque / 4);

        if (inputManager.vertical > 0 && (KPH) < 150)
        {
            rigid.AddRelativeForce(Vector3.forward * 10000);
			wheels.backLeft.brakeTorque = 0;
			wheels.backRight.brakeTorque = 0;
        }
        else if(inputManager.vertical < 0)
        {
            rigid.AddRelativeForce(Vector3.back * 10000);
			wheels.backLeft.brakeTorque = 0;
			wheels.backRight.brakeTorque = 0;
		}
        else if(inputManager.vertical == 0)
        {
			wheels.backLeft.brakeTorque = breakPower;
			wheels.backRight.brakeTorque = breakPower;
		}
        
        KPH = rigid.velocity.magnitude * 3.6f;
    }

    private void SteerVehicle()
    {
        if(inputManager.horizontal > 0)
        {
            wheels.frontLeft.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * inputManager.horizontal;
            wheels.frontRight.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * inputManager.horizontal;
        }
        else if(inputManager.horizontal < 0)
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

    private void AddDownForce()
    {
        rigid.AddForce(-transform.up * downForceValue * rigid.velocity.magnitude);
    }

    private void LimitMoveSpeed()
    {
        if (rigid.velocity.x > limtSpeed)
        {
            rigid.velocity = new Vector3(limtSpeed, rigid.velocity.y, rigid.velocity.z);
        }
        if (rigid.velocity.x < (limtSpeed * -1))
        {
            rigid.velocity = new Vector3((limtSpeed * -1), rigid.velocity.y, rigid.velocity.z);
        }

        if (rigid.velocity.z > limtSpeed)
        {
            rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y, limtSpeed);
        }
        if (rigid.velocity.z < (limtSpeed * -1))
        {
            rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y, (limtSpeed * -1));
        }
    }

    private void CheckPointTeleport()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rigid.velocity = Vector3.zero;
            KPH = 0;
            transform.position = checkPoint.position;
            transform.rotation = checkPoint.rotation;
		}
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Points"))
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