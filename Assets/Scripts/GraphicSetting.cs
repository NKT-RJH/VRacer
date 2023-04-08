using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

// 유니티의 모든 씬에서 활용할 수 있도록 싱글톤 클래스를 상속했습니다.
public class GraphicSetting : Singleton<GraphicSetting>
{
	// 씬이 로딩되자마자 실행
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// HDRP 그래픽 설정을 담당하는 Volume과 Camera 정보 지역변수에 할당
		Volume volume = FindObjectOfType<Volume>();
		HDAdditionalCameraData[] hdAdditionalCameraDatas = FindObjectsOfType<HDAdditionalCameraData>();
		
		// 안개(Fog) 설정
		// VolumeProfile에서 Fog 클래스 가져오기
		if (volume.profile.TryGet(out Fog fog))
		{
			// 설정 덮어쓰기 활성화
			fog.enabled.overrideState = true;
			// 세이브파일에서 Bool 형태로 불러온 안개 활성 여부 입력
			fog.enabled.value = OptionData.fog;
		}

		// 모션 블러(Motion Blur) 설정
		// VolumeProfile에서 MotionBlur 클래스 가져오기
		if (volume.profile.TryGet(out MotionBlur motionBlur))
		{
			motionBlur.intensity.overrideState = true;
			// 세이브파일 Bool 값에 따라 모션 블러 수치 조정
			if (OptionData.motionBlur)
			{
				motionBlur.intensity.value = 1.5f;
			}
			else
			{
				motionBlur.intensity.value = 0;
			}
		}

		// 블룸(Bloom) 설정
		// VolumeProfile에서 Bloom 클래스 가져오기
		if (volume.profile.TryGet(out Bloom bloom))
		{
			bloom.intensity.overrideState = true;
			// 세이브파일 Bool 값에 따라 블룸 수치 조정
			if (OptionData.bloom)
			{
				bloom.intensity.value = 0.3f;
			}
			else
			{
				bloom.intensity.value = 0;
			}
		}

		// 안티앨리어싱 설정
		for (int count = 0; count < hdAdditionalCameraDatas.Length; count++)
		{
			// 씬 내 모든 카메라에 세이브파일 Bool 값에 따라 활성화 여부 입력
			HDAdditionalCameraData.AntialiasingMode antialiasingMode = OptionData.antiAliasing ? HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing : HDAdditionalCameraData.AntialiasingMode.None;
			hdAdditionalCameraDatas[count].antialiasing = antialiasingMode;
		}
	}
}