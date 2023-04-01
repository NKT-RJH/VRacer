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
		inputManager = FindObjectOfType<InputManager>();

		tutorialsText.Add("Ʃ�丮�� ���Ű��� ȯ���մϴ�");
		tutorialsText.Add("����� Ʃ�丮�󿡼� UI ����� ���� ���� ����� ���� �̴ϴ�");
		tutorialsText.Add("�� ���� UI�� ���� ������ �帮���� �ϰڽ��ϴ�");
		tutorialsText.Add("����� ���ø� �ڵ����� �ӵ��� Ȯ���Ͻ� �� �ֽ��ϴ�");
		tutorialsText.Add("���� ����� ���ø� �� lap° �����ִ��� Ȯ���Ͻ� �� �ֽ��ϴ�");
		tutorialsText.Add("������ ����� ���ø� ������ �÷����ϰ� �ִ� �ð��� Ȯ���Ͻ� �� �ֽ��ϴ�");
		tutorialsText.Add("���� �ϴ��� ���ø� �̴ϸ��� ���� ����� ��ġ�� Ȯ���Ͻ� �� �ֽ��ϴ�");
		tutorialsText.Add("������ �ϴ��� ���ø� ����� ��� ���¸� Ȯ���Ͻ� �� �ֽ��ϴ�");
		tutorialsText.Add("������ ���� ���۹��� ���� ������ �帮���� �ϰڽ��ϴ�");
		tutorialsText.Add("E�� Q Ȥ�� ��� ��⸦ �����Ͽ� �� �����Ͻ� �� �ֽ��ϴ�");
		tutorialsText.Add("W Ȥ�� ������ ���� ������ �����Ͻ� �� �ֽ��ϴ�");
		tutorialsText.Add("S Ȥ�� �극��ũ�� ���� �����Ͻ� �� �ֽ��ϴ�");
		tutorialsText.Add("A Ȥ�� �ڵ��� �������� ���� ��ȸ���Ͻ� �� �ֽ��ϴ�");
		tutorialsText.Add("D Ȥ�� �ڵ��� ���������� ���� ��ȸ���Ͻ� �� �ֽ��ϴ�");
		tutorialsText.Add("���������� Space Ȥ�� ���� Ű�� ������ ���� ����Ʈ�� �̵��Ͻ� �� �ֽ��ϴ�");
		tutorialsText.Add("����� ������ ���۹���� ���� ���̽��ϴ�");
		tutorialsText.Add("��ſ� �ð� �ǽñ� �ٶ��ϴ�");

		goodText.Add("���ϼ̽��ϴ�");
		goodText.Add("�Ǹ��մϴ�");
		goodText.Add("���� �����׿�");
	}

	private void MaskPosSetting()
	{
		maskPos.Add(new Vector3(0, 450, 0));
		maskPos.Add(new Vector3(850, 470, 0));
		maskPos.Add(new Vector3(-850, 470, 0));
		maskPos.Add(new Vector3(810, -400, 0));
		maskPos.Add(new Vector3(-810, -400, 0));
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

		int gear = inputManager.gear;

		tutorialText.text = tutorialsText[next];
		while (true)
		{
			if (inputManager.gear != gear)
			{
				next++;
				tutorialText.text = goodText[Random.Range(0, goodText.Count)];
				yield return new WaitForSeconds(2);
				break;
			}
			yield return null;
		}

		tutorialText.text = tutorialsText[next];
		while (true)
		{
			if (inputManager.gas > 0)
			{
				next++;
				tutorialText.text = goodText[Random.Range(0, goodText.Count)];
				yield return new WaitForSeconds(2);
				break;
			}
			yield return null;
		}

		tutorialText.text = tutorialsText[next];
		while (true)
		{
			if (inputManager.brake > 0)
			{
				next++;
				tutorialText.text = goodText[Random.Range(0, goodText.Count)];
				yield return new WaitForSeconds(2);
				break;
			}
			yield return null;
		}

		tutorialText.text = tutorialsText[next];
		while (true)
		{
			if (inputManager.horizontal < 0)
			{
				next++;
				tutorialText.text = goodText[Random.Range(0, goodText.Count)];
				yield return new WaitForSeconds(2);
				break;
			}
			yield return null;
		}

		tutorialText.text = tutorialsText[next];
		while (true)
		{
			if (inputManager.horizontal > 0)
			{
				next++;
				tutorialText.text = goodText[Random.Range(0, goodText.Count)];
				yield return new WaitForSeconds(2);
				break;
			}
			yield return null;
		}

		tutorialText.text = tutorialsText[next];
		while (true)
		{
			if (inputManager.respawn)
			{
				next++;
				tutorialText.text = goodText[Random.Range(0, goodText.Count)];
				yield return new WaitForSeconds(2);
				break;
			}
			yield return null;
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
