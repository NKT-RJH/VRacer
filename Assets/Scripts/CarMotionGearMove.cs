using UnityEngine;

public class CarMotionGearMove : MonoBehaviour
{
	private float vibrationRollValue;

	private InputManager inputManager;
	private MotionGear motionGear;
	private Car car;

	private void Awake()
	{
		inputManager = FindObjectOfType<InputManager>();
		motionGear = FindObjectOfType<MotionGear>();
		car = GetComponent<Car>();
	}

	private void Update()
	{// ������ ���̱�, �ڵ��� ������ ���� ȸ���ϱ�, �ε����� �Ҹ� �� ���ȰŸ���
	 // �� ���̱�
		float bodyTlitPitch = (car.BodyTlit.x <= 180 ? -car.BodyTlit.x : 360 - car.BodyTlit.x) * 0.8f;
		float bodyTlitRoll = car.BodyTlit.z <= 180 ? car.BodyTlit.z : car.BodyTlit.z - 360;

		float brakeValue = 0;

		motionGear.Vibration(Mathf.Clamp(car.rpm / 50, 0.5f, car.rpm / 50));

		if (inputManager.brake > 0 && car.rpm > 300)
		{
			brakeValue = -6 * inputManager.brake + bodyTlitPitch;
		}
		else
		{
			brakeValue = 0;
		}

		float driftValue = 0;

		if (inputManager.drift && car.rpm > 500)
		{
			int leftRight = inputManager.horizontal > 0 ? -1 : 1;
			driftValue = 5 * leftRight;
		}

		vibrationRollValue = (vibrationRollValue > 0 ? -1 : 1) * (Random.Range(0.05f, 0.2f) + car.rpm / 3500);

		motionGear.LeanMotionGear(bodyTlitPitch + brakeValue, driftValue + vibrationRollValue + bodyTlitRoll);
	}
}
