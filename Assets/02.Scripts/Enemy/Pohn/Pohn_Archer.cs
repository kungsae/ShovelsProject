using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pohn_Archer :  EnemyControl
{
	public GameObject arrow;

	public void fire()
	{
		Instantiate(arrow, transform.position, Quaternion.Euler(Vector2.right));
	}
}
