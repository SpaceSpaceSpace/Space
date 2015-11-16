using UnityEngine;
using System.Collections.Generic;

[ RequireComponent (typeof ( Rigidbody2D ) ) ]
[ RequireComponent ( typeof ( ThrustScript ) ) ]

// I'm sailing away...
public class ShipScript : MonoBehaviour
{
	public float accelForce = 50.0f; // the accel force for thrust
	public float turnForce = 10.0f; // the turn force for thrust
	public float maxMoveSpeed = 5.0f; // max move speed for thrust

	protected float m_health = 100.0f;
	protected float m_maxHealth = 100.0f;

	protected HitParticleSpawner m_hitParticles;
	protected ThrustScript m_thrust;
	protected WeaponSlot[] m_weaponSlots;
	protected ShieldScript m_shield;

	protected static GameObject m_exploder;

	public WeaponSlot[] WeaponSlots
	{
		get { return m_weaponSlots; }
	}

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
		if(m_exploder == null)
			m_exploder = Resources.Load("ShipPrefabs/ShipExplosion") as GameObject;
		m_thrust = GetComponent<ThrustScript>();
		m_hitParticles = GetComponentInChildren<HitParticleSpawner>();

		InitWeapons();

		Transform shieldTrans = transform.FindChild( "Shield" );
		if( shieldTrans != null )
		{
			m_shield = shieldTrans.GetComponent<ShieldScript>();
		}
	}

	// Checks which weapons are attached and loads them into m_weapons
	protected void InitWeapons()
	{
		Transform weaponSlots = transform.FindChild( "Weapons" );
		if( weaponSlots != null )
		{
			WeaponSlot[] weapons = new WeaponSlot[ weaponSlots.childCount ];
			for( int i = 0; i < weapons.Length; i++ )
			{
				weapons[ i ] = weaponSlots.GetChild( i ).GetComponent<WeaponSlot>();
			}
			
			m_weaponSlots = weapons;
		}
		else
		{
			// Just in case there are no weapons
			m_weaponSlots = new WeaponSlot[0];
		}
	}

	protected void FireWeapons()
	{
		for( int i = 0; i < m_weaponSlots.Length; i++ )
		{			
			m_weaponSlots[ i ].Fire();
		}
	}
	
	protected void ReleaseFire()
	{
		for( int i = 0; i < m_weaponSlots.Length; i++ )
		{
			m_weaponSlots[ i ].OnRelease();
		}
	}
	
	protected void HandleCollision( Collision2D collision )
	{
		if (collision.collider.tag == "SectorBounds") 
			return;
		
		ApplyDamage( collision.relativeVelocity.magnitude * collision.rigidbody.mass * 0.02f );
	}

	protected virtual void Die()
	{
		Instantiate(m_exploder, transform.position, Quaternion.identity);
		Destroy( gameObject );
	}
}
