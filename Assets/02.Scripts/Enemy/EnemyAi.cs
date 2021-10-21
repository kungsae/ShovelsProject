using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public enum State
    {
        PATROL,
        TRACE,
        ATTACK,
		STAY,
        DIE
    }
    public State state;

    private EnemyControl enemy;
    private EnemyFOV fov;
	private bool isDie = false;

	private bool stateChange = false;
	// Start is called before the first frame update
	private void Awake()
	{ 
		enemy = GetComponent<EnemyControl>();
        fov = GetComponent<EnemyFOV>();
	}
	void Start()
    {
        state = State.PATROL;
		StartCoroutine(StateCheck());
		StartCoroutine(StateAction());
    }

    //���¿� ���� �ൿ�����ϴ� �Լ�
    private IEnumerator StateAction()
    {
		while (!isDie)
		{
			if (state == State.DIE)
			{
				yield break;
			}
			switch (state)
			{
				case State.PATROL:
					StartCoroutine(enemy.StayPoint());
					break;

				case State.TRACE:
					enemy.Trace();
					break;


				case State.ATTACK:
					if (enemy.canAttack)
					{
						enemy.Attack();
					}
					else
					{
						StartCoroutine(enemy.StayState(3f));
					}

					break;

				case State.DIE:
					isDie = true;
					enemy.DieAnimation();

					break;
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

    //���� ���� ���ִ� �Լ�
    private IEnumerator StateCheck()
    {
		while (!isDie)
		{
			float dist = (transform.position - GameManager.instance.player.transform.position).sqrMagnitude;
			float dist_X = Mathf.Abs(transform.position.x - GameManager.instance.player.transform.position.x);
			//���ݻ�Ÿ� ����� ����
			if (state != State.PATROL)
			{
				if (dist <= fov.attackRange * fov.attackRange)
				{
					state = State.ATTACK;
				}
				else if (dist <= fov.viewRange*fov.viewRange&& dist > fov.attackRange * fov.attackRange)
				{
					state = State.TRACE;
					if (dist_X <= 0.5f)
					{
						StartCoroutine(StateChange(State.TRACE, 2f));
					}
				}
				else if (fov.aggroRange * fov.aggroRange < dist)
				{
					StartCoroutine(StateChange(State.PATROL, 2f));
				}
			}
			else
			{
				if (fov.IsTracePlayer() && fov.IsViewPlayer())
				{
					state = State.TRACE;
				}
				else
				{
					state = State.PATROL;
				}
			}

			yield return new WaitForSeconds(0.1f);
		}
	}

	///<summary>
	///���� ��ȯ ���� ��� ���ߴ� �Լ�
	///</summary>
	private IEnumerator StateChange(State nextState, float waitTime)
	{
		if (!stateChange)
		{
			stateChange = true;
			StartCoroutine(enemy.StayState(waitTime));
			yield return new WaitForSeconds(waitTime);
			state = nextState;
			stateChange = false;
		}
	}
	public void SetDead()
	{
		state = State.DIE;
		StartCoroutine(DeadProcess());
	}
	IEnumerator DeadProcess()
	{
		yield return new WaitForSeconds(5f);
		//gameObject.SetActive(false);
		Destroy(gameObject);
	}
}
