using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public bool isClear;

	private InputManager inputManager;

	private void Awake()
	{
		inputManager = FindObjectOfType<InputManager>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GoTitle();
		}
		if (inputManager.Cross)
		{
			GoTitle();
		}
	}

	public void GoTitle()
	{
		LoadScene.MoveTo("Title");
	}
}
