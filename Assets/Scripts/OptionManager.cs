using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Net.NetworkInformation;
using Unity.VisualScripting;

public class OptionManager : MonoBehaviour
{
    [Header("Cashing")]
    [SerializeField] GameObject optionsPC;
    [SerializeField] GameObject optionsVR;
    [SerializeField] private UISoundVolume uiSoundVolume;
    [SerializeField] private UIGraphicOption[] uiGraphicOption = new UIGraphicOption[4];

    private int currentButtonID;

    private SoundSetting soundSetting;

    private void Awake()
    {
        soundSetting = FindObjectOfType<SoundSetting>();
    }

    private void Start()
    {
        for (int count = 0; count < uiGraphicOption.Length; count++)
        {
            uiGraphicOption[count].onButton.onClick.AddListener(delegate { On(); });
            uiGraphicOption[count].onButton.gameObject.name = string.Format("On/{0}", count);
            uiGraphicOption[count].offButton.onClick.AddListener(delegate { Off(); });
            uiGraphicOption[count].offButton.gameObject.name = string.Format("Off/{0}", count);

            string optionName = uiGraphicOption[count].name.ToLower();

            string optionFog = nameof(OptionData.fog).ToLower();
            string optionAntiAliasing = nameof(OptionData.antiAliasing).ToLower();
            string optionMotionBlur = nameof(OptionData.motionBlur).ToLower();
            string optionBloom = nameof(OptionData.bloom).ToLower();
            //이제 돌고 끝나는거, 멀티, 진동, 핸들만남음
            
            if (optionName.Equals(optionFog))
            {
                if (OptionData.fog)
                {
                    OnTextEffect(count);
                }
                else
                {
                    OffTextEffect(count);
                }
            }
            if (optionName.Equals(optionAntiAliasing))
            {
                if (OptionData.antiAliasing)
                {
                    OnTextEffect(count);
                }
                else
                {
                    OffTextEffect(count);
                }
            }
            if (optionName.Equals(optionMotionBlur))
            {
                if (OptionData.motionBlur)
                {
                    OnTextEffect(count);
                }
                else
                {
                    OffTextEffect(count);
                }
            }
            if (optionName.Equals(optionBloom))
            {
                if (OptionData.bloom)
                {
                    OnTextEffect(count);
                }
                else
                {
                    OffTextEffect(count);
                }
            }
        }

        StartCoroutine(PrintVolumeText());
    }

    private IEnumerator PrintVolumeText()
    {
        float value;
        while(true)
        {
            if (!optionsPC.activeSelf || !optionsVR.activeSelf) yield return null;

            value = uiSoundVolume.volumeSlider.value * 100;
            
            uiSoundVolume.volumeText.text = string.Format("{0}", Mathf.RoundToInt(value));

            OptionData.volume = (int)value;

            soundSetting.SetVolume();
            
            yield return null;
        }
    }

    public int FindID()
    {
        string clickedButtonName = EventSystem.current.currentSelectedGameObject.name;
        
        string[] splitString = clickedButtonName.Split('/');

        if (splitString.Length < 2) return -1;

        if (!(splitString[0].Equals("On") || splitString[0].Equals("Off"))) return -1;
        
        if (int.Parse(splitString[1]) < 0 || int.Parse(splitString[1]) >= uiGraphicOption.Length) return -1;

        return int.Parse(splitString[1]);
    }

    public void On()
    {
        int number;

        if ((number = FindID()) == -1) return;

        OnTextEffect(number);
    }

    private void OnTextEffect(int value)
    {
        uiGraphicOption[value].onText.color = Color.yellow;
        uiGraphicOption[value].offText.color = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 200 / 255f);
    }

    public void Off()
    {
        int number;

        if ((number = FindID()) == -1) return;

        OffTextEffect(number);
    }

    private void OffTextEffect(int value)
    {
        uiGraphicOption[value].onText.color = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 200 / 255f);
        uiGraphicOption[value].offText.color = Color.yellow;
    }
}
