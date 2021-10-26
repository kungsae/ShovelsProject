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
    public float hp; /*{ get; protected set; }*/
    public bool dead { get; protected set; }
    public event Action OnDeath;
	protected virtual void Awake()
	{
        hp = initHealth;
        sprite = GetComponent<SpriteRenderer>();
    }
	public virtual void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal,float damageDrng)
	{
        if (canDamage)
        {
            hp -= damage* damageDrng;
            //Debug.Log( gameObject.name+ ": 받은 데미지 : " + damage * damageDrng + " 현재 체력 : " + hp);
            StartCoroutine(DamageDelay());
        }
        if (hp <= 0 && !dead)
        {
            Die();
            Debug.Log("사망");
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
        dead = true;
    }
}
