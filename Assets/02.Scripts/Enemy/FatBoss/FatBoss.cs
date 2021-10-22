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
				rigid.velocity = new Vector2(attackSpeed * dir * Time.deltaTime, rigid.velocity.y);
			yield return null;
		}

	}

	public IEnumerator AttackSpike()
    {
		float dir = transform.localScale.x > 0 ? 1 : -1;
		if(this.gameObject.CompareTag("Player")&&damagedtime > 3)
        {
			yield return null;
		}

	}

	public void attackEndEvent()
	{
		AttackEnd();
		wallCheck = false;
	}
}
