using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    private float dir;

    public bool isStop;
    public bool isAttack = false;
    public bool canAttack = true;

    public float attackDelay;
    private WaitForSeconds attackDelayWaitSecond;

    public float speed = 1f;

    protected Rigidbody2D rigid;
    protected Animator animator;

    public Vector3[] patrollPoint;
    public int patrollIndex = 0;
    Vector3 destination;

    EnemyHealth health;

    public bool facingRight = true;

    private void Awake()
	{
        health = GetComponent<EnemyHealth>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dir = (destination.x - transform.position.x) > 0 ? 1 : -1;
    }
	private void Start()
	{
        attackDelayWaitSecond = new WaitForSeconds(attackDelay);
        destination = patrollPoint[patrollIndex];
    }
	protected virtual void Update()
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
    protected virtual void FixedUpdate()
	{
        if (health.dead)
        {
            return;
        }
        if (!isStop)
        {
            if (!isAttack)
            {
                rigid.velocity = new Vector2(speed * dir * Time.deltaTime, rigid.velocity.y);
                //IsStayPoint();

                if ((facingRight && rigid.velocity.x < 0) || (!facingRight && rigid.velocity.x > 0))
                {
                    Flip();
                }
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
	public virtual void Attack()
    {
        isAttack = true;
        canAttack = false;
    }
    //�ִϸ��̼� ������ ���� ���ִ� �Լ�,��ũ��Ʈ���� ���� ����
    public void AttackEnd()
    {
        float dir = GameManager.instance.player.transform.position.x - transform.position.x > 0 ? 1 : -1;
        if ((facingRight && dir < 0) || (!facingRight && dir > 0))
        {
            Flip();
        }
        isAttack = false;
        StartCoroutine(AttackDelay());
    }
    //���� ��Ÿ��
    private IEnumerator AttackDelay()
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
        if (!isStop)
        {
            isStop = true;
            yield return new WaitForSeconds(waitTime);
            isStop = false;
        }
    }
    public void DieAnimation()
    {
        animator.SetTrigger("dead");
    }
    protected void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        facingRight = !facingRight;
        transform.localScale = scale;
    }
}
