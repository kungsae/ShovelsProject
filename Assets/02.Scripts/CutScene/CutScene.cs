using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Playables;

public class CutScene : MonoBehaviour
{
    PlayableDirector director;
    public GameObject cameraTarget;




    // Start is called before the first frame update
    void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        director.Play();
        CameraManager.instance.followCamChange(CameraManager.instance.mainCam, 470,260);
        Destroy(GetComponent<BoxCollider2D>());
	}

}
