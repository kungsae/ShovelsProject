using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBox : EnemyHealth
{
    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public override void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal, float damageDrng)
	{
		base.OnDamage(damage, hitPosition, hitNormal, damageDrng);
        DropCoin();
    }
}
