using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraShake : MonoBehaviour
{
	public static CameraShake instance { get; private set; }
	public PixelPerfectCamera pixelCam;
	private float curretTime;
	private bool isShake = false;

    private CinemachineVirtualCamera virCam;
    private CinemachineBasicMultiChannelPerlin camNoise;
	// Start is called before the first frame update
	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("다수의 카메라 쉐이크 스크립트가 실행중입니다.");
		}
		instance = this;
		virCam = GetComponent<CinemachineVirtualCamera>();
		camNoise = virCam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
	}
	private IEnumerator ShakeUpdate(float intensity, float time)
	{
		if (isShake)
		{
			yield break;
		}
		isShake = true;
		camNoise.m_AmplitudeGain = intensity;
		curretTime = 0;


		while (true)
		{
			curretTime += Time.deltaTime;
			if (curretTime >= time)
			{
				break;
			}
			camNoise.m_AmplitudeGain = Mathf.Lerp(intensity, 0, curretTime / time);
			yield return null;
		}
		isShake = false;
		camNoise.m_AmplitudeGain = 0;
	}
	public void ShakeCam(float intensity,float time)
	{
		StartCoroutine(ShakeUpdate(intensity,time));
	}
	public void followCamChange(GameObject followObj,int x = 320,int y = 180)
	{
		virCam.Follow = followObj.transform;
		pixelCam.refResolutionX = x;
		pixelCam.refResolutionY = y;
	}
}
