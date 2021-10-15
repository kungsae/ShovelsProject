using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    enum State
    {
        PATROL,
        TRACE,
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

    //상태에 따른 행동실행하는 함수
    private void StateAction()
    {
		switch (state)
		{
			case State.PATROL:
				StartCoroutine(enemy.StayPoint());
				break;

			case State.TRACE:
				//if ()
				{
					
				}
				break;


			case State.STAY:
				break;

			case State.DIE:
				break;

			default:
				break;
		}
	}

    //상태 변경 해주는 함수
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
					
				}
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
