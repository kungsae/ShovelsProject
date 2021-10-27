using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatBoss : EnemyControl
{
	public LayerMask layer;
	private bool wallCheck = false;
	private bool canDash = false;
	public float attackSpeed;

	private float dashAttackDelay = 6f;

	private int attackType = 1;

	protected override void Start()
	{
		base.Start();
		GetComponent<EnemyAi>().state = EnemyAi.State.TRACE;
		health.hitEvent += () => 
		{
			attackType = 3;
			Attack();
		};
		canDash = true;
		attackType = 2;
		Attack();
		//StartCoroutine(DashDelay());
	}
	protected override void Update()
	{
		base.Update();

		animator.SetBool("canDash", canDash);
		animator.SetBool("wallCheck", wallCheck);
	}

	public IEnumerator AttackDash()
	{
		float dir = transform.localScale.x > 0 ? 1 : -1;
		while (!wallCheck)
		{
			wallCheck = Physics2D.Raycast(transform.position, Vector2.right * dir, 2f, layer);
			Debug.DrawRay(transform.position, Vector2.right * dir * 2f, Color.red, 0.1f);
			if (isAttack&&!wallCheck)
				rigid.velocity = new Vector2(attackSpeed * dir, rigid.velocity.y);
			yield return null;
		}

	}

	public void DashAttackEndEvent()
	{
		AttackEnd();
		wallCheck = false;
		canDash = false;
		StartCoroutine(DashDelay());
	}
	IEnumerator DashDelay()
	{
		yield return new WaitForSeconds(dashAttackDelay);
		canDash = true;
		//canAttack = true;
		attackType = 2;
		Attack();
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player")&&attackType == 2)
		{
			//rigid.velocity = new Vector2(0, 0);
			//AttackEnd();
		}
	}
	public override void AttackEnd()
	{
		base.AttackEnd();
		attackType = 1;
	}
	public override void Attack()
	{
		animator.SetInteger("AttackType", attackType);
		base.Attack();
	}
}
