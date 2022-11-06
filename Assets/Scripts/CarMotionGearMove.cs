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
	{// 진동값 높이기, 자동차 각도에 따라 회전하기, 부딪히면 소리 및 덜컹거리기
		float bodyTlitPitch = (car.BodyTlit.x > 0 ? -car.BodyTlit.x : 360 - car.BodyTlit.x) * 0.8f;
		float bodyTlitRoll = car.BodyTlit.z > 0 ? -car.BodyTlit.z : 360 - car.BodyTlit.z;

		motionGear.Vibration(Mathf.Clamp(car.KPH / 10, 0.5f, car.KPH / 10));

		if (inputManager.Brake > 0 && car.KPH > 10)
		{
			motionGear.LeanMotionGear(-6 * inputManager.Brake + bodyTlitPitch, null);
		}
		else
		{
			motionGear.LeanMotionGear(0 + bodyTlitPitch, null);
		}

		float driftValue = 0;

		if (inputManager.Drift && car.KPH > 20)
		{
			int leftRight = inputManager.Horizontal > 0 ? -1 : 1;
			driftValue = 5 * leftRight;
		}

		vibrationRollValue = (vibrationRollValue > 0 ? -1 : 1) * (Random.Range(0.05f, 0.2f) + car.KPH / 75);

		motionGear.LeanMotionGear(bodyTlitPitch, driftValue + vibrationRollValue + bodyTlitRoll);
	}
}
