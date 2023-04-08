using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

// ����Ƽ�� ��� ������ Ȱ���� �� �ֵ��� �̱��� Ŭ������ ����߽��ϴ�.
public class GraphicSetting : Singleton<GraphicSetting>
{
	// ���� �ε����ڸ��� ����
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// HDRP �׷��� ������ ����ϴ� Volume�� Camera ���� ���������� �Ҵ�
		Volume volume = FindObjectOfType<Volume>();
		HDAdditionalCameraData[] hdAdditionalCameraDatas = FindObjectsOfType<HDAdditionalCameraData>();
		
		// �Ȱ�(Fog) ����
		// VolumeProfile���� Fog Ŭ���� ��������
		if (volume.profile.TryGet(out Fog fog))
		{
			// ���� ����� Ȱ��ȭ
			fog.enabled.overrideState = true;
			// ���̺����Ͽ��� Bool ���·� �ҷ��� �Ȱ� Ȱ�� ���� �Է�
			fog.enabled.value = OptionData.fog;
		}

		// ��� ��(Motion Blur) ����
		// VolumeProfile���� MotionBlur Ŭ���� ��������
		if (volume.profile.TryGet(out MotionBlur motionBlur))
		{
			motionBlur.intensity.overrideState = true;
			// ���̺����� Bool ���� ���� ��� �� ��ġ ����
			if (OptionData.motionBlur)
			{
				motionBlur.intensity.value = 1.5f;
			}
			else
			{
				motionBlur.intensity.value = 0;
			}
		}

		// ���(Bloom) ����
		// VolumeProfile���� Bloom Ŭ���� ��������
		if (volume.profile.TryGet(out Bloom bloom))
		{
			bloom.intensity.overrideState = true;
			// ���̺����� Bool ���� ���� ��� ��ġ ����
			if (OptionData.bloom)
			{
				bloom.intensity.value = 0.3f;
			}
			else
			{
				bloom.intensity.value = 0;
			}
		}

		// ��Ƽ�ٸ���� ����
		for (int count = 0; count < hdAdditionalCameraDatas.Length; count++)
		{
			// �� �� ��� ī�޶� ���̺����� Bool ���� ���� Ȱ��ȭ ���� �Է�
			HDAdditionalCameraData.AntialiasingMode antialiasingMode = OptionData.antiAliasing ? HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing : HDAdditionalCameraData.AntialiasingMode.None;
			hdAdditionalCameraDatas[count].antialiasing = antialiasingMode;
		}
	}
}