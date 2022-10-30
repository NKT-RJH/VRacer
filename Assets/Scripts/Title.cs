using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class UICanvas
{
    public Image fadeIn;
    public GameObject playScreen;
    public GameObject carScreen;
    public GameObject equipmentScreen;
}

[RequireComponent(typeof(AudioSource))]
public class Title : MonoBehaviour
{
	public AudioClip BGM;
	public AudioClip carStart;
	public AudioClip click;

	[SerializeField] private UICanvas[] uiCanvas = new UICanvas[2];

	private AudioSource audioSource;

	private bool isInput;

	private bool isPlayScreen;
	private bool isCarScreen;
	private bool isEquipmentScreen;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void BeforeStart()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 120;
	}

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		StartCoroutine(FaidOutAndBGM());
	}

	private IEnumerator FaidOutAndBGM()
	{
		audioSource.PlayOneShot(carStart);

		for (int count1 = 0; count1 < uiCanvas.Length; count1++)
		{
			for (float count = 1; count >= 0; count -= 2 / 255f)
			{
				uiCanvas[count1].fadeIn.color = new Color(uiCanvas[count1].fadeIn.color.r, uiCanvas[count1].fadeIn.color.g, uiCanvas[count1].fadeIn.color.b, count);
				yield return null;
			}
		}

		for (int count = 0; count < uiCanvas.Length; count++)
		{
			uiCanvas[count].fadeIn.gameObject.SetActive(false);
		}

		audioSource.clip = BGM;
		audioSource.Play();

		isInput = true;
	}
	private IEnumerator Delay()
	{
		isInput = false;
		yield return new WaitForSeconds(0.3f);
		isInput = true;
	}

	private void Update()
	{
		if (!isInput) return;

		if (!isPlayScreen)
		{
			if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Circle))
			{
				GamePlay();
				StartCoroutine(Delay());
			}
			if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Cross))
			{
				Exit();
			}

			return;
		}

		if (!isCarScreen)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				ExitGamePlay();
			}
			if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Cross))
			{
				ExitGamePlay();
				StartCoroutine(Delay());
			}
			if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Triangle))
			{
				SetMap(0);
				GameCarPlay();
				StartCoroutine(Delay());
			}
			else if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Circle))
			{
				//SetMap(1);
				//GameCarPlay();
				//StartCoroutine(Delay());
			}

			return;
		}

		if (isEquipmentScreen)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				ExitEquipmentPlay();
			}
			if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Cross))
			{
				ExitEquipmentPlay();
				StartCoroutine(Delay());
			}
			if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Triangle))
			{
				SetEquipment(0);
				MoveScene();
			}
			else if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Circle))
			{
				SetEquipment(1);
				MoveScene();
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				ExitGameCarPlay();
			}
			if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Cross))
			{
				ExitGameCarPlay();
				StartCoroutine(Delay());
			}
			if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Triangle))
			{
				SetCar(0);
				GameEquipmentPlay();
				StartCoroutine(Delay());
			}
			else if (LogitechInput.GetKeyPresssed(LogitechKeyCode.FirstIndex, LogitechKeyCode.Circle))
			{
				//SetCar(1);
				//GameEquipmentPlay();
				//StartCoroutine(Delay());
			}
		}
	}

	public void Exit()
	{
		Application.Quit();
	}

	public void GamePlay()
	{
		audioSource.PlayOneShot(click);
		for (int count = 0; count < uiCanvas.Length; count++)
		{
            uiCanvas[count].playScreen.SetActive(true);
            uiCanvas[count].fadeIn.color = new Color(uiCanvas[count].fadeIn.color.r, uiCanvas[count].fadeIn.color.g, uiCanvas[count].fadeIn.color.b, 200 / 255f);
            uiCanvas[count].fadeIn.gameObject.SetActive(true);
		}
		isPlayScreen = true;
	}

	public void ExitGamePlay()
	{
        audioSource.PlayOneShot(click);
		for (int count = 0; count < uiCanvas.Length; count++)
		{
            uiCanvas[count].playScreen.SetActive(false);
            uiCanvas[count].fadeIn.gameObject.SetActive(false);
            uiCanvas[count].fadeIn.color = new Color(uiCanvas[count].fadeIn.color.r, uiCanvas[count].fadeIn.color.g, uiCanvas[count].fadeIn.color.b, 1);
		}
		isPlayScreen = false;
    }

	public void GameCarPlay()
	{
        audioSource.PlayOneShot(click);
		for (int count = 0; count < uiCanvas.Length; count++)
		{
            uiCanvas[count].playScreen.SetActive(false);
            uiCanvas[count].carScreen.SetActive(true);
		}
		isCarScreen = true;
	}
	public void ExitGameCarPlay()
	{
        audioSource.PlayOneShot(click);
		for (int count = 0; count < uiCanvas.Length; count++)
		{
            uiCanvas[count].carScreen.SetActive(false);
            uiCanvas[count].fadeIn.gameObject.SetActive(false);
            uiCanvas[count].fadeIn.color = new Color(uiCanvas[count].fadeIn.color.r, uiCanvas[count].fadeIn.color.g, uiCanvas[count].fadeIn.color.b, 1);
		}
		isPlayScreen = false;
		isCarScreen = false;
    }

	public void GameEquipmentPlay()
	{
        audioSource.PlayOneShot(click);
		for (int count = 0; count < uiCanvas.Length; count++)
		{
            uiCanvas[count].carScreen.SetActive(false);
            uiCanvas[count].equipmentScreen.SetActive(true);
		}
        isEquipmentScreen = true;
    }
    public void ExitEquipmentPlay()
    {
        audioSource.PlayOneShot(click);
		for (int count = 0; count < uiCanvas.Length; count++)
		{
            uiCanvas[count].equipmentScreen.SetActive(false);
            uiCanvas[count].fadeIn.gameObject.SetActive(false);
            uiCanvas[count].fadeIn.color = new Color(uiCanvas[count].fadeIn.color.r, uiCanvas[count].fadeIn.color.g, uiCanvas[count].fadeIn.color.b, 1);
		}
        isPlayScreen = false;
        isCarScreen = false;
		isEquipmentScreen = false;
    }

	public void MoveScene()
	{
		switch (PlayData.map)
		{
			case 0:
				SceneManager.LoadScene("Lake");
				break;
			case 1:
				SceneManager.LoadScene("Mountain");
				break;
		}
	}

	public void SetMap(int index)
	{
		PlayData.map = index;
	}
	public void SetCar(int index)
	{
		PlayData.car = index;
	}
	public void SetEquipment(int index)
	{
		PlayData.equipment = index;
	}
}
