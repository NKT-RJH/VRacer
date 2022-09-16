using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public float time;
    public int minuite;
    public float second;
    private GameStart gameStart;

    private void Awake()
    {
        gameStart = GetComponent<GameStart>();
    }

    private void Update()
    {
        if (!gameStart.startGame) return;

        time += Time.deltaTime;

        string minuite = (((int)time) / 60).ToString("00");
        string second = (time % 60).ToString("00.00");

        timeText.text = string.Format("{0}:{1}", minuite, second);
    }
}
