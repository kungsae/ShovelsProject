using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : LivingEntity
{
    private EnemyAi ai;
    [HideInInspector] public int hitCount = 0;
    public Action hitEvent;
    public Action hitEvent2;
    public GameObject damageText;

    public int haveCoinMin;
    public int haveCoinMax;
    public GameObject coinPrefab;

	// Start is called before the first frame update
	protected override void Awake()
    {
        base.Awake();
        ai = GetComponent<EnemyAi>();
    }

    // Update is called once per frame
    void Update()
    {
        
    } 

	public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal,float damageDrng)
	{
		base.OnDamage(damage, hitPosition, hitNormal, damageDrng);
        hitCount++;
        if (hitEvent != null)
        {
            if(hitCount % 3 == 0)
            hitEvent();
            else if(hitCount % 7 == 0)
			{
                hitEvent2();
			}
        }
        GameObject text = Instantiate(damageText, transform.position, Quaternion.identity);
        text.GetComponent<DamageText>().text.text = damage.ToString();

    }
    public void DropCoin()
    {
        int dropCoin = UnityEngine.Random.Range(haveCoinMin, haveCoinMax);
		for (int i = 0; i < dropCoin; i++)
		{
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }
	public override void Die()
	{
		base.Die();
        DropCoin();
        ai.SetDead();
    }
}
