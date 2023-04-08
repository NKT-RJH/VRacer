using MotionHouse;
using UnityEngine;

// 유니티의 모든 씬에서 사용할 수 있게 제가 만든 싱글톤 클래스를 상속했습니다.
public class MotionGear : Singleton<MotionGear>
{
	/// <summary>
	/// 모션기어 앞, 뒤 기울임 값
	/// </summary>
	private float roll;
	/// <summary>
	/// 모션기어 좌, 우 기울임 값
	/// </summary>
	private float pitch;
	/// <summary>
	/// 모션기어 위, 아래 이동 값
	/// </summary>
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

	/// <summary>
	/// Roll, Pitch에 범위 내의 값 할당
	/// </summary>
	/// <param name="pitch"></param>
	/// <param name="roll"></param>
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

	/// <summary>
	/// Roll, Pitch 값 초기화
	/// </summary>
	public void StopLeanMotionGear()
	{
		roll = 0;
		pitch = 0;
	}

	/// <summary>
	/// Vibration 값 할당 및 모션기어의 위, 아래 이동 방향 변경 활성화
	/// </summary>
	/// <param name="value"></param>
	public void Vibration(float value)
	{
		vibration = value;

		isVibration = true;
	}

	/// <summary>
	/// Vibration 초기화 및 모션기어의 위, 아래 이동 방향 변경 비활성화
	/// </summary>
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