using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

public class GraphicSetting : DontDestroyOnLoad<GraphicSetting>
{
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Volume volume = FindObjectOfType<Volume>();
		HDAdditionalCameraData[] hdAdditionalCameraDatas = FindObjectsOfType<HDAdditionalCameraData>();
		print(volume.gameObject.name);
		if (volume.profile.TryGet(out Fog fog))
		{
			fog.enabled.overrideState = true;
			fog.enabled.value = OptionData.fog;
		}

		if (volume.profile.TryGet(out MotionBlur motionBlur))
		{
			motionBlur.intensity.overrideState = true;
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
			bloom.intensity.overrideState = true;
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
