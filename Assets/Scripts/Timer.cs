using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CountDown countDown;
    private float time;

    private void Update()
    {
        if (!countDown.CountDownEnd) return;

        time += Time.deltaTime;

        string minuite = (((int)time) / 60).ToString("00");
        string second = (time % 60).ToString("00.00");

        text.text = string.Format("{0}:{1}", minuite, second);
    }
}
