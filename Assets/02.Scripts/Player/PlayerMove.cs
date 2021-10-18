using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : LivingEntity
{
    private PlayerInput playerInput;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;

    public LayerMask whatIsGround;
    public LayerMask whatIsEnemy;
    public LayerMask notPlayer;
    public RaycastHit2D hit;

    public GameObject groundCheckObj;
    public bool isGround = false;
    
    public bool facingRight = true;

    public bool canAttack = true;
    public bool canHit = true;

    public bool isAttack = false;
    public bool isJump = false;
    public bool isOnDamaged = false;
    public bool isfalling = false;
    public bool isInvincible = false;

    //이동 관련 수치
    public float groundCheckDistance;
    public float attackCheckDistance;
    public float jumpPower = 3f;
    public float moveSpeed;
    public float jumpTime;


    private float xMove;
    private float jump;

    public float x;
    public float y;

    public GameObject aiPrefab;
    public Transform afterImageTrm;
    public float aiCreateTerm = 0.3f;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        rigid = GetComponentInParent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log(rigid);

        PoolManager.CreatPool<Afterimage>(aiPrefab, afterImageTrm, 20);
        jump = jumpPower;
    }
    private void Update()
    {
        xMove = playerInput.xMove;

        Debug.DrawRay(groundCheckObj.transform.position, Vector2.down * groundCheckDistance, Color.red);
        Debug.DrawRay(groundCheckObj.transform.position + new Vector3(0.1f,0,0), Vector2.down * attackCheckDistance, Color.green);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Collider2D enemy = hit.collider;
                if (isfalling && hit && !isOnDamaged && hit.point.y < groundCheckObj.transform.position.y && !isInvincible)
                {
                    EnemyDamage(enemy, damage, 1);
                }
                else if (!isInvincible)
                {
                    PlayerDamage(enemy);
                }
            }
            if (hit.collider.gameObject.CompareTag("Hit"))
            {
                Collider2D enemy = hit.collider;
                if (isfalling && hit && !isOnDamaged && hit.point.y < groundCheckObj.transform.position.y&& !isInvincible)
                {
                    EnemyDamage(enemy, damage, 4);
                }
                else if (!isInvincible)
                {
                    PlayerDamage(enemy);
                }
            }
        }

        if (((facingRight && xMove < 0) || (!facingRight && xMove > 0))&& !isOnDamaged)
        {
            Flip();
        }

        if (isGround||hit)
        {
            if (playerInput.jump&&!isOnDamaged)
            {
                jump = jumpPower*2;
            }
            if (isOnDamaged)
            {
                jump = jumpPower;
            }

        }
        else
        {
            if (rigid.velocity.y < 0)
            {
                isfalling = true;
            }
            if (playerInput.attack&& !isOnDamaged)
            {
                StartCoroutine(Attack());
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(/*groundCheckObj.*/transform.position, new Vector3(x, y));
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        isGround = Physics2D.BoxCast(/*groundCheckObj*/transform.position, new Vector2(x, y), 0, Vector2.down, 0.1f, whatIsGround);
        hit = Physics2D.BoxCast(/*groundCheckObj.*/transform.position, new Vector2(x, y), 0, Vector2.down, 0.1f, whatIsEnemy);
        canAttack = !Physics2D.Raycast(groundCheckObj.transform.position, Vector2.down, attackCheckDistance, ~(1 << 7));



        if (isGround)
        {
            if(!isOnDamaged)
            StartCoroutine(JumpDelay());

            isfalling = false;
            isAttack = false;
            isOnDamaged = false;
        }
        else
        {
            isJump = false;
        }
        if (!isGround && !isAttack && !isOnDamaged)
        {
            rigid.velocity = new Vector3(xMove * moveSpeed, rigid.velocity.y);
        }
        else if(!isOnDamaged)
        {
            rigid.velocity = new Vector3(0, rigid.velocity.y);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (isGround && collision.gameObject.CompareTag("Ground"))
        //{
        //    isfalling = false;
        //    isAttack = false;
        //    isOnDamaged = false;

        //    StartCoroutine(JumpDelay());
        //}


        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    if (isfalling && hit && !isOnDamaged && collision.transform.position.y < groundCheckObj.transform.position.y)
        //    {
        //        Debug.Log("A");
        //        EnemyDamage(collision,damage, 1);
        //    }
        //    else if (!isOnDamaged)
        //    {
        //        PlayerDamage(collision);
        //    }
        //}

  //      if (collision.gameObject.CompareTag("Hit"))
  //      {
  //          if (/*isfalling &&*/hit && !isOnDamaged && collision.transform.position.y < groundCheckObj.transform.position.y)
  //          {
  //              if(collision.gameObject.name == "Head")
  //              Debug.Log(collision.gameObject.name);
  //              EnemyDamage(collision,damage,4);
  //          }
  //          else if (!isOnDamaged)
  //          {
  //              PlayerDamage(collision);
  //          }

		//}
    }
	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.CompareTag("Damage")&&!isInvincible)
        {
            PlayerDamage(collision);
        }
    }


	public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal, float damageDrng)
	{
		base.OnDamage(damage, hitPosition, hitNormal, damageDrng);

        isOnDamaged = true;
        OnDamageEffect(hitPosition);
        StartCoroutine(Invincible());

        //무적 레이어
        gameObject.layer = 8;
    }
    public void OnDamageEffect(Vector3 hitPosition)
    {
        int dir = transform.position.x - hitPosition.x > 0 ? 1 : -1;
        //Vector2 dir = (transform.position - targetPos);

        rigid.velocity = new Vector2(dir, 1)*5;
        //rigid.AddForce(new Vector2(dir, 1) * 5, ForceMode2D.Impulse);
        Debug.Log(dir);
        // rigid.AddForce(dir*5, ForceMode2D.Impulse);
    }
    IEnumerator Attack()
    {
        if (!isAttack&&canAttack)
        {
            isAttack = true;
            canAttack = false;
            rigid.velocity = new Vector2(0, 0);

            rigid.gravityScale = 0;
            yield return new WaitForSeconds(0.2f);
            rigid.gravityScale = 3;
            rigid.AddForce(new Vector2(0,-30f),ForceMode2D.Impulse);
        }

        float time = 0;
        float aftertime = 0;
        float TargetTime = Random.Range(0.03f, 0.06f);

        while (isAttack)
        {
            time += Time.deltaTime;
            aftertime += Time.deltaTime;

            if (aftertime >= TargetTime)
            {
                Afterimage ai = PoolManager.GetItem<Afterimage>();
                ai.SetSprite(spriteRenderer.sprite, spriteRenderer.flipX, transform.position);
                TargetTime = Random.Range(0.01f, 0.02f);
                aftertime = 0;
            }
            yield return null;
        }

    }
    //IEnumerator HitDelay()
    //{
    //    yield return new WaitForSeconds(0.2f);
    //    canHit = true;
    //}
    IEnumerator Invincible()
    {
        isInvincible = true;
        StartCoroutine(InvincibleEffect());
        yield return new WaitForSeconds(1.5f);
        isInvincible = false;

        //플레이어 레이어
        gameObject.layer = 7;

    }
    IEnumerator InvincibleEffect()
    {
		for (int i = 0; i < 3; i++)
		{
            sprite.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.25f);
            sprite.color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(0.25f);
        }
    }
    IEnumerator JumpDelay()
    {
        if (!isJump)
        {
            isJump = true;
            yield return new WaitForSeconds(jumpTime);
            Jump();
        }

    }
    public void Jump()
    {
        if (!isOnDamaged)
        {
            rigid.velocity = new Vector2(0, jump);
        }
        //rigid.AddForce(Vector2.up * (jump), ForceMode2D.Impulse);
        jump = jumpPower;   
    }
    public void PlayerDamage(Collision2D collision)
    {
        LivingEntity target = collision.transform.GetComponentInParent<LivingEntity>();
        if (target != null&&!isOnDamaged)
        {
            OnDamage(target.damage, collision.transform.position, hit.normal, 1f);
        }
    }
    public void PlayerDamage(Collider2D collision)
    {
        LivingEntity target = collision.transform.GetComponentInParent<LivingEntity>();
        if (target != null && !isOnDamaged)
        {
            OnDamage(target.damage, collision.transform.position, hit.normal, 1f);
        }

    }
    public void EnemyDamage(Collision2D collision, float damage, float damageDrng)
    {
        if (isAttack)
        {
            damage += 1;
        }
        IDamageable target = collision.transform.GetComponentInParent<IDamageable>();
        if (target != null)
        {
            target.OnDamage(damage, hit.point, hit.normal, damageDrng);
            Jump();
            isAttack = false;
        }
    }
    public void EnemyDamage(Collider2D collision, float damage, float damageDrng)
    {
        if (isAttack)
        {
            damage += 1;
        }
        IDamageable target = collision.transform.GetComponentInParent<IDamageable>();
        if (target != null)
        {
            target.OnDamage(damage, hit.point, hit.normal, damageDrng);
            Jump();
            isAttack = false;
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        facingRight = !facingRight;
        transform.localScale = scale;
    }
}
