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
            fog.active = OptionData.fog;
        }

        if (volume.profile.TryGet(out MotionBlur motionBlur))
        {
            motionBlur.active = OptionData.motionBlur;
        }

        if (volume.profile.TryGet(out Bloom bloom))
        {
            bloom.active = OptionData.bloom;
        }

        for (int count = 0; count < hdAdditionalCameraDatas.Length; count++)
        {
            HDAdditionalCameraData.AntialiasingMode antialiasingMode = OptionData.antiAliasing ? HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing : HDAdditionalCameraData.AntialiasingMode.None;
            hdAdditionalCameraDatas[count].antialiasing = antialiasingMode;
        }
    }
}
