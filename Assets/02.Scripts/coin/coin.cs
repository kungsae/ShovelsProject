using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class coin : MonoBehaviour
{
    Rigidbody2D rig;
	public float followSpeed;
	private bool canGet = false;
	public float testPower;

	public Action dropCoin;
	public Action getCoin;
	PlayerStat player;

	private void Awake()
	{
		rig = GetComponent<Rigidbody2D>();
		player = GameManager.instance.player.GetComponent<PlayerStat>();
		dropCoin += () =>
		{
			rig.AddForce(new Vector2(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(5, 10)) * testPower);
			Invoke("canGetCoin", 1f);
		};
	}

	private void OnDisable()
	{
		canGet = false;
	}

	void Update()
    {

		if (Physics2D.OverlapCircle(transform.position, 3, 1 << 7) != null && canGet)
		{
			transform.position = Vector2.MoveTowards(transform.position,GameManager.instance.player.transform.position,followSpeed*Time.deltaTime);
			if (Physics2D.OverlapCircle(transform.position, 0.2f, 1 << 7) != null)
			{
				player.money++;
				getCoin();
				gameObject.SetActive(false);
			}
		}

	}
	private void canGetCoin()
	{
		canGet = true;
	}
}
