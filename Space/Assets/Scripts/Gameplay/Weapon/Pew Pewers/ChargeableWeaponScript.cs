using UnityEngine;

// 'Hold to charge, release to fire' weapon
public class ChargeableWeaponScript : WeaponScript
{
	private float m_chargeTime;
	
	public override void Fire()
	{
		if( !m_active )
		{
			// Early return
			return;
		}
		
		m_chargeTime += Time.deltaTime;
			
		// Printing for lack of visual feedback atm
		print( "Charging - " + ( ( m_chargeTime / fireTime ) * 100 ) + "%" );
	}
	
	public override void OnRelease()
	{
		if( m_chargeTime >= fireTime )
		{
			float angle = transform.eulerAngles.z + Random.Range( -maxSpreadAngle, maxSpreadAngle );
			Instantiate( projectilePrefab, transform.position, Quaternion.AngleAxis( angle, Vector3.forward ) );
		}
		
		m_chargeTime = 0f;
	}
	
	public override void ToggleActive()
	{
		base.ToggleActive();
		m_chargeTime = 0f;
	}
}
