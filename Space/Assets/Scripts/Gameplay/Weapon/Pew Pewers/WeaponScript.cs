using UnityEngine;
using System.Collections;

// Standard 'shoot projectile' weapon
// Also the base class for other derrived weapons
public abstract class WeaponScript : MonoBehaviour
{
	///
	/// Public members to be assigned in the Inpector
	///
	public float fireTime = 0.5f;
	public float maxSpreadAngle = 15.0f;
	public string fireSoundName = "Laser_Bolt";
	
	///
	/// Protected (Private) members
	///
	protected Collider2D m_parentCollider;
	protected SoundSystemScript m_soundSystem;

	void Init()
	{	
		m_parentCollider = transform.root.GetComponent<Collider2D>();
		m_soundSystem = GetComponent<SoundSystemScript>();
	}
	
	///
	/// Public Methods
	///
	public abstract void Fire();
	public abstract void OnRelease();
}
