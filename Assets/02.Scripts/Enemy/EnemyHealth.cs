using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : LivingEntity
{
    private EnemyAi ai;

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
        
        //데미지 수치 뜨는거 만들예정
	}
	public override void Die()
	{
		base.Die();
        ai.SetDead();
    }
}
