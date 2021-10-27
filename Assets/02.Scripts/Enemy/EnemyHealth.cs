using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : LivingEntity
{
    private EnemyAi ai;
    [HideInInspector] public int hitCount = 0;
    [SerializeField] private int maxHitCount = 5;
    public Action hitEvent;
    public GameObject damageText;

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
        if (maxHitCount <= hitCount && hitEvent != null)
        {
            hitEvent();
            hitCount = 0;
        }
        GameObject text = Instantiate(damageText, transform.position, Quaternion.identity);
        text.GetComponent<DamageText>().text.text = damage.ToString();
        //데미지 수치 뜨는거 만들예정

    }
	public override void Die()
	{
		base.Die();
        ai.SetDead();
    }
}
