using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStart : MonoBehaviour
{
    public TextMeshProUGUI countDownText;
    public bool startGame;

    private void Start()
    {
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1f);
        countDownText.gameObject.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countDownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        countDownText.text = "Go!";
        startGame = true;
        yield return new WaitForSeconds(1f);
        countDownText.gameObject.SetActive(false);
    }
}
