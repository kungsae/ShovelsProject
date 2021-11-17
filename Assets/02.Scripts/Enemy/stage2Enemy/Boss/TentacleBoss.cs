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
				fov.attackRange = originalAttackRange;
				break;
			case 2:
			case 3:
				fov.attackRange = originalAttackRange*5;
				break;
			default:
				break;
		}
	}
	private void FlipDir()
	{
		int flipDir = GameManager.instance.player.transform.position.x - transform.position.x > 0 ? 1 : -1;
		if ((facingRight && flipDir < 0) || (!facingRight && flipDir > 0))
		{
			Flip();
		}
	}
	public void BackDash()
	{
		int dir = GameManager.instance.player.transform.position.x - transform.position.x > 0 ? 1 : -1;
		FlipDir();
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
		FlipDir();
		GameObject projection = Instantiate(projectionObject, transform.position, Quaternion.identity);
		Vector3 dir = GameManager.instance.player.transform.position - projection.transform.position;
		projection.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);	
	}
	public void MultieShootProjectionAttack()
	{
		FlipDir();
		for (int i = 0; i <= 5; i++)
		{
			GameObject projection = Instantiate(projectionObject, transform.position, Quaternion.identity);
			int dir = GameManager.instance.player.transform.position.x - transform.position.x > 0 ? 0 : 90;
			projection.transform.rotation = Quaternion.Euler(new Vector3(0,0,dir+(i*18)));
		}

	}
	public IEnumerator ContinuousShootProjectionAttack()
	{
		FlipDir();
		for (int i = 0; i < 5; i++)
		{
			ProjectionAttack();
			yield return new WaitForSeconds(0.1f);
		}

	}
}
