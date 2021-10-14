using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
	EnemyMove enemy;

	public float viewRange = 10f; //시야거리
	[Range(0, 360)]
	public float viewAngle = 40f;//시야 각도

	private void Awake()
	{
		enemy = GetComponent<EnemyMove>();
	}
	public Vector2 CirclePoint(float angle)
	{
		return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
	}
	public bool IsTracePlayer()
	{
		bool isTracePlayer = false;
		Collider2D col = Physics2D.OverlapCircle(transform.position, viewAngle, 1<<7);
		if (col != null)
		{
			Vector2 dir = GameManager.instance.player.transform.position - transform.position;
			Vector3 right = enemy.facingRight ? transform.right : transform.right * -1;

			if (Vector2.Angle(right, dir) < viewAngle * 0.5f)
			{
				isTracePlayer = true;
			}
		}
		return isTracePlayer;
	}
	private void Update()
	{
		if (IsTracePlayer())
		{
			IsViewPlayer();
		}
	}

	public bool IsViewPlayer()
	{
		bool isView = false;
		Vector2 dir = GameManager.instance.player.transform.position - transform.position;

		RaycastHit2D hit2D = Physics2D.Raycast(transform.position, dir.normalized, viewRange, 1<<8);
		Debug.DrawRay(transform.position, dir.normalized * viewRange, Color.red, 0.1f);
		if (hit2D.collider != null)
		{
			isView = (hit2D.collider.gameObject.CompareTag("Player"));
			Debug.Log(isView);
		}

		return isView;
	}
}
