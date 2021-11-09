using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        CameraShake.instance.followCamChange(cameraTarget,470,260);
        Destroy(this);
	}
}
