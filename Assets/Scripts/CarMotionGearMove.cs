using UnityEngine;

public class CarMotionGearMove : MonoBehaviour
{
	private float vibrationRollValue;
	private float driftValue;

	private InputManager inputManager;
	private MotionGear motionGear;
	private CarMove car;

	private void Awake()
	{
		inputManager = FindObjectOfType<InputManager>();
		motionGear = FindObjectOfType<MotionGear>();
		car = FindObjectOfType<CarMove>();
	}

	private void Update()
	{// 진동값 높이기, 자동차 각도에 따라 회전하기, 부딪히면 소리 및 덜컹거리기
		motionGear.Vibration(Mathf.Clamp((car.KPH + 1) / 30, 0.5f, (car.KPH + 1) / 30));

		if (inputManager.Brake > 0)
		{
			//15에서 12
			motionGear.LeanMotionGear(-6 * inputManager.Brake, null);
		}
		else
		{
			motionGear.LeanMotionGear(0, null);
		}

		if (inputManager.Drift)
		{
			int leftRight = inputManager.Horizontal > 0 ? -1 : 1;
			driftValue = 5 * leftRight;
		}
		else
		{
			driftValue = 0;
		}

		vibrationRollValue = (vibrationRollValue > 0 ? -1 : 1) * Random.Range(0.05f, 0.2f);

		motionGear.LeanMotionGear(null, driftValue + vibrationRollValue);
	}
}
