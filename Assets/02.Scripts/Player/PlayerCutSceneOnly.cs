using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCutSceneOnly : MonoBehaviour
{
	public Rigidbody2D rig;

	private void OnEnable()
	{
		rig.velocity = new Vector2(0, 0);
		rig.isKinematic = true;
	}
	private void OnDisable()
	{
		rig.velocity = new Vector2(0, 0);
		rig.isKinematic = false;
	}
}
