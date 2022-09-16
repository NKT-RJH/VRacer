using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameStart gameStart;
    private void Start()
    {
        gameStart = FindObjectOfType<GameStart>();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("InGame");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
