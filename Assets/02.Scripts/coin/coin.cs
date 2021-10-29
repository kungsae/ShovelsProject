using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    Rigidbody2D rig;
	public float followSpeed;
	private bool canGet = false;
	public float testPower;
	

	private void Awake()
	{
		rig = GetComponent<Rigidbody2D>();
	}
	private void Start()
	{
		rig.AddForce(new Vector2(Random.Range(-10, 10), Random.Range(0, 10))* testPower);
		Invoke("canGetCoin", 1f);
	}

	// Update is called once per frame
	void Update()
    {

		if (Physics2D.OverlapCircle(transform.position, 3, 1 << 7) != null && canGet)
		{
			transform.position = Vector2.MoveTowards(transform.position,GameManager.instance.player.transform.position,followSpeed*Time.deltaTime);
			if (Physics2D.OverlapCircle(transform.position, 0.2f, 1 << 7) != null)
			{
				GameManager.instance.player.GetComponent<PlayerStat>().money++;
				Destroy(gameObject);
			}
		}

	}
	private void canGetCoin()
	{
		canGet = true;
	}
}
