using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatEnemy : EnemyControl
{
	public LayerMask layer;
	private bool wallCheck = false;
	public float attackSpeed;
	public AudioClip runAttackSound;
	public AudioClip strunSound;

	protected override void Update()
	{
		base.Update();

		animator.SetBool("wallCheck", wallCheck);
	}
	public IEnumerator AttackDash()
	{
		float dir =  transform.localScale.x > 0 ? 1 : -1;
		while (!wallCheck)
		{
			wallCheck = Physics2D.Raycast(transform.position - new Vector3(0,1,0), Vector2.right * dir, 2f, layer);
			Debug.DrawRay(transform.position - new Vector3(0,1,0), Vector2.right * dir * 2f, Color.red, 0.1f);
			if (isAttack && !wallCheck)
				rigid.velocity = new Vector2(attackSpeed * dir, rigid.velocity.y);
			if (health.dead)
				yield break;
			yield return null;
		}	
	}
	public void attackEndEvent()
	{
		AttackEnd();
		wallCheck = false;
	}
	public void Shake()
	{
		CameraManager.instance.ShakeCam(10, 0.5f);
	}
	public void AttackSound()
	{
		SoundManager.instance.SFXPlay(runAttackSound, transform.position, 0.8f);
	}
	public void SturnSound()
	{
		SoundManager.instance.SFXPlay(strunSound, transform.position, 0.4f);
	}
}
