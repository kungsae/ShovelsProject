using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerStat
{
    private PlayerInput playerInput;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    public GameObject fireParticle;
    public GameObject dustParticle;
    public ParticleSystem ParryingParticle;

    public LayerMask whatIsGround;
    public LayerMask whatIsEnemy;
    public LayerMask notPlayer;

    public RaycastHit2D hitRay;
    public RaycastHit2D attackRay;

    public GameObject groundCheckObj;
    public bool isGround = false;
    
    private bool facingRight = true;

    private bool canAttack = true;
    private bool canParryied = true;

    private bool energyRecover;
    private bool isAttack = false;
    private bool isJump = false;
    private bool isOnDamaged = false;
    private bool parryied = false;

    //private bool isfalling = false;
    private bool isInvincible = false;
    public float invincibleTime = 1.5f;

    //이동 관련 수치
    public float groundCheckRadius;
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

    public GameObject hitPos;
   

    [Header("카메라 흔들림")]
    public float intensity;
    public float shakeTime;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        rigidBody = GetComponentInParent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        PoolManager.CreatPool<Afterimage>(aiPrefab, afterImageTrm, 20);
        jump = jumpPower;
    }
	private void Start()
	{
        energy = maxEnergy;
        UIManager.instance.StatUpdate();
        StartCoroutine(EnergyRecover());
	}
	private void Update()
    {
        if (dead)
            return;

        xMove = playerInput.xMove;

        if (Input.GetKeyDown(KeyCode.G))
        {
            OnDamage(1, transform.position, transform.position, 1);
        }
        //바닥체크
        if (isGround || attackRay)
        {
            if (playerInput.jump && !isOnDamaged && energy > 0)
            {
                jump = jumpPower * 2;
            }
            else
            {
                jump = jumpPower;
            }
        }
        //체공중
        else
        {
            if (playerInput.parrying&&canParryied&&!isAttack)
            {
                parryied = true;
                StartCoroutine(Parrying());
            }
            if (rigidBody.velocity.y < 0)
            {
                //isfalling = true;
            }
            if (playerInput.attack && !isOnDamaged && energy > 0&&!parryied)
            {
                StartCoroutine(Attack());
            }
        }

        //데미지 입는 부분
        if (hitRay.collider != null)
        {
            if (hitRay.collider.gameObject.CompareTag("Enemy")|| hitRay.collider.gameObject.CompareTag("Hit"))
            {
                Collider2D enemy = hitRay.collider;
                if(!isOnDamaged && !isInvincible)
                PlayerDamage(enemy);
            }
        }
        //데미지 주는부분
        if (attackRay.collider != null)
        {
            if (attackRay.collider.gameObject.CompareTag("Enemy") || attackRay.collider.gameObject.CompareTag("Hit"))
            {
                Collider2D enemy = attackRay.collider;
                if (!isOnDamaged && !isInvincible)
                    EnemyDamage(enemy, damage, 1);
            }
			else if (attackRay.collider.gameObject.CompareTag("Damage") && parryied)
			{
                SucceesParrying();
            }
		}

        if (((facingRight && xMove < 0) || (!facingRight && xMove > 0))&& !isOnDamaged)
        {
            Flip();
        }


    }
    // Update is called once per frame
    void FixedUpdate()
    {

        isGround = Physics2D.BoxCast(groundCheckObj.transform.position, new Vector2(0.5f,0.5f), 0, Vector2.down, 0.1f, whatIsGround);
        attackRay = Physics2D.BoxCast(groundCheckObj.transform.position, new Vector2(0.4f, 0.4f), 0, Vector2.down, 0.1f, whatIsEnemy);
        //데미지 입는 부분
        hitRay = Physics2D.BoxCast(hitPos.transform.position, new Vector2(x, y),0,Vector2.zero,0.1f,whatIsEnemy);

        //땅과 거리 계산
        canAttack = !Physics2D.Raycast(groundCheckObj.transform.position, Vector2.down, attackCheckDistance, ~(1 << 7) + ~(1 << 8));

        if (dead)
        {
            if (isGround)
            {
                rigidBody.velocity = new Vector2(0, 0);
            }
            return;
        }

        if (isGround&&rigidBody.velocity.y<1)
        {
            if(isAttack)
            {
                Instantiate(dustParticle, ParryingParticle.transform.position, Quaternion.identity);
            }
            if (!isOnDamaged && !parryied)
            {
                StartCoroutine(JumpDelay());
            }
            isAttack = false;
            isOnDamaged = false;
        }
        else
        {
            isJump = false;
        }
        if (!isGround && !isAttack && !isOnDamaged)
        {
            rigidBody.velocity = new Vector3(xMove * moveSpeed, rigidBody.velocity.y);
        }
        else if(!isOnDamaged)
        {
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(hitPos.transform.position, new Vector3(x, y));
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundCheckObj.transform.position, Vector2.down * groundCheckRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundCheckObj.transform.position + new Vector3(0.1f, 0, 0), Vector2.down * attackCheckDistance);

        Gizmos.DrawWireCube(groundCheckObj.transform.position, new Vector3(0.4f, 0.4f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.CompareTag("Damage")&&!isInvincible)
        {
            rigidBody.velocity = new Vector2(0, 0);
            if (parryied)
            {
                SucceesParrying();
            }
            else
            PlayerDamage(collision);
        }
    }


	public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal, float damageDrng)
	{
		base.OnDamage(damage, hitPosition, hitNormal, damageDrng);
        CameraShake.instance.ShakeCam(intensity, shakeTime);
        UIManager.instance.StatUpdate(true);
        isOnDamaged = true;
        OnDamageEffect(hitPosition);
        StartCoroutine(Invincible(invincibleTime,true));

        //무적 레이어
        gameObject.layer = 8;
    }
    public void OnDamageEffect(Vector3 hitPosition)
    {
        int dir = transform.position.x - hitPosition.x > 0 ? 1 : -1;
        rigidBody.velocity = new Vector2(0, 0);
        rigidBody.velocity = new Vector2(dir, 1)*5;
    }
    IEnumerator Attack()
    {
        if (!isAttack&&canAttack)
        {
            UseEnergy(1,true);
            fireParticle.SetActive(true);
            Invoke("fireParticleOff", 0.5f);
            isAttack = true;
            canAttack = false;
            rigidBody.velocity = new Vector2(0, 0);

            rigidBody.gravityScale = 0;
            yield return new WaitForSeconds(0.2f);
            rigidBody.gravityScale = 3;
            rigidBody.AddForce(new Vector2(0,-30f),ForceMode2D.Impulse);
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
                print(ai);
                ai.SetSprite(spriteRenderer.sprite, transform.localScale, transform.position);
                TargetTime = Random.Range(0.01f, 0.02f);
                aftertime = 0;
            }
            yield return null;
        }

    }
    IEnumerator Invincible(float time,bool onEffect)
    {
        isInvincible = true;
        if (onEffect)
        {
            StartCoroutine(InvincibleEffect(time));
        }
        yield return new WaitForSeconds(time);
        isInvincible = false;

        //플레이어 레이어
        gameObject.layer = 7;

    }
    IEnumerator InvincibleEffect(float time)
    {
        int count = 3;
		for (int i = 0; i < count; i++)
		{
			sprite.color = new Color(1, 1, 1, 0.5f);
			yield return new WaitForSeconds(time / (count*2));
            sprite.color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(time / (count*2));
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
            rigidBody.velocity = new Vector2(0, jump);
            if (jump > jumpPower)
            {
                UseEnergy(1,true);
            }
        }
        //rigid.AddForce(Vector2.up * (jump), ForceMode2D.Impulse);
        jump = jumpPower;   
    }

    public void PlayerDamage(Collision2D collision)
    {
        LivingEntity target = collision.transform.GetComponentInParent<LivingEntity>();
        if (target != null&&!isOnDamaged)
        {
            OnDamage(target.damage, collision.transform.position, hitRay.normal, 1f);
        }
    }
    public void PlayerDamage(Collider2D collision)
    {
        LivingEntity target = collision.transform.GetComponentInParent<LivingEntity>();
        if (target != null && !isOnDamaged)
        {
            OnDamage(target.damage, collision.transform.position, hitRay.normal, 1f);
        }
        else if(!isOnDamaged)
        {
            OnDamage(0, collision.transform.position, hitRay.normal, 1f);
        }

    }
    public void EnemyDamage(Collider2D collision, float damage, float damageDrng)
    {
        if (isAttack)
        {
            damage += 3;
        }
        Debug.Log(collision.transform.GetComponent<LivingEntity>());
        LivingEntity target = collision.transform.GetComponentInParent<LivingEntity>();
        if (target != null&&target.canDamage)
        {
            target.OnDamage(damage, hitRay.point, hitRay.normal, damageDrng);
            Jump();
            isAttack = false;   
        }
    }
    private void UseEnergy(int energeyConsumption,bool isDown)
    {
        if (isDown)
            energyRecover = false;
        UIManager.instance.EnergyUpdate(isDown);
        energy -= energeyConsumption;

    }
    private IEnumerator EnergyRecover()
    {
        float time = 0;

		while (true)
		{
            time += Time.deltaTime;
            if (!energyRecover)
            {
                time = 0;
                energyRecover = true;
            }
            if (time > 3&& energyRecover)
            {
                time = 2f;
                if(energy < maxEnergy)
                UseEnergy(-1, false);
            }
            yield return null;
		}
	}
    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        facingRight = !facingRight;
        transform.localScale = scale;
    }
    private void ParryingEnd()
    {
        if (parryied)
        {
            parryied = false;
            Debug.Log("AAAA");
        }
    }
    private IEnumerator Parrying()
    {
        parryied = true;
        float dir = transform.localScale.x;

        for (int i = 0; i < 36; i++)
		{
            Debug.Log("A");
            transform.Rotate(new Vector3(0, 0, 10 * dir));
            yield return new WaitForSeconds(0.0001f);
        }

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        parryied = false;
    }
    private void SucceesParrying()
    {
        Debug.Log("패링");

        rigidBody.velocity = new Vector2(0, 0);
        rigidBody.velocity = new Vector2(0, 2) * 5;

        rigid.velocity = new Vector2(0, 0);
        rigid.velocity = new Vector2(0, 3) * 5;
        UseEnergy(-1, false);

        ParryingParticle.Play();
        parryied = false;
        StartCoroutine(Invincible(0.5f,false));

        //무적 레이어
        gameObject.layer = 8;
    }
    private void fireParticleOff()
    {
        fireParticle.SetActive(false);
    }
}
