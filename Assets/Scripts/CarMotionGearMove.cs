using UnityEngine;

public class CarMotionGearMove : MonoBehaviour
{
	/// <summary>
	/// �ڵ����� �¿�� ������ ���� ǥ���ϴ� ����
	/// </summary>
	private float vibrationRollValue;

	private InputManager inputManager;
	private MotionGear motionGear;
	private Car car;

	private void Awake()
	{
		// �ܺ� ������Ʈ ���������� �Ҵ�

		inputManager = FindObjectOfType<InputManager>();
		motionGear = FindObjectOfType<MotionGear>();
		car = GetComponent<Car>();
	}

	private void Update()
	{
		// ������ ���� Roll, Pitch �� ���
		float bodyTlitPitch = (car.BodyTlit.x <= 180 ? -car.BodyTlit.x : 360 - car.BodyTlit.x) * 0.8f;
		float bodyTlitRoll = car.BodyTlit.z <= 180 ? car.BodyTlit.z : car.BodyTlit.z - 360;
		
		// RPM�� ���� ����
		motionGear.Vibration(Mathf.Clamp(car.rpm / 50, 0.5f, car.rpm / 50));

		// �극��ũ ��, ��Ǳ���� ������ ������ ��
		float brakeValue;
		if (inputManager.brake > 0 && car.rpm > 300)
		{
			// ���� ���� �Ҵ� ��, ������ ������ �ٲ�
			brakeValue = -6 * inputManager.brake + bodyTlitPitch;
		}
		else
		{
			brakeValue = 0;
		}

		// RPM�� ����Ͽ� ��, ��� ������ ���� ǥ��
		// vibrationRollValue = (���Ⱚ) * (���� ����)
		vibrationRollValue = (vibrationRollValue > 0 ? -1 : 1) * (Random.Range(0.05f, 0.2f) + car.rpm / 3500);

		motionGear.LeanMotionGear(bodyTlitPitch + brakeValue, vibrationRollValue + bodyTlitRoll);
	}
}