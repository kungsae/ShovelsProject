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
				break;

			case State.ATTACK:
				enemy.Attack();
				state = State.STAY;
				break;

			case State.STAY:
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
		switch (state)
		{
			case State.PATROL:
				if ((fov.IsTracePlayer() && fov.IsViewPlayer()))
				{
					state = State.TRACE;
				}
					break;

			case State.TRACE:
				if ((transform.position - GameManager.instance.player.transform.position).sqrMagnitude < 5f)
				{
					state = State.ATTACK;
				}
					break;

			case State.ATTACK:

				break;

			case State.STAY:
				break;

			case State.DIE:
				break;

			default:
				break;
		}

	}
}
