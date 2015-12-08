using UnityEngine;
using System.Collections;

public class ProjectileWeaponScript : WeaponScript
{
	public float fireTime = 0.5f;
	public float maxSpreadAngle = 15.0f;
	public float projectileSpeed = 10.0f;
	public float projectileLifeTime = 2.0f;
	public int projectilesPerShot = 1;
	
	private GameObject m_projectileContainer;
	private ProjectileScript[] m_projectilePool;
	private int m_currProjectile;
	private int m_numProjectiles;
	private bool m_canFire;
	
	void Start ()
	{
		m_canFire = true;
		m_currProjectile = 0;
		Init();
		SpawnProjectiles();
	}
	
	void OnDestroy()
	{
		Destroy( m_projectileContainer );
	}
	
	public override void Fire()
	{
		if( m_active && m_canFire )
		{
			for( int i = 0; i < projectilesPerShot; i++ )
			{
				FireProjectile();
			}
			StartCoroutine( FireDelay() );
		}
	}
	
	public override void OnRelease()
	{
		// Do some stuff
	}

	public override WeaponInfo ToInfo()
	{
		WeaponInfo info = new WeaponInfo( weaponType, modifier );

		string dmgString = RoundStatToDecimalPlaces( damage, 3 ).ToString();

		if( weaponType == WeaponType.SCATTER_SHOT )
		{
			dmgString += " (x" + projectilesPerShot + ")";
		}

		info.AddAttribute( "Damage", dmgString );
		info.AddAttribute( "Fire Rate", RoundStatToDecimalPlaces( 1 / fireTime, 2 ) + "/s" );
		info.AddAttribute( "Projectile Speed", RoundStatToDecimalPlaces( projectileSpeed, 1 ).ToString() );
		info.AddAttribute( "Accuracy", RoundStatToDecimalPlaces( ( 1 - ( maxSpreadAngle * 2 / 180 ) ) * 100, 1 ) + "%" );

		return info;
	}

	public override WeaponInfo ToInfo( WeaponModifier.ModifierNames mod )
	{
		float moddedDamage = damage * WeaponModifier.GetModifierValue( mod, WeaponModifier.Stats.DAMAGE );
		float moddedFireRate = fireTime / WeaponModifier.GetModifierValue( mod, WeaponModifier.Stats.FIRE_RATE );
		float moddedAccuracy = maxSpreadAngle / WeaponModifier.GetModifierValue( mod, WeaponModifier.Stats.ACCURACY );

		WeaponInfo info = new WeaponInfo( weaponType, mod );

		if( weaponType != WeaponType.SCATTER_SHOT )
		{
			info.AddAttribute( "Damage", RoundStatToDecimalPlaces( moddedDamage, 1 ).ToString() );
		}
		else
		{
			float bonusProjectiles = WeaponModifier.GetModifierValue( mod, WeaponModifier.Stats.BONUS_PROJECTILES );
			int count = Mathf.CeilToInt( projectilesPerShot * bonusProjectiles );
			info.AddAttribute( "Damage", RoundStatToDecimalPlaces( damage, 1 ) + " (x" + count + ")" );
		}

		info.AddAttribute( "Fire Rate", RoundStatToDecimalPlaces( 1 / moddedFireRate, 1 ) + "/s" );
		info.AddAttribute( "Projectile Speed", RoundStatToDecimalPlaces( projectileSpeed, 1 ).ToString() );
		info.AddAttribute( "Accuracy", RoundStatToDecimalPlaces( ( 1 - ( moddedAccuracy * 2 / 180 ) ) * 100, 1 ) + "%" );
		
		return info;
	}

	protected override void ApplyModifier()
	{
		if( modifier == WeaponModifier.ModifierNames.DEFAULT )
		{
			// Early return
			return;
		}

		if( weaponType == WeaponType.SCATTER_SHOT )
		{
			float bonusProjectiles = WeaponModifier.GetModifierValue( modifier, WeaponModifier.Stats.BONUS_PROJECTILES );
			projectilesPerShot = Mathf.CeilToInt( projectilesPerShot * bonusProjectiles);
		}
		else
		{
			damage *= WeaponModifier.GetModifierValue( modifier, WeaponModifier.Stats.DAMAGE );
		}

		maxSpreadAngle /= WeaponModifier.GetModifierValue( modifier, WeaponModifier.Stats.ACCURACY );
		fireTime /= WeaponModifier.GetModifierValue( modifier, WeaponModifier.Stats.FIRE_RATE );
	}
	
	private void FireProjectile()
	{
		float angle = transform.eulerAngles.z + Random.Range( -maxSpreadAngle, maxSpreadAngle );
		
		ProjectileScript proj = m_projectilePool[ m_currProjectile ];
		proj.transform.position = fireFromPoint.position;
		proj.FireProj( angle );
		m_currProjectile = ( m_currProjectile + 1 ) % m_numProjectiles;
		
		m_soundSystem.PlayOneShot( fireSoundName );
	}
	
	// Waits for the fireTime before setting canFire to true
	private IEnumerator FireDelay()
	{
		m_canFire = false;
		yield return new WaitForSeconds( fireTime );
		m_canFire = true;
	}
	
	private void SpawnProjectiles()
	{
		// Init the collection
		int numProjectiles = Mathf.CeilToInt( projectileLifeTime / fireTime ) * projectilesPerShot;
		m_numProjectiles = numProjectiles;
		m_projectilePool = new ProjectileScript[ numProjectiles ];
		
		// Create a container GO for the projectiles
		m_projectileContainer = new GameObject( gameObject.name + " Projectiles" );

		// Spawn the Projectiles
		for( int i = 0; i < numProjectiles; i++ )
		{
			GameObject projectile = (GameObject)Instantiate( projectilePrefab, transform.position, Quaternion.identity );
			ProjectileScript projScript = projectile.GetComponent<ProjectileScript>();
			projScript.transform.parent = m_projectileContainer.transform;
			projScript.Init( damage, projectileSpeed, projectileLifeTime, GetComponentInParent<ShipScript>().transform.gameObject );
			m_projectilePool[ i ] = projScript;
			projectile.SetActive( false );
		}
	}
}
