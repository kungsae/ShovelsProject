using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float damage;
    public float initHealth;
    public bool canDamage = true;
    public SpriteRenderer sprite;
    public Rigidbody2D rigid;
    public float hp; /*{ get; protected set; }*/
    public bool dead { get; protected set; }
    public event Action OnDeath;
	protected virtual void Awake()
	{
        hp = initHealth;
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }
	public virtual void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal,float damageDrng)
	{
        if (canDamage)
        {
            hp -= damage* damageDrng;
            //Debug.Log( gameObject.name+ ": ���� ������ : " + damage * damageDrng + " ���� ü�� : " + hp);
            StartCoroutine(DamageDelay());
        }
        if (hp <= 0 && !dead)
        {
            int dir = transform.position.x - hitPosition.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(dir, 1) * 2, ForceMode2D.Impulse);
            Die();
            Debug.Log("���");
        }
    }
    IEnumerator DamageDelay()
    {
        sprite.color = Color.red;
        canDamage = false;
        yield return new WaitForSeconds(0.15f);
        canDamage = true;
        sprite.color = Color.white;
    }
    public virtual void Die()
    {
        if (OnDeath != null) OnDeath();
        StartCoroutine(WaitForIt());
    }

    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(0.0001f);
        dead = true;
    }
    
}
