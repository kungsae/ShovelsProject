using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public float dir;

    public bool isStop;
    public bool isAttack = false;
    public bool canAttack = true;

    public float attackDelay;
    private WaitForSeconds attackDelayWaitSecond;

    public float speed = 1f;

    Rigidbody2D rigid;
    private Animator animator;

    public Vector3[] patrollPoint;
    public int patrollIndex = 0;
    Vector3 destination;

    public bool facingRight = true;

    private void Awake()
	{
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dir = (destination.x - transform.position.x) > 0 ? 1 : -1;
    }
	private void Start()
	{
        attackDelayWaitSecond = new WaitForSeconds(attackDelay);
        destination = patrollPoint[patrollIndex];
    }
	private void Update()
	{
        animator.SetFloat("moveSpeed", Mathf.Abs(rigid.velocity.x));
        animator.SetBool("isAttack", isAttack);
        animator.SetBool("isMove", !isStop);

		//�׽�Ʈ

		//if ((transform.position - GameManager.instance.player.transform.position).sqrMagnitude < 5f)
		//{
		//	Attack();
		//}

		
	}
	private void FixedUpdate()
	{
        if (isStop)
        {
            return;
        }

        if (!isStop)
        {
            if (!isAttack)
            {
                rigid.velocity = new Vector2(speed * dir * Time.deltaTime, rigid.velocity.y);
                //IsStayPoint();
            }

            if (((facingRight && rigid.velocity.x < 0) || (!facingRight && rigid.velocity.x > 0)) && !isAttack)
            {
                Flip();
            }
        }
        //�׽�Ʈ

        //if ((transform.position - GameManager.instance.player.transform.position).sqrMagnitude < 5f)
        //{
        //    Attack();
        //}

        //if (!isAttack)
        //{
        //    rigid.velocity = new Vector2(speed * dir * Time.deltaTime, rigid.velocity.y);
        //    //IsStayPoint();
        //}
    }
    //����
	public void Attack()
    {
        if (canAttack)
        {
            isAttack = true;
            canAttack = false;
            StartCoroutine(AttackDelay());
        }
        else
        {
            StartCoroutine(StayState(2f));
        }
    }
    //�ִϸ��̼� ������ ���� ���ִ� �Լ�,��ũ��Ʈ���� ���� ����
    public void AttackEnd()
    {
        isAttack = false;
    }
    //���� ��Ÿ��
    public IEnumerator AttackDelay()
    {
        yield return attackDelayWaitSecond;
        canAttack = true;
    }


    //�̵� ��ǥ ����,���� ���� ����
    public void Trace()
    {
        destination = GameManager.instance.player.transform.position;
        dir = (destination.x - transform.position.x) > 0 ? 1 : -1;
        //isStop = false;
    }
    //�̵� ��ǥ�� ��Ʈ�� ����Ʈ�� ��Ʈ�� ����Ʈ�� ����,������� ����
    public void NextPatrollPoint()
    {
        patrollIndex = (patrollIndex + 1) % patrollPoint.Length;
        destination = patrollPoint[patrollIndex];
        dir = (destination.x - transform.position.x) > 0 ? 1 : -1;
        isStop = false;
    }
    //��� ���� ��ġ���� ���߰� ���� ��Ʈ�� ����Ʈ�� �̵� �Ǵ� �÷��̾� ����
    public IEnumerator StayPoint()
    {
        float dist = Mathf.Abs(patrollPoint[patrollIndex].x - transform.position.x);
        if (!isStop&&dist < 0.5f)
        {
            isStop = true;
            yield return new WaitForSeconds(2f);
            NextPatrollPoint();
        }
    }
    public IEnumerator StayState(float waitTime)
    {
        isStop = true;
        rigid.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(waitTime);
        isStop = false;
    }
    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        facingRight = !facingRight;
        transform.localScale = scale;
    }
}
