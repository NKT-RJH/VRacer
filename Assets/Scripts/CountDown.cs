using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
	public GameManager gameManager;
    public TextMeshProUGUI countDownText;

    private void Start()
    {
        StartCoroutine(CountDownStart());
    }

    private IEnumerator CountDownStart()
    {
        yield return new WaitForSeconds(1);

        countDownText.gameObject.SetActive(true);

        for (int count = 3; count > 0; count--)
        {
            countDownText.text = count.ToString();
            yield return new WaitForSeconds(1);
        }

        countDownText.text = "Go!";
        gameManager.gameStart = true;

        yield return new WaitForSeconds(1.2f);

        countDownText.gameObject.SetActive(false);
    }
}
