using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pohn_Archer :  EnemyControl
{
	public GameObject arrow;
	public float arrowSpeed;

	public void fire()
	{
		float dir = GameManager.instance.player.transform.position.x - transform.position.x > 0 ? 1 : -1;
		GameObject arrow = Instantiate(this.arrow, transform.position, Quaternion.identity);
		Rigidbody2D rig = arrow.GetComponent<Rigidbody2D>();
		rig.velocity = new Vector2(arrowSpeed*dir, 0);
	}
}
