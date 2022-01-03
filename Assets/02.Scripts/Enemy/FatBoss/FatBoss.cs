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

	public AudioClip runAttackSound;
	public AudioClip strunSound;
	public AudioClip spikeSound;
	public AudioClip shoutSound;
	protected override void Start()
	{
		base.Start();
		GetComponent<EnemyAi>().state = EnemyAi.State.TRACE;
		health.hitEvent += () => 
		{
			attackType = 3;
			Attack();
		};
		health.hitEvent2 += () =>
		{
			canDash = true;
			attackType = 2;
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
		float dist_X = Mathf.Abs(transform.position.x - GameManager.instance.player.transform.position.x);
		if (dist_X > 10&&!canDash)
		{
			canDash = true;
			Attack();
		}
	}

	public IEnumerator AttackDash()
	{
		float dir = transform.localScale.x > 0 ? 1 : -1;
		while (!wallCheck)
		{
			wallCheck = Physics2D.Raycast(transform.position, Vector2.right * dir, 2f, layer);
			Debug.DrawRay(transform.position, Vector2.right * dir * 2f, Color.red, 0.1f);
			if (isAttack && !wallCheck)
			{
				rigid.velocity = new Vector2(attackSpeed * dir, rigid.velocity.y);
			}
			else
			{
				CameraManager.instance.ShakeCam(10, 0.3f);
			}	

			yield return null;
		}

	}

	public void DashAttackEndEvent()
	{
		AttackEnd();
		wallCheck = false;
		canDash = false;
		//StartCoroutine(DashDelay());
	}
	//IEnumerator DashDelay()
	//{
	//	yield return new WaitForSeconds(dashAttackDelay);
	//	canDash = true;
	//	//canAttack = true;
	//	attackType = 2;
	//	Attack();
	//}
	public void Shout()
	{
		CameraManager.instance.ShakeCam(10, 1.5f);
		print("A");
		SoundManager.instance.SFXPlay(shoutSound, transform.position, 0.8f);
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
	public void DieEvent()
	{
		GameManager.instance.OpneDoor();
		CameraManager.instance.followCamChange(CameraManager.instance.mainCam);
		SoundManager.instance.ChageBgm(0);
	}
	public void AttackSound()
	{
		SoundManager.instance.SFXPlay(runAttackSound, transform.position, 0.8f);
	}
	public void SturnSound()
	{
		CameraManager.instance.ShakeCam(10, 0.5f);
		SoundManager.instance.SFXPlay(strunSound, transform.position, 0.4f);
	}
}
