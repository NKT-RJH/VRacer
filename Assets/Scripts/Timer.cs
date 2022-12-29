using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CountDown countDown;
	[SerializeField] private ClearCheck clearCheck;
	private float time;

	private void Update()
    {
		if (clearCheck.IsClear) return;
        if (!countDown.CountDownEnd) return;

        time += Time.deltaTime;

        string minuite = (((int)time) / 60).ToString("00");
        string second = (time % 60).ToString("00.00");

        text.text = string.Format("{0}:{1}", minuite, second);
    }
}
