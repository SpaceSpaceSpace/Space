using UnityEngine;

// 'Hold to charge, release to fire' weapon
public class ChargeableWeaponScript : WeaponScript
{
	private float m_chargeTime;
	
	public override void Fire()
	{
		m_chargeTime += Time.deltaTime;
			
		// Printing for lack of visual feedback atm
		//print( "Charging - " + ( ( m_chargeTime / fireTime ) * 100 ) + "%" );
	}
	
	public override void OnRelease()
	{
		//if( m_chargeTime >= fireTime )
		{
			//FireProjectile();
		}
		
		m_chargeTime = 0f;
	}
}
