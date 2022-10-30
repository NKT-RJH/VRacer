using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public bool gameStart;

	public bool isClear;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SceneManager.LoadScene("Title");
		}
		if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Cross))
		{
			SceneManager.LoadScene("Title");
		}
	}
}
