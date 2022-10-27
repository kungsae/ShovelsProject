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

    public float speed = 1f;

    protected Rigidbody2D rigid;
    protected Animator animator;

    public Vector3[] patrollPoint;
    public int patrollIndex = 0;
    private Vector3 destination;

    protected EnemyHealth health;
    protected EnemyAi ai;

    public AudioClip attack1;
    public AudioClip attack2;

    public bool facingRight = true;

    protected virtual void Awake()
    {
        health = GetComponent<EnemyHealth>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ai = GetComponent<EnemyAi>();
    }
	protected virtual void Start()
    {
        destination = patrollPoint[patrollIndex];
        dir = (destination.x - transform.position.x) > 0 ? 1 : -1;
    }
	protected virtual void Update()
    {
        animator.SetFloat("moveSpeed", Mathf.Abs(rigid.velocity.x));
        animator.SetBool("isAttack", isAttack);
        animator.SetBool("isMove", !isStop);
		
	}
    protected virtual void FixedUpdate()
	{
        if (health.dead)
        {
            return;
        }
        if (!isStop)
        {
            if (ai.state != EnemyAi.State.ATTACK&&!isStop&&!isAttack)
            {
                rigid.velocity = new Vector2(speed * dir, rigid.velocity.y);
                if ((facingRight && rigid.velocity.x < 0) || (!facingRight && rigid.velocity.x > 0))
                {
                    Flip();
                }
            }
        }
    }
    //����
	public virtual void Attack()
    {
          isAttack = true;
          canAttack = false;
    }
    //�ִϸ��̼� ������ ���� ���ִ� �Լ�,��ũ��Ʈ���� ���� ���� 
    public virtual void AttackEnd()
    {
        isAttack = false;
        float dir = GameManager.instance.player.transform.position.x - transform.position.x > 0 ? 1 : -1;
        if ((facingRight && dir < 0) || (!facingRight && dir > 0))
        {
            Flip();
        }
        StartCoroutine(AttackDelay());
    }
    //���� ��Ÿ��
    protected IEnumerator AttackDelay()
    {
         yield return new WaitForSeconds(attackDelay);
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
        animator.Update(0);
    }
    public void AttackSound()
    {
        SoundManager.instance.SFXPlay(attack1, transform.position,0.2f);
    }
    public void AttackSound2()
    {
        SoundManager.instance.SFXPlay(attack2, transform.position,0.5f);
    }
    protected void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        facingRight = !facingRight;
        transform.localScale = scale;
    }
}
