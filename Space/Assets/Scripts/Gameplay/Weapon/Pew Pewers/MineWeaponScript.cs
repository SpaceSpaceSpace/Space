using UnityEngine;
using System.Collections;

public class MineWeaponScript : WeaponScript
{
	public float fireTime = 1.0f;
	public float mineLifeTime = 60.0f;
	public float projectileSpeed = 10.0f;

	private MineProjectileScript m_mineProj;
	private bool m_canFire = true;

	void Start ()
	{
		m_mineProj = projectilePrefab.GetComponent<MineProjectileScript>();
		Init();
	}

	public override void Fire()
	{
		if( !m_active || !m_canFire )
		{
			// Early return
			return;
		}
		
		FireMine();
		
		StartCoroutine( FireDelay() );
	}

	public override void OnRelease()
	{
	}

	public override WeaponInfo ToInfo()
	{
		WeaponInfo info = new WeaponInfo( weaponType, modifier );
		info.AddAttribute( "Damage", RoundStatToDecimalPlaces( damage, 1 ).ToString() );
		info.AddAttribute( "Fire Rate", RoundStatToDecimalPlaces( 1 / fireTime, 1 ) + "/s" );
		info.AddAttribute( "Projectile Speed", RoundStatToDecimalPlaces( projectileSpeed, 1 ).ToString() );
		return info;
	}

	public override WeaponInfo ToInfo( WeaponModifier.ModifierNames mod )
	{
		float moddedDamage = damage * WeaponModifier.GetModifierValue( mod, WeaponModifier.Stats.DAMAGE );
		float moddedSpeed = projectileSpeed * WeaponModifier.GetModifierValue( mod, WeaponModifier.Stats.MINE_SPEED );
		float moddedFireRate = fireTime / WeaponModifier.GetModifierValue( mod, WeaponModifier.Stats.FIRE_RATE );

		WeaponInfo info = new WeaponInfo( weaponType, mod );
		info.AddAttribute( "Damage", RoundStatToDecimalPlaces( moddedDamage, 1 ).ToString() );
		info.AddAttribute( "Fire Rate", RoundStatToDecimalPlaces(  1 / moddedFireRate, 1 ) + "/s" );
		info.AddAttribute( "Projectile Speed", RoundStatToDecimalPlaces( moddedSpeed, 1 ).ToString() );
		return info;
	}

	protected override void ApplyModifier()
	{
		if( modifier == WeaponModifier.ModifierNames.DEFAULT )
		{
			// Early return
			return;
		}
		
		damage *= WeaponModifier.GetModifierValue( modifier, WeaponModifier.Stats.DAMAGE );
		projectileSpeed *= WeaponModifier.GetModifierValue( modifier, WeaponModifier.Stats.MINE_SPEED );
		fireTime /= WeaponModifier.GetModifierValue( modifier, WeaponModifier.Stats.FIRE_RATE );
	}

	private void FireMine()
	{	
		MineProjectileScript projectile = (MineProjectileScript)Instantiate( m_mineProj, 
		                                                					 transform.position, 
		                                               						 Quaternion.identity );
		projectile.Init( damage, projectileSpeed, mineLifeTime, transform.root.gameObject );
		projectile.FireProj( transform.eulerAngles.z );
		
		m_soundSystem.PlayOneShot( fireSoundName );
	}
	
	private IEnumerator FireDelay()
	{
		m_canFire = false;
		yield return new WaitForSeconds( fireTime );
		m_canFire = true;
	}
}
