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

			case State.STAY:
				
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
    private IEnumerator StateCheck()
    {


		while (true)
		{
			float dist = (transform.position - GameManager.instance.player.transform.position).sqrMagnitude;
			float dist_X = Mathf.Abs(transform.position.x - GameManager.instance.player.transform.position.x);
			//���ݻ�Ÿ� ����� ����
			Debug.Log(dist);
			if (dist <= fov.attackRange * fov.attackRange)
			{
				if (fov.IsTracePlayer() && fov.IsViewPlayer())
				{
					state = State.ATTACK;
				}
			}
			//�ؿ��� ���� ���¿� ���� �׳� Ʈ���̽� �ϴ��� �þ� �ȿ� �־�� Ʈ���̽� �ϴ����� �ٲܿ���
			else if (fov.IsTracePlayer() && fov.IsViewPlayer())
			{
				state = State.TRACE;
			}
			else if (state != State.PATROL && fov.aggroRange * fov.aggroRange < dist)
			{
				StartCoroutine(StateChange(State.PATROL, 2f));
			}
			yield return new WaitForSeconds(0.5f);
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
}
