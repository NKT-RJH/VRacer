using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Title : MonoBehaviour
{
	public AudioClip BGM;
	public AudioClip carStart;
	public AudioClip click;

	[SerializeField] private UITitle[] titleUI = new UITitle[2];

	private AudioSource audioSource;
	private InputManager inputManager;

	private bool isInput;

	private bool isOptionScreen;
	private bool isMapScreen;
	private bool isModeScreen;
	private bool isCarScreen;
	private bool isEquipmentScreen;

	private void Awake()
	{
		MotionGear motionGear = FindObjectOfType<MotionGear>();

		motionGear.StopLeanMotionGear();
		motionGear.StopVibration();

		audioSource = GetComponent<AudioSource>();
		inputManager = FindObjectOfType<InputManager>();
	}

	private void Start()
	{
		StartCoroutine(FaidOutAndBGM());
	}

	private IEnumerator FaidOutAndBGM()
	{
		audioSource.PlayOneShot(carStart);

		for (int count1 = 0; count1 < titleUI.Length; count1++)
		{
			for (float count = 1; count >= 0; count -= 15 / 255f)
			{
				titleUI[count1].fadeIn.color = new Color(titleUI[count1].fadeIn.color.r, titleUI[count1].fadeIn.color.g, titleUI[count1].fadeIn.color.b, count);
				yield return null;
			}
		}

		for (int count = 0; count < titleUI.Length; count++)
		{
			titleUI[count].fadeIn.gameObject.SetActive(false);
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

		if (isOptionScreen)
		{
			if (inputManager.crossButton)
			{
				ExitOptions();
				StartCoroutine(Delay());
			}

			return;
		}

		if (!isEquipmentScreen)
		{
			if (inputManager.circleButton)
			{
				GameEquipmentPlay();
			}
			else if (inputManager.triangleButton)
			{
				Options();
			}
			else if (inputManager.crossButton)
			{
				Exit();
			}

			StartCoroutine(Delay());

			return;
		}

		if (!isModeScreen)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				ExitEquipmentPlay();
			}

			if (inputManager.crossButton)
			{
				ExitEquipmentPlay();
			}
			else if (inputManager.triangleButton)
			{
				SetEquipment(0);
				GameModePlay();
			}
			else if (inputManager.circleButton)
			{
				SetEquipment(1);
				GameModePlay();
			}

			StartCoroutine(Delay());

			return;
		}

		if (!isCarScreen)
		{
			// 모드 선택
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				ExitGameModePlay();
			}
			if (inputManager.crossButton)
			{
				ExitGameModePlay();
			}
			if (inputManager.triangleButton)
			{
				SetMode(0);
				GameCarPlay();
			}
			else if (inputManager.circleButton)
			{
				SetMode(1);
				GameCarPlay();
			}
			else if (inputManager.squareButton)
			{
				SetMode(-1);
				MoveScene();
			}
			StartCoroutine(Delay());

			return;
		}

		if (!isMapScreen)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				ExitGameCarPlay();
			}

			if (inputManager.crossButton)
			{
				ExitGameCarPlay();
			}
			else if (inputManager.triangleButton)
			{
				SetCar(0);
				GameMap();
			}
			else if (inputManager.circleButton)
			{
				SetCar(1);
				GameMap();
			}
			StartCoroutine(Delay());
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				ExitGameMap();
			}

			if (inputManager.crossButton)
			{
				ExitGameMap();
			}
			else if (inputManager.triangleButton)
			{
				SetMap(0);
				MoveScene();
			}
			else if (inputManager.circleButton)
			{
				SetMap(1);
				MoveScene();
			}
			StartCoroutine(Delay());
		}
	}

	public void Exit()
	{
		Application.Quit();
	}

	public void Options()
	{
		audioSource.PlayOneShot(click);
		for (int count = 0; count < titleUI.Length; count++)
		{
			titleUI[count].optionScreen.SetActive(true);
			titleUI[count].fadeIn.color = new Color(titleUI[count].fadeIn.color.r, titleUI[count].fadeIn.color.g, titleUI[count].fadeIn.color.b, 200 / 255f);
			titleUI[count].fadeIn.gameObject.SetActive(true);
		}
		isOptionScreen = true;
	}

	public void ExitOptions()
	{
		audioSource.PlayOneShot(click);
		for (int count = 0; count < titleUI.Length; count++)
		{
			titleUI[count].optionScreen.SetActive(false);
			titleUI[count].fadeIn.gameObject.SetActive(false);
			titleUI[count].fadeIn.color = new Color(titleUI[count].fadeIn.color.r, titleUI[count].fadeIn.color.g, titleUI[count].fadeIn.color.b, 1);
		}
		isOptionScreen = false;
	}

	public void GameMap()
	{
		audioSource.PlayOneShot(click);
		for (int count = 0; count < titleUI.Length; count++)
		{
			titleUI[count].carScreen.SetActive(false);
			titleUI[count].fadeIn.color = new Color(titleUI[count].fadeIn.color.r, titleUI[count].fadeIn.color.g, titleUI[count].fadeIn.color.b, 200 / 255f);
			titleUI[count].playScreen.SetActive(true);
		}
		isMapScreen = true;
	}

	public void ExitGameMap()
	{
		audioSource.PlayOneShot(click);
		for (int count = 0; count < titleUI.Length; count++)
		{
			titleUI[count].playScreen.SetActive(false);
			titleUI[count].fadeIn.gameObject.SetActive(false);
			titleUI[count].fadeIn.color = new Color(titleUI[count].fadeIn.color.r, titleUI[count].fadeIn.color.g, titleUI[count].fadeIn.color.b, 1);
		}
		isEquipmentScreen = false;
		isCarScreen = false;
		isModeScreen = false;
		isMapScreen = false;
	}

	public void GameCarPlay()
	{
		audioSource.PlayOneShot(click);
		for (int count = 0; count < titleUI.Length; count++)
		{
			titleUI[count].modeScreen.SetActive(false);
			titleUI[count].fadeIn.color = new Color(titleUI[count].fadeIn.color.r, titleUI[count].fadeIn.color.g, titleUI[count].fadeIn.color.b, 200 / 255f);
			titleUI[count].carScreen.SetActive(true);
		}
		isCarScreen = true;
	}
	public void ExitGameCarPlay()
	{
		audioSource.PlayOneShot(click);
		for (int count = 0; count < titleUI.Length; count++)
		{
			titleUI[count].carScreen.SetActive(false);
			titleUI[count].fadeIn.gameObject.SetActive(false);
			titleUI[count].fadeIn.color = new Color(titleUI[count].fadeIn.color.r, titleUI[count].fadeIn.color.g, titleUI[count].fadeIn.color.b, 1);
		}
		isEquipmentScreen = false;
		isModeScreen = false;
		isCarScreen = false;
	}

	public void GameEquipmentPlay()
	{
		audioSource.PlayOneShot(click);
		for (int count = 0; count < titleUI.Length; count++)
		{
			titleUI[count].equipmentScreen.SetActive(true);
			titleUI[count].fadeIn.color = new Color(titleUI[count].fadeIn.color.r, titleUI[count].fadeIn.color.g, titleUI[count].fadeIn.color.b, 200 / 255f);
			titleUI[count].fadeIn.gameObject.SetActive(true);
		}
		isEquipmentScreen = true;
	}
	public void ExitEquipmentPlay()
	{
		audioSource.PlayOneShot(click);
		for (int count = 0; count < titleUI.Length; count++)
		{
			titleUI[count].equipmentScreen.SetActive(false);
			titleUI[count].fadeIn.gameObject.SetActive(false);
			titleUI[count].fadeIn.color = new Color(titleUI[count].fadeIn.color.r, titleUI[count].fadeIn.color.g, titleUI[count].fadeIn.color.b, 1);
		}
		isEquipmentScreen = false;
	}

	public void GameModePlay()
	{
		audioSource.PlayOneShot(click);
		for (int count = 0; count < titleUI.Length; count++)
		{
			titleUI[count].equipmentScreen.SetActive(false);
			titleUI[count].fadeIn.color = new Color(titleUI[count].fadeIn.color.r, titleUI[count].fadeIn.color.g, titleUI[count].fadeIn.color.b, 200 / 255f);
			titleUI[count].modeScreen.SetActive(true);
		}
		isModeScreen = true;
	}

	public void ExitGameModePlay()
	{
		audioSource.PlayOneShot(click);
		for (int count = 0; count < titleUI.Length; count++)
		{
			titleUI[count].modeScreen.SetActive(false);
			titleUI[count].fadeIn.gameObject.SetActive(false);
			titleUI[count].fadeIn.color = new Color(titleUI[count].fadeIn.color.r, titleUI[count].fadeIn.color.g, titleUI[count].fadeIn.color.b, 1);
		}
		isEquipmentScreen = false;
		isModeScreen = false;
	}

	public void MoveScene()
	{
		if (PlayData.mode == -1)
		{
			LoadScene.MoveTo("Tutorial");
			return;
		}
		if (PlayData.mode == 1)
		{
			LoadScene.MoveTo("MultyRoom");
			return;
		}

		switch (PlayData.map)
		{
			case 0:
				LoadScene.MoveTo("Lake");
				break;
			case 1:
				LoadScene.MoveTo("Mountain");
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

	public void SetMode(int index)
	{
		PlayData.mode = index;
	}
}
