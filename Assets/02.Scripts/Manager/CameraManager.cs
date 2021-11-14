using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Linq;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance { get; private set; }

    public PixelPerfectCamera pixelCam;

	public CinemachineVirtualCamera nowCam;
	public CinemachineVirtualCamera mainCam = new CinemachineVirtualCamera();
    public List <CinemachineVirtualCamera> vCams = new List<CinemachineVirtualCamera>();
	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("다수의 카메라 매니저 스크립트가 실행중입니다.");
		}
		instance = this;
		nowCam = mainCam;
	}
	public void followCamChange(CinemachineVirtualCamera lookCam, int x = 320, int y = 180)
	{
		mainCam.Priority = 0;
		CinemachineVirtualCamera cam = null;
		for (int i = 0; i < vCams.Count; i++)
		{
			if (cam != null)
			{
				if (cam.Priority < vCams[i].Priority)
					cam = vCams[i];
			}
			else
			{
				cam = vCams[i];
			}	
		}
		nowCam = cam;
		pixelCam.refResolutionX = x;
		pixelCam.refResolutionY = y;
	}
	public void ShakeCam(float intensity, float shakeTime)
	{
		CinemachineBasicMultiChannelPerlin cam = nowCam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
		CameraShake.instance.ShakeCam(intensity, shakeTime, cam);
	}
}
