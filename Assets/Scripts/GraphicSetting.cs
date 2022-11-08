using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class GraphicSetting : DontDestroyOnLoad<GraphicSetting>
{
    protected override void Awake()
    {
        base.Awake();

        Volume volume = FindObjectOfType<Volume>();
        HDAdditionalCameraData[] hdAdditionalCameraDatas = FindObjectsOfType<HDAdditionalCameraData>();

        if (volume.profile.TryGet(out Fog fog))
        {
            fog.enabled.value = OptionData.fog;
        }

        if (volume.profile.TryGet(out MotionBlur motionBlur))
        {
            if (OptionData.motionBlur)
            {
                motionBlur.intensity.value = 1.5f;
            }
            else
            {
                motionBlur.intensity.value = 0;
            }
        }

        if (volume.profile.TryGet(out Bloom bloom))
        {
            if (OptionData.motionBlur)
            {
                bloom.intensity.value = 0.3f;
            }
            else
            {
                bloom.intensity.value = 0;
            }
        }

        for (int count = 0; count < hdAdditionalCameraDatas.Length; count++)
        {
            HDAdditionalCameraData.AntialiasingMode antialiasingMode = OptionData.antiAliasing ? HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing : HDAdditionalCameraData.AntialiasingMode.None;
            hdAdditionalCameraDatas[count].antialiasing = antialiasingMode;
        }
    }
}
