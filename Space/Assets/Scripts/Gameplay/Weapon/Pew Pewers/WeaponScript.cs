using UnityEngine;
using System.Collections;

// Standard 'shoot projectile' weapon
// Also the base class for other derrived weapons
public abstract class WeaponScript : MonoBehaviour
{
	public enum WeaponType
	{
		SHOOTY_KILLY,
		SCATTER_SHOT,
		MISSLE_LAUNCHER,
		MINE_LAUNCHER,
		BEAM,
		NUM_WEAPON_TYPES
	}

	///
	/// Public members to be assigned in the Inpector
	///
	public GameObject projectilePrefab;
	public Transform fireFromPoint;

	public string fireSoundName = "Laser_Bolt";

	public float damage = 10.0f;

	public WeaponType weaponType = WeaponType.SHOOTY_KILLY;
	public WeaponModifier.ModifierNames modifier = WeaponModifier.ModifierNames.DEFAULT;

	///
	/// Protected (Private) members
	///
	protected SoundSystemScript m_soundSystem;
	protected bool m_active = true;

	///
	/// Public Methods
	///
	public abstract void Fire();
	public abstract void OnRelease();

	public abstract WeaponInfo ToInfo();

	public virtual void ToggleActive()
	{
		m_active = !m_active;
	}
	
	protected void Init()
	{
		m_soundSystem = GetComponent<SoundSystemScript>();
	}

	protected abstract void ApplyModifier();
}
