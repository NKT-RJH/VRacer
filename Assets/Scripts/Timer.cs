using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameManager gameManager;
    private float time;

    private void Update()
    {
        if (!gameManager.gameStart) return;

        time += Time.deltaTime;

        string minuite = (((int)time) / 60).ToString("00");
        string second = (time % 60).ToString("00.00");

        timerText.text = string.Format("{0}:{1}", minuite, second);
    }
}
