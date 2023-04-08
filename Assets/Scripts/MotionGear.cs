using MotionHouse;
using UnityEngine;

// ����Ƽ�� ��� ������ ����� �� �ְ� ���� ���� �̱��� Ŭ������ ����߽��ϴ�.
public class MotionGear : Singleton<MotionGear>
{
	/// <summary>
	/// ��Ǳ�� ��, �� ����� ��
	/// </summary>
	private float roll;
	/// <summary>
	/// ��Ǳ�� ��, �� ����� ��
	/// </summary>
	private float pitch;
	/// <summary>
	/// ��Ǳ�� ��, �Ʒ� �̵� ��
	/// </summary>
	private float vibration;

	private bool isEnabled;
	private bool isVibration;

	protected override void Awake()
	{
		base.Awake();

		// ��Ǳ�� ���� �Լ� �� ���� Ȱ��ȭ
		if (isEnabled) return;

		MotionHouseSDK.MHRun();
		MotionHouseSDK.MotionControlStart();

		isEnabled = true;
	}

	private void Update()
	{
		// ��Ǳ���� ��, �Ʒ� �̵� ���� ����
		if (isVibration)
		{
			vibration *= -1;
		}

		// ���� ���������� ��Ǳ� �����̴� �Լ��� ����
		// �� ������ ���� 0�̶�� �������� ����
		if (roll != 0 || pitch != 0 || vibration != 0)
		{
			MotionHouseSDK.MotionTelemetry(roll, pitch, 0, 0, vibration, 0, 0);
		}
	}

	/// <summary>
	/// Roll, Pitch�� ���� ���� �� �Ҵ�
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
	/// Roll, Pitch �� �ʱ�ȭ
	/// </summary>
	public void StopLeanMotionGear()
	{
		roll = 0;
		pitch = 0;
	}

	/// <summary>
	/// Vibration �� �Ҵ� �� ��Ǳ���� ��, �Ʒ� �̵� ���� ���� Ȱ��ȭ
	/// </summary>
	/// <param name="value"></param>
	public void Vibration(float value)
	{
		vibration = value;

		isVibration = true;
	}

	/// <summary>
	/// Vibration �ʱ�ȭ �� ��Ǳ���� ��, �Ʒ� �̵� ���� ���� ��Ȱ��ȭ
	/// </summary>
	public void StopVibration()
	{
		vibration = Mathf.Abs(vibration);

		isVibration = false;
	}

	// ���α׷� ���� �� ��Ǳ�� ����
	private void OnApplicationQuit()
	{
		MotionHouseSDK.MotionControlEnd();
		MotionHouseSDK.MHStop();
	}
}