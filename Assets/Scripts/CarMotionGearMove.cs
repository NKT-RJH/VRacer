using UnityEngine;

public class CarMotionGearMove : MonoBehaviour
{
	/// <summary>
	/// 자동차가 좌우로 떨리는 것을 표현하는 변수
	/// </summary>
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