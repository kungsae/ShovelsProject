using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    enum State
    {
        PATROL,
        TRACE,
        ATTACK,
		STAY,
        DIE
    }
    private State state;

    private EnemyControl enemy;
    private EnemyFOV fov;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		StateAction();
	}

    //���¿� ���� �ൿ�����ϴ� �Լ�
    private void StateAction()
    {
		switch (state)
		{
			case State.PATROL:
				StartCoroutine(enemy.StayPoint());
				break;

			case State.TRACE:
				enemy.Trace();
				break;


			case State.ATTACK:
				enemy.Attack();
				break;

			case State.DIE:
				break;

			default:
				break;
		}
	}

    //���� ���� ���ִ� �Լ�
    private void StateCheck()
    {
		float dist = (GameManager.instance.player.transform.position - transform.position).sqrMagnitude;

		//���ݻ�Ÿ� ����� ����            
		if (dist <= fov.attackRange * fov.attackRange)
		{
			if (fov.IsTracePlayer() && fov.IsViewPlayer())
			{
				state = State.ATTACK;
			}
		}
		else if (fov.IsTracePlayer() && fov.IsViewPlayer())
		{
			state = State.TRACE;
		}
		else
		{
			StartCoroutine(StateChange(State.PATROL, 2f));
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
			yield return new WaitForSeconds(waitTime);
			state = nextState;
			stateChange = false;
		}
	}

	///<summary>
	///�÷��̾�� ������ �Ÿ�
	///</summary>
	private float PlayerDistance()
	{
		return (transform.position - GameManager.instance.player.transform.position).sqrMagnitude;
	}
}
