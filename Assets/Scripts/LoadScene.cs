using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
	[SerializeField] private Image progressBar;

	private static string nextScene;

	private void Start()
	{
		LockVRCamera lockVRCamera = FindObjectOfType<LockVRCamera>();
		lockVRCamera.Lock();

		StartCoroutine(Loading());
	}

	public static void MoveTo(string sceneName)
	{
		nextScene = sceneName;
		SceneManager.LoadScene("Load");
	}

	private IEnumerator Loading()
	{
		yield return null;
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextScene);
		asyncOperation.allowSceneActivation = false;
		float timer = 0;
		while (!asyncOperation.isDone)
		{
			yield return null;
			timer += Time.deltaTime;
			if (asyncOperation.progress < 0.9f)
			{
				progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, asyncOperation.progress, timer);
				if (progressBar.fillAmount >= asyncOperation.progress)
				{
					timer = 0f;
				}
			}
			else
			{
				progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
				if (progressBar.fillAmount == 1f)
				{
					asyncOperation.allowSceneActivation = true;
					yield break;
				}
			}
		}
	}
}
