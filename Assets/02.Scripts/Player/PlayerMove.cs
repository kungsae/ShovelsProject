
using System.Collections;
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

    public bool rest = false;
    public bool isParrying = false;

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

    public float parryingDuration;

    [Header("카메라 흔들림")]
    public float intensity;
    public float shakeTime;


    [Header("사운드")]
    public AudioClip groundSound;
    public AudioClip parryingSound;
    public AudioClip hitSound;
    public AudioClip attackSound;
    public AudioClip attackHeadSound;


    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        rigidBody = GetComponentInParent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        PoolManager.CreatPool<Afterimage>(aiPrefab, afterImageTrm, 20);
        LoadStat();
        jump = jumpPower;
    }
	protected override void Start()
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

        if (Input.anyKeyDown)
        {
            rest = false;
        }
        //바닥체크
        if (isGround || attackRay)
        {
            if (playerInput.jump && !isOnDamaged && energy > 0)
            {
                jump = 20;
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
                StartCoroutine(Parrying());
                canParryied = false;
            }
            if (rigidBody.velocity.y < 0)
            {
                //isfalling = true;
            }
            if (playerInput.attack && !isOnDamaged && energy > 0&&!isParrying)
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
        if (attackRay.collider != null&& hitRay.collider == null)
        {
            if (attackRay.collider.gameObject.CompareTag("Enemy") || attackRay.collider.gameObject.CompareTag("Hit"))
            {
                Collider2D enemy = attackRay.collider;
                if (!isOnDamaged /*&& !isInvincible*/&& rigidBody.velocity.y < 1)
                {
                    if (!isParrying)
                        EnemyDamage(enemy, damage, 1);
					else
						PlayerDamage(enemy);
				}
                    
            }
			else if (attackRay.collider.gameObject.CompareTag("Damage") && isParrying&&!isOnDamaged)
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
        attackRay = Physics2D.BoxCast(groundCheckObj.transform.position, new Vector2(0.6f, 0.4f), 0, Vector2.down, 0.1f, whatIsEnemy);
        //데미지 입는 부분
        hitRay = Physics2D.BoxCast(hitPos.transform.position, new Vector2(x, y),0,Vector2.zero,0.1f,whatIsEnemy);

        //땅과 거리 계산
        canAttack = !Physics2D.Raycast(groundCheckObj.transform.position, Vector2.down, attackCheckDistance, notPlayer);

        if (dead)
        {
            if (isGround)
            {
                rigidBody.velocity = new Vector2(0, 0);
            }
            return;
        }

        //바닥체크
        if (isGround&&rigidBody.velocity.y<1)
        {
            if(isAttack)
            {
                Instantiate(dustParticle, ParryingParticle.transform.position, Quaternion.identity);
                CameraManager.instance.ShakeCam(intensity, shakeTime);
                SoundManager.instance.SFXPlay(attackHeadSound, transform.position, 0.8f);
            }
            if (!isOnDamaged && !isParrying&&!rest)
            {

                StartCoroutine(JumpDelay());
                canParryied = true;
            }
            if (isOnDamaged)
            {
                StartCoroutine(Invincible(invincibleTime, true));
            }
            isAttack = false;
            isOnDamaged = false;

        }
        else
        {
            isJump = false;
        }
        if (!isGround && !isAttack && !isOnDamaged&&!rest)
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

        Gizmos.DrawWireCube(groundCheckObj.transform.position, new Vector3(0.6f, 0.4f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.CompareTag("Damage")&&!isInvincible&&!isOnDamaged)
        {
            rigidBody.velocity = new Vector2(0, 0);
            if (isParrying)
            {
                SucceesParrying();
            }
            else
            PlayerDamage(collision);
        }
        if (collision.gameObject.CompareTag("Save"))
        {
            SavePoint save = collision.GetComponent<SavePoint>();
            if (!save.save)
            {
                save.Save();
                rest = true;
                transform.position = new Vector3(collision.transform.position.x, transform.position.y);
            }
        }
        if (collision.gameObject.CompareTag("ChangeCameraMax"))
        {
            CameraManager.instance.ChangeCameraMax(collision);
        }
    }

	public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal, float damageDrng)
	{
		base.OnDamage(damage, hitPosition, hitNormal, damageDrng);
        CameraManager.instance.ShakeCam(intensity, shakeTime);
        UIManager.instance.StatUpdate(true);
        isOnDamaged = true;
        OnDamageEffect(hitPosition);


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
            SoundManager.instance.SFXPlay(groundSound, transform.position, 0.5f);
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
        SoundManager.instance.SFXPlay(hitSound, transform.position);
        LivingEntity target = collision.transform.GetComponentInParent<LivingEntity>();
        if (target != null && !isOnDamaged)
        {
            OnDamage(target.damage, collision.transform.position, hitRay.normal, 1f);
        }
        else if(!isOnDamaged)
        {
            OnDamage(1f, collision.transform.position, hitRay.normal, 1f);
        }

    }
    public void EnemyDamage(Collider2D collision, float damage, float damageDrng)
    {
        float sound = 0;
        if (isAttack)
        {
            damage += 3;
            CameraManager.instance.ShakeCam(intensity, shakeTime);
            sound = 0.3f;
        }
        SoundManager.instance.SFXPlay(attackHeadSound, transform.position,0.5f+sound);
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

		for (int i = 0; i < energeyConsumption; i++)
		{
            UIManager.instance.EnergyUpdate(isDown);
            if (!isDown&&maxEnergy > energy)
            {
                energy += 1;
            }
            else if (isDown)
                energy -= 1;
        }


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
                UseEnergy(1, false);
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
        if (isParrying)
        {
            isParrying = false;
            Debug.Log("AAAA");
        }
    }
    private IEnumerator Parrying()
    {
        if (canParryied&&energy>=2)
        {
            UseEnergy(2, true);
            isParrying = true;
            float dir = transform.localScale.x;

            for (int i = 0; i < 36; i++)
		    {
                Debug.Log("A");
                transform.Rotate(new Vector3(0, 0, 10 * dir));
                yield return new WaitForSeconds(0.01f);
            }

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            isParrying = false;
        }

    }
    private void SucceesParrying()
    {
        Debug.Log("패링");
        SoundManager.instance.SFXPlay(parryingSound, transform.position, 0.07f);
        rigidBody.velocity = new Vector2(0, 0);
        rigidBody.velocity = new Vector2(0, 2) * 5;

        rigid.velocity = new Vector2(0, 0);
        rigid.velocity = new Vector2(0, 3) * 5;
        UseEnergy(4, false);
        StartCoroutine(ParryingEffect());

        ParryingParticle.Play();
        StartCoroutine(Invincible(0.5f,false));

        isParrying = false;
        canParryied = true;
        //무적 레이어
        gameObject.layer = 8;
    }
    private IEnumerator ParryingEffect()
    {
            float elapsedTime = 0f;
            Time.timeScale = 0f;
        CameraManager.instance.ShakeCam(5, 0.5f);
        while (elapsedTime < parryingDuration)
            {
                yield return 0;
                elapsedTime += Time.unscaledDeltaTime;
            }
            Time.timeScale = 1f;
    }
    private void fireParticleOff()
    {
        fireParticle.SetActive(false);
    }
    private void LoadStat()
    {
        if (PlayerPrefs.GetInt("maxEnergy") != 0)
        {
            maxEnergy = PlayerPrefs.GetInt("maxEnergy");
            initHealth = PlayerPrefs.GetInt("maxHealth");
            money = PlayerPrefs.GetInt("money");
        }
        else
        {
            maxEnergy = 10;
            initHealth = 5;
            money = 0;
        }
        hp = initHealth;

    }

}
