using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TutorialManager : MonoBehaviour
{
	public bool isStart;

	public TextMeshProUGUI tutorialText;
	public GameObject UIMask;
	public RectTransform mask;
	private InputManager inputManager;
	private int tutorial = 0;
	private List<string> tutorialsText = new List<string>();
	private List<string> goodText = new List<string>();
	private List<Vector3> maskPos = new List<Vector3>();

	private void Start()
	{
		TextSetting();
		MaskPosSetting();
		StartCoroutine(Tutorials());
	}

	private void TextSetting()
	{
		tutorialsText.Add("튜토리얼에 오신것을 환영합니다");
		tutorialsText.Add("당신은 튜토리얼에서 UI 설명과 게임 조작 방법을 배우실 겁니다");
		tutorialsText.Add("자 먼저 UI에 대해 설명해 드리도록 하겠습니다");
		tutorialsText.Add("상단을 보시면 자동차의 속도를 확인하실 수 있습니다");
		tutorialsText.Add("왼쪽 상단을 보시면 몇 lap째 돌고있는지 확인하실 수 있습니다");
		tutorialsText.Add("오른쪽 상단을 보시면 게임을 플레이하고 있는 시간을 확인하실 수 있습니다");
		tutorialsText.Add("왼쪽 하단을 보시면 미니맵을 통해 당신의 위치를 확인하실 수 있습니다");
		tutorialsText.Add("오른쪽 하단을 보시면 당신의 기어 상태를 확인하실 수 있습니다");
		tutorialsText.Add("다음은 게임 조작법에 대해 설명해 드리도록 하겠습니다");
		tutorialsText.Add("E 혹은 기어 기기를 조작하여 기어를 변경하실 수 있습니다");
		tutorialsText.Add("W 혹은 엑셀을 눌러 앞으로 전진하실 수 있습니다");
		tutorialsText.Add("A 혹은 핸들을 왼쪽으로 돌려 좌회전하실 수 있습니다");
		tutorialsText.Add("D 혹은 핸들을 오른쪽으로 돌려 우회전하실 수 있습니다");
		tutorialsText.Add("Shift 혹은 동그라미 키를 눌러서 드리프트를 하실 수 있습니다");
		tutorialsText.Add("마지막으로 Space 혹은 세모 키를 눌러서 스폰 포인트로 이동하실 수 있습니다");
		tutorialsText.Add("당신은 게임의 조작방법을 전부 배우셨습니다");
		tutorialsText.Add("즐거운 시간 되시길 바랍니다");

		goodText.Add("잘하셨습니다");
		goodText.Add("훌륭합니다");
		goodText.Add("정말 멋지네요");
	}

	private void MaskPosSetting()
	{
		maskPos.Add(new Vector3(0, 450, 0));
		maskPos.Add(new Vector3(-850, 470, 0));
		maskPos.Add(new Vector3(850, 470, 0));
		maskPos.Add(new Vector3(-810, -400, 0));
		maskPos.Add(new Vector3(810, -400, 0));
	}

	private IEnumerator Tutorials()
	{
		int next = 0;
		for (int i = 0; i < 3; i++)
		{
			tutorialText.text = tutorialsText[next];
			yield return new WaitForSeconds(5);
			next++;
		}
		UIMask.SetActive(true);
		for (int i = 0; i < maskPos.Count; i++)
		{
			tutorialText.text = tutorialsText[next];
			mask.localPosition = maskPos[i];
			yield return new WaitForSeconds(6);
			next++;
		}
		UIMask.SetActive(false);
		tutorialText.text = tutorialsText[next];
		yield return new WaitForSeconds(5);
		next++;
		isStart = true;

		int gear = inputManager.Gear;

		tutorialText.text = tutorialsText[next];
		while (true)
		{
			if (inputManager.Gear != gear)
			{
				next++;
				tutorialText.text = goodText[Random.Range(0, goodText.Count)];
				yield return new WaitForSeconds(2);
				tutorial++;
				break;
			}
			yield return new WaitForEndOfFrameUnit();
		}

		tutorialText.text = tutorialsText[next];
		while (true)
		{
			if (inputManager.Gas > 0)
			{
				next++;
				tutorialText.text = goodText[Random.Range(0, goodText.Count)];
				yield return new WaitForSeconds(2);
				tutorial++;
				break;
			}
			yield return new WaitForEndOfFrameUnit();
		}

		tutorialText.text = tutorialsText[next];
		while (true)
		{
			if (inputManager.Horizontal < -10)
			{
				next++;
				tutorialText.text = goodText[Random.Range(0, goodText.Count)];
				yield return new WaitForSeconds(2);
				tutorial++;
				break;
			}
			yield return new WaitForEndOfFrameUnit();
		}

		tutorialText.text = tutorialsText[next];
		while (true)
		{
			if (inputManager.Horizontal > 10)
			{
				next++;
				tutorialText.text = goodText[Random.Range(0, goodText.Count)];
				yield return new WaitForSeconds(2);
				tutorial++;
				break;
			}
			yield return new WaitForEndOfFrameUnit();
		}

		tutorialText.text = tutorialsText[next];
		while (true)
		{
			if (inputManager.Drift)
			{
				next++;
				tutorialText.text = goodText[Random.Range(0, goodText.Count)];
				yield return new WaitForSeconds(2);
				tutorial++;
				break;
			}
			yield return new WaitForEndOfFrameUnit();
		}

		tutorialText.text = tutorialsText[next];
		while (true)
		{
			if (inputManager.Respawn)
			{
				next++;
				tutorialText.text = goodText[Random.Range(0, goodText.Count)];
				yield return new WaitForSeconds(2);
				tutorial++;
				break;
			}
			yield return new WaitForEndOfFrameUnit();
		}

		for (int i = 0; i < 2; i++)
		{
			tutorialText.text = tutorialsText[next];
			yield return new WaitForSeconds(5);
			next++;
		}
		LoadScene.MoveTo("Title");
	}
}
