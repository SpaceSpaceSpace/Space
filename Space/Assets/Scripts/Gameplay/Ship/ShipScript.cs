using UnityEngine;
using System.Collections.Generic;

[ RequireComponent (typeof ( Rigidbody2D ) ) ]
[ RequireComponent ( typeof ( ThrustScript ) ) ]

// I'm sailing away...
public class ShipScript : MonoBehaviour
{
	protected float m_health = 100.0f;
	protected float m_maxHealth = 100.0f;

	protected HitParticleSpawner m_hitParticles;
	protected ThrustScript m_thrust;
	protected WeaponScript[] m_weapons;
	protected ShieldScript m_shield;

	// Primarily handles "collisions" with projeciles 
	public void TakeHit( Vector2 force, Vector2 hitPoint )
	{
		m_hitParticles.ReactToHit(hitPoint);
		m_thrust.AppyImpulse( force, hitPoint );
	}

	public virtual void ApplyDamage( float damage, float shieldPen = 0.0f )
	{
		float damageToShip = damage * shieldPen;
		if( m_shield != null )
		{
			// ShieldScript.ApplyDamage returns any leftover damage
			damageToShip += m_shield.ApplyDamage( damage - damageToShip );
		}
		else
		{
			damageToShip = damage;
		}

		m_health -= damageToShip;

		if( m_health <= 0 )
		{
			Die();
		}
	}
	
	// Basically the Start method of the script,
	// since the Start of a base class script will not be called
	protected void InitShip()
	{
		m_thrust = GetComponent<ThrustScript>();
		m_hitParticles = GetComponentInChildren<HitParticleSpawner>();

		InitWeapons();

		// Temporary
		Transform shieldTrans = transform.FindChild( "Shield" );
		if( shieldTrans != null )
		{
			m_shield = shieldTrans.GetComponent<ShieldScript>();
		}
	}

	// Checks which weapons are attached and loads them into m_weapons
	protected void InitWeapons()
	{
		//Loop through children and see which ones are weapons
		List<WeaponScript> tempWeapons = new List<WeaponScript>();

		for( int i = 0; i < transform.childCount; i++ )
		{
			WeaponScript weapon = transform.GetChild( i ).GetComponent<WeaponScript>();

			if(weapon != null)
				tempWeapons.Add(weapon);			
		}

		m_weapons = tempWeapons.ToArray();
	}

	protected void FireWeapons()
	{
		for( int i = 0; i < m_weapons.Length; i++ )
		{			
			m_weapons[ i ].Fire();
		}
	}
	
	protected void ReleaseFire()
	{
		for( int i = 0; i < m_weapons.Length; i++ )
		{
			m_weapons[ i ].OnRelease();
		}
	}
	
	protected void HandleCollision( Collision2D collision )
	{
		m_hitParticles.ReactToHit(collision.transform.position);

		ApplyDamage( collision.relativeVelocity.magnitude * collision.rigidbody.mass * 0.02f );
	}

	protected virtual void Die()
	{
		Destroy( gameObject );
	}
}
