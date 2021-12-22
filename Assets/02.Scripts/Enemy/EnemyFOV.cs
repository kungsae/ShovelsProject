using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
	EnemyControl enemy;

	public LayerMask layer;

	public float viewRange = 10f; //�þ߰Ÿ�
	[Range(0, 360)]
	public float viewAngle = 40f;//�þ� ����

	private float attackRange2;
	public float attackRange = 2f;
	public float aggroRange = 5f;


	private void Awake()
	{
		enemy = GetComponent<EnemyControl>();
		attackRange2 = attackRange;
	}
	public Vector2 CirclePoint(float angle)
	{
		return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
	}


	//���� �ȿ� �÷��̾ ���Դ��� üũ
	public bool IsTracePlayer()
	{
		bool isTracePlayer = false;
		Collider2D col = Physics2D.OverlapCircle(transform.position, viewAngle, (1<<7)+(1<<8));
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
	//�÷��̾ ���� ���� �ȿ� ���Դ��� üũ
	//public bool IsAttackRangeInPlayer()
	//{
	//	bool isAttack = false;
	//	Vector2 dir = GameManager.instance.player.transform.position - transform.position;
	//	RaycastHit2D hit2D = Physics2D.Raycast(transform.position, dir.normalized, attackRange, (1 << 7) + (1 << 8));
	//	Vector3 right = enemy.facingRight ? transform.right : transform.right * -1;
	//	if (Vector2.Angle(right, dir) < 180 * 0.5f)
	//	{
	//		Debug.DrawRay(transform.position, dir.normalized * attackRange, Color.blue, 0.1f);
	//		if (hit2D.collider != null)
	//		{
	//			isAttack = (hit2D.collider.gameObject.CompareTag("Player"));
	//			Debug.Log(isAttack);
	//		}

	//	}

	//	return isAttack;
	//}
	private void Update()
	{
		//if(IsAttackRangeInPlayer())
		//if (IsTracePlayer())
		//{
		//	IsViewPlayer();

		//}
	}

	//�÷��̾�� �ڽ� ���̿� �ٸ� ��ü�� ������ üũ
	public bool IsViewPlayer()
	{
		bool isView = false;
		Vector2 dir = GameManager.instance.player.transform.position - transform.position;

		RaycastHit2D hit2D = Physics2D.Raycast(transform.position, dir.normalized, viewRange, layer);
		//Debug.DrawRay(transform.position, dir.normalized * viewRange, Color.red, 0.1f);
		if (hit2D.collider != null)
		{
			isView = (hit2D.collider.gameObject.CompareTag("Player"));
		}

		return isView;
	}

	//public bool IsAggroOutPlayer()
	//{
	//	bool aggro = true;
	//	Vector2 dir = GameManager.instance.player.transform.position - transform.position;

	//	RaycastHit2D hit2D = Physics2D.Raycast(transform.position, dir.normalized, aggroRange, 1 << 7);
	//	Debug.DrawRay(transform.position, dir.normalized * aggroRange, Color.cyan, 0.1f);
	//	if (hit2D.collider != null)
	//	{
	//		aggro = !(hit2D.collider.gameObject.CompareTag("Player"));
	//		Debug.Log(aggro);
	//	}

	//	return aggro;
	//}
}
