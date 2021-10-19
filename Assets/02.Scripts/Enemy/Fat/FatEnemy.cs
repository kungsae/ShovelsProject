using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatEnemy : EnemyControl
{
	public LayerMask layer;
	private bool wallCheck = false;

	protected override void Update()
	{
		base.Update();
		float dir = transform.localScale.x > 0 ? 1 : -1;
		wallCheck = Physics2D.Raycast(transform.position, Vector2.right * dir, 1f, layer);
		animator.SetBool("wallCheck", wallCheck);

		if (isAttack && !wallCheck)
		{
			rigid.velocity = new Vector2(speed * dir * Time.deltaTime, rigid.velocity.y);
		}
	}

	public override void Attack()
	{
		base.Attack();
		
	}
	public void attackEndEvent()
	{
		AttackEnd();
		StartCoroutine(AttackDelay());
	}
}
