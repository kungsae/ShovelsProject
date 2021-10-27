using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatBoss : EnemyControl
{
	public LayerMask layer;
	public BoxCollider2D colliderD;
	private bool wallCheck = false;
	public float attackSpeed;
	public int damagedtime;

	private float dashAttackDelay = 3f;

	private int attackType;

	protected override void Start()
	{
		base.Start();
		GetComponent<EnemyAi>().state = EnemyAi.State.TRACE;
		health.hitEvent += () => 
		{
			attackType = 3;
			canAttack = true;
			Attack();
		};
	}
	protected override void Update()
	{
		base.Update();

		animator.SetBool("wallCheck", wallCheck);
	}

	public IEnumerator AttackDash()
	{
		float dir = transform.localScale.x > 0 ? 1 : -1;
		while (!wallCheck)
		{
			wallCheck = Physics2D.Raycast(transform.position, Vector2.right * dir, 2f, layer);
			Debug.DrawRay(transform.position, Vector2.right * dir * 2f, Color.red, 0.1f);
			if (isAttack && !wallCheck)
				rigid.velocity = new Vector2(attackSpeed * dir, rigid.velocity.y);
			yield return null;
		}

	}

	public void DashAttackEndEvent()
	{
		AttackEnd();
		wallCheck = false;
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			
		}
	}
	public override void AttackEnd()
	{
		base.AttackEnd();
		attackType = 1;
	}
	public override void Attack()
	{
		base.Attack();
	}
}
