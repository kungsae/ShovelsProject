using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public bool facingRight = true;
    public bool isStop;
    public float dir;


    Rigidbody2D rigid;

    Vector3 destination;
    public float speed = 1f;
    private Animator animator;

    public Vector2[] patrollPoint;
    public int patrollIndex = 0;

    private void Awake()
	{
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
	private void Start()
	{
        //테스트용
        Trace();
    }
	private void Update()
	{
        animator.SetBool("isMove", !isStop);
        if (isStop)
        {
            return;
        }
        

        rigid.velocity = new Vector2(speed*dir* Time.deltaTime, rigid.velocity.y);

		
		if (((facingRight && rigid.velocity.x < 0) || (!facingRight && rigid.velocity.x > 0))&&!isStop)
        {
            Flip();
        }
    }

    public void NextPatrollPoint()
    {
        patrollIndex = patrollIndex + 1 % patrollPoint.Length;
        destination = patrollPoint[patrollIndex];
    }
    public void Trace()
    {
        destination = GameManager.instance.player.transform.position;
        dir = (destination.x - transform.position.x) > 0 ? 1 : -1;
        //rigid.velocity = new Vector2(speed * dir, 0) * Time.deltaTime;
        isStop = false;
    }
    public void IsStayPoint()
    {
        if (Mathf.Abs(destination.x - transform.position.x) < 0.5f || Mathf.Abs(GameManager.instance.player.transform.position.x - transform.position.x) < 1f)
        {
            StartCoroutine(StayPoint());
        }
    }
    IEnumerator StayPoint()
    {
        isStop = true;
        yield return new WaitForSeconds(1f);
        NextPatrollPoint();
    }
	private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        facingRight = !facingRight;
        transform.localScale = scale;
    }
}
