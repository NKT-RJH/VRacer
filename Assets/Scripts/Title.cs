using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Title : MonoBehaviour
{
	public Image fadeIn;

    public AudioClip BGM;
	public AudioClip carStart;
	public AudioClip click;

	public GameObject playScreen;
	public GameObject carScreen;
	public GameObject equipmentScreen;

    private AudioSource audioSource;

	private bool isStart;

	private bool isPlayScreen;
	private bool isCarScreen;
	private bool isEquipmentScreen;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void BeforeStart()
	{
		Application.targetFrameRate = 60;
		QualitySettings.vSyncCount = 0;
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

		for (float count = 1; count >= 0; count -= 1 / 255f)
		{
			fadeIn.color = new Color(fadeIn.color.r, fadeIn.color.g, fadeIn.color.b, count);
			yield return null;
		}
		fadeIn.gameObject.SetActive(false);

		audioSource.clip = BGM;
		audioSource.Play();

		isStart = true;
	}
	private IEnumerator Delay()
	{
		isStart = false;
		yield return new WaitForSeconds(0.3f);
		isStart = true;
	}

	private void Update()
	{
		if (!isStart) return;

		if (isPlayScreen)
		{
			if (isCarScreen)
			{
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
			else
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
			}
		}
		else
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
		}
	}

	public void Exit()
	{
		Application.Quit();
	}

	public void GamePlay()
	{
		audioSource.PlayOneShot(click);
		playScreen.SetActive(true);
		fadeIn.color = new Color(fadeIn.color.r, fadeIn.color.g, fadeIn.color.b, 200 / 255f);
		fadeIn.gameObject.SetActive(true);
		isPlayScreen = true;
	}

	public void ExitGamePlay()
	{
        audioSource.PlayOneShot(click);
        playScreen.SetActive(false);
		fadeIn.gameObject.SetActive(false);
		fadeIn.color = new Color(fadeIn.color.r, fadeIn.color.g, fadeIn.color.b, 1);
		isPlayScreen = false;
    }

	public void GameCarPlay()
	{
        audioSource.PlayOneShot(click);
        playScreen.SetActive(false);
		carScreen.SetActive(true);
		isCarScreen = true;
	}
	public void ExitGameCarPlay()
	{
        audioSource.PlayOneShot(click);
        carScreen.SetActive(false);
        fadeIn.gameObject.SetActive(false);
        fadeIn.color = new Color(fadeIn.color.r, fadeIn.color.g, fadeIn.color.b, 1);
		isPlayScreen = false;
		isCarScreen = false;
    }

	public void GameEquipmentPlay()
	{
        audioSource.PlayOneShot(click);
        carScreen.SetActive(false);
        equipmentScreen.SetActive(true);
        isEquipmentScreen = true;
    }
    public void ExitEquipmentPlay()
    {
        audioSource.PlayOneShot(click);
        equipmentScreen.SetActive(false);
        fadeIn.gameObject.SetActive(false);
        fadeIn.color = new Color(fadeIn.color.r, fadeIn.color.g, fadeIn.color.b, 1);
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
