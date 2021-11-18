using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pohn_Archer :  EnemyControl
{
	public GameObject arrow;
	public float arrowSpeed;
	public AudioClip shootSound;

	public void fire()
	{
		SoundManager.instance.SFXPlay(shootSound, transform.position,0.8f);
		float dir = GameManager.instance.player.transform.position.x - transform.position.x > 0 ? 1 : -1;
		GameObject arrow = Instantiate(this.arrow, transform.position, Quaternion.identity);
	}
}
