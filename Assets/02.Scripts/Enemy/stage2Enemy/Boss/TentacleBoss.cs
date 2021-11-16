using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleBoss : EnemyControl
{
	private int attackType = 1;
	private EnemyFOV fov;
	public GameObject projectionObject;
	private float originalAttackRange;
	protected override void Awake()
	{
		base.Awake();
		health.hitEvent = BackDashAttack;
		fov = GetComponent<EnemyFOV>();
		originalAttackRange = fov.attackRange;
	}
	public override void Attack()
	{
		animator.SetInteger("AttackType", attackType);
		base.Attack();		
	}
	public override void AttackEnd()
	{
		attackDelay = Random.Range(attackDelay - 1f, attackDelay + 1f);
		attackType = Random.Range(1, 4);
		print("µô·¹ÀÌ : " + attackDelay + "Å¸ÀÔ" + attackType);
		base.AttackEnd();
		switch (attackType)
		{
			case 1:
				fov.aggroRange = originalAttackRange;
				break;
			case 2:
				fov.aggroRange = originalAttackRange*3;
				break;
			case 3:
				fov.aggroRange = originalAttackRange * 2 ;
				break;
			default:
				break;
		}
	}
	public void BackDash()
	{
		int dir = GameManager.instance.player.transform.position.x - transform.position.x > 0 ? 1 : -1;
		if ((facingRight && dir < 0) || (!facingRight && dir > 0))
		{
			Flip();
		}
		if (attackType != 4)
		rigid.AddForce(new Vector2(-dir*2, 1) * 30, ForceMode2D.Impulse);
		else
			rigid.AddForce(new Vector2(-dir, 1) * 30, ForceMode2D.Impulse);
	}
	public void BackDashAttack()
	{
		StopCoroutine(AttackDelay());
		attackType = 4;
		canAttack = true;
		isAttack = false;
		Attack();
	}
	public void ProjectionAttack()
	{
		Instantiate(projectionObject,transform.position,Quaternion.identity);
	}
}
