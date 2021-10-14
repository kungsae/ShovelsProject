using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
	//�� ��ũ��Ʈ EnemyMove�� ���� �Ұ�

	private bool isAttack = false;
	private bool canAttack = true;

	public float waitForSecondsVal;
	private WaitForSeconds attackDelayWaitSecond;


	private Animator animator;
	private void Awake()
	{
		attackDelayWaitSecond = new WaitForSeconds(waitForSecondsVal);
		animator = GetComponent<Animator>();
	}
	private void Update()
	{

		animator.SetBool("isAttack", isAttack);
		//�׽�Ʈ
		if ((transform.position - GameManager.instance.player.transform.position).sqrMagnitude < 5f)
		{
			Attack();
		}
	}
	protected void Attack()
	{
		if (canAttack)
		{
			isAttack = true;
			canAttack = false;
			StartCoroutine(AttackDelay());
		}
	}
	public void AttackEnd()
	{
		isAttack = false;
	}
	public IEnumerator AttackDelay()
	{
		yield return attackDelayWaitSecond;
		canAttack = true;
	}
}
