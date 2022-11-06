using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadSaveFile()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            OptionData.volume = PlayerPrefs.GetInt("Volume");
        }

        if (PlayerPrefs.HasKey("Fog"))
        {
            OptionData.fog = Convert.ToBoolean(PlayerPrefs.GetInt("Fog"));
        }

        if (PlayerPrefs.HasKey("AntiAliasing"))
        {
            OptionData.antiAliasing = Convert.ToBoolean(PlayerPrefs.GetInt("AntiAliasing"));
        }

        if (PlayerPrefs.HasKey("MotionBlur"))
        {
            OptionData.motionBlur = Convert.ToBoolean(PlayerPrefs.GetInt("MotionBlur"));
        }

        if (PlayerPrefs.HasKey("Bloom"))
        {
            OptionData.bloom = Convert.ToBoolean(PlayerPrefs.GetInt("Bloom"));
        }
    }

    public static void Save()
    {
        PlayerPrefs.SetInt("Volume", OptionData.volume);
        PlayerPrefs.SetInt("Fog", Convert.ToInt32(OptionData.fog));
        PlayerPrefs.SetInt("AntiAliasing", Convert.ToInt32(OptionData.antiAliasing));
        PlayerPrefs.SetInt("MotionBlur", Convert.ToInt32(OptionData.motionBlur));
        PlayerPrefs.SetInt("Bloom", Convert.ToInt32(OptionData.bloom));
    }
}
