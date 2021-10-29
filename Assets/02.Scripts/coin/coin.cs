using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    Rigidbody2D rig;
	public float testPower;
	

	private void Awake()
	{
		rig = GetComponent<Rigidbody2D>();
	}
	private void Start()
	{
		rig.AddForce(new Vector2(Random.Range(-10, 10), Random.Range(0, 10))* testPower);
	}

	// Update is called once per frame
	void Update()
    {
		if (Physics2D.OverlapCircle(transform.position, 2, 1 << 7))
		{
			Vector3 dir = GameManager.instance.player.transform.position - transform.position;
			transform.position += dir * Time.deltaTime;
		}
    }
}
