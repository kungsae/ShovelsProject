using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioClip[] BGM;
    public AudioSource audio;
	// Start is called before the first frame update
	private void Awake()
	{
        if (instance != null)
        {
            Debug.Log("사운드 매니저 중복");
            Destroy(this);
        }
        instance = this;
    }
	void Start()
    {
        audio.clip = BGM[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NullBgm()
    {
        audio.Stop();
        audio.clip = null;
    }
    public void ChageBgm(int num)
    {
        audio.clip = BGM[num];
        audio.loop = true;
        audio.Play();
    }
    public void SFXPlay(AudioClip clip,Vector3 pos, float volume = 1f)
    {
        if (clip != null)
        {
            GameObject sound = new GameObject("sound");
            sound.transform.position = pos;
            AudioSource audio = sound.AddComponent<AudioSource>();
            audio.clip = clip;
            audio.volume = volume;
            audio.Play();
            Destroy(sound, clip.length);

        }
    }
}
