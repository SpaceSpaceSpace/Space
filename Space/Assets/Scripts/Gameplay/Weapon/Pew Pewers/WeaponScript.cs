using UnityEngine;
using System.Collections;

// Standard 'shoot projectile' weapon
// Also the base class for other derrived weapons
public abstract class WeaponScript : MonoBehaviour
{
	public enum WeaponType
	{
		LASER_MACHINE_GUN,
		SNIPER,
		SCATTER_SHOT,
		BEAM,
		MISSILE_LAUNCHER,
		MINE_LAUNCHER,
		NUM_WEAPONS
	}

	///
	/// Public members to be assigned in the Inpector
	///
	public GameObject projectilePrefab;
	public Transform fireFromPoint;

	public string fireSoundName = "Laser_Bolt";

	public float damage = 10.0f;
	public float cost = 25.0f;

	public WeaponType weaponType = WeaponType.LASER_MACHINE_GUN;
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
	public abstract WeaponInfo ToInfo( WeaponModifier.ModifierNames mod );

	public virtual void ToggleActive()
	{
		m_active = !m_active;
	}

	public void SetModifier( WeaponModifier.ModifierNames mod )
	{
		modifier = mod;
		//ApplyModifier();
	}
	
	protected void Init()
	{
		ApplyModifier();
		m_soundSystem = GetComponent<SoundSystemScript>();
	}

	protected abstract void ApplyModifier();

	protected float RoundStatToDecimalPlaces( float statValue, int sigFigs )
	{
		return (float)System.Math.Round( statValue, sigFigs );
	}
	protected void SetCost(float c)
	{
		cost = c;
	}

}
