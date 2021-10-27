using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TestCutScene : MonoBehaviour
{
    PlayableDirector director;
    // Start is called before the first frame update
    void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        director.Play();
        Destroy(this);
	}
}
