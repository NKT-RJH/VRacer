using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadScene : MonoBehaviour
{
	[SerializeField] private Image[] progressBars;
	
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
		AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
		op.allowSceneActivation = false;
		float timer = 0.0f;
		while (!op.isDone)
		{
			yield return null;
			timer += Time.deltaTime;
			if (op.progress < 0.9f)
			{
				for (int count = 0; count < progressBars.Length; count++)
				{
					progressBars[0].fillAmount = Mathf.Lerp(progressBars[0].fillAmount, op.progress, timer);
				}
				if (progressBars[0].fillAmount >= op.progress)
				{
					timer = 0f;
				}
			}
			else
			{
				for (int count = 0; count < progressBars.Length; count++)
				{
					progressBars[0].fillAmount = Mathf.Lerp(progressBars[0].fillAmount, 1f, timer);
				}
				if (progressBars[0].fillAmount == 1f)
				{
					op.allowSceneActivation = true;
					yield break;
				}
			}
		}
	}
}
