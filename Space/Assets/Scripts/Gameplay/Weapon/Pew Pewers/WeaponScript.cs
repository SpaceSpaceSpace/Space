using UnityEngine;
using System.Collections;

// Standard 'shoot projectile' weapon
// Also the base class for other derrived weapons
public class WeaponScript : MonoBehaviour
{
	///
	/// Public members to be assigned in the Inpector
	///
	public GameObject projectilePrefab;
	
	public float fireTime = 0.5f;
	public float maxSpreadAngle = 15.0f;
	
	///
	/// Protected (Private) members
	///
	protected bool m_active;
	protected bool m_canFire;
	
	protected Collider2D m_parentCollider;
	
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
		
		m_parentCollider = transform.root.GetComponent<Collider2D>();
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
		
		FireProjectile();
		
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
	
	protected void FireProjectile()
	{
		float angle = transform.eulerAngles.z + Random.Range( -maxSpreadAngle, maxSpreadAngle );
		GameObject projectile = (GameObject)Instantiate( projectilePrefab, 
														 transform.position, 
														 Quaternion.AngleAxis( angle, Vector3.forward ) );
		ProjectileScript projScript = projectile.GetComponent<ProjectileScript>();
		projScript.Init( m_parentCollider );
	}
	
	// Waits for the fireTime before setting canFire to true
	private IEnumerator FireDelay()
	{
		m_canFire = false;
		yield return new WaitForSeconds( fireTime );
		m_canFire = true;
	}
}
