using UnityEngine;
using System.Collections;

// Standard 'shoot projectile' weapon
// Also the base class for other derrived weapons
public class WeaponScript : MonoBehaviour
{
	///
	/// Public members to be assigned in the Inpector
	///
	public ProjectileScript projectilePrefab;
	
	public float projectileSpeed
	{
		get{return projectilePrefab.speed;}
		set{projectilePrefab.speed = value;}
	}
	public float projectileLifeTime
	{
		get{return projectilePrefab.lifeTime;}
		set{projectilePrefab.lifeTime = value;}
	}
	public float attackPower;
	public float shieldPiercing;
	public float cooldown;
	public float shotsBeforeCooldown;
	public float projectilesPerShot;
	public float shotsPerClip;
	public float maxReserveClips;
	public float knockback;
	public float fireTime = 0.5f;
	public float maxSpreadAngle = 15.0f;
	
	///
	/// Protected (Private) members
	///
	protected bool m_active;
	protected bool m_canFire;
	
	///
	/// Properties for access to private members
	///
	public bool Active
	{
		get { return m_active; }
		set { m_active = value; }
	}
	
	///
	/// Monobehavior Methods
	///
	void Start ()
	{
		m_active = false;
		m_canFire = true;
	}
	
	///
	/// Public Methods
	///
	public virtual void Fire()
	{
		if( !m_active || !m_canFire )
		{
			// Early return
			return;
		}
		
		float angle = transform.eulerAngles.z + Random.Range( -maxSpreadAngle, maxSpreadAngle );
		Instantiate( projectilePrefab, transform.position, Quaternion.AngleAxis( angle, Vector3.forward ) );
		
		StartCoroutine( FireDelay() );
	}
	
	// Called when a weapon has stopped firing
	// Could be used for visual or sound effects
	// Or to make Chargable Weapons work...
	public virtual void OnRelease()
	{
		if( !m_active )
		{
			// Early return
			return;
		}
		
		// Do stuff
	}
	
	// Toggles the weapon's active state
	// Mainly used in PlayerShipScript.SetActiveWeapons
	public virtual void ToggleActive()
	{
		m_active = !m_active;
	}
	
	///
	/// Private Methods
	///
	
	// Waits for the fireTime before setting canFire to true
	private IEnumerator FireDelay()
	{
		m_canFire = false;
		yield return new WaitForSeconds( fireTime );
		m_canFire = true;
	}
}
