using UnityEngine;

// 'Imma firin' mah layzah' weapon
public class BeamWeaponScript : WeaponScript
{
	public float beamRange = 15.0f;

	private LayerMask m_layerMask;
	private float m_beamSpriteSize;
	private GameObject m_beam;
	private Collider2D m_ownCollider;
	 
	void Start ()
	{
		Init();
		m_beam = (GameObject)Instantiate( projectilePrefab, fireFromPoint.position, Quaternion.identity );
		m_beam.transform.parent = transform;
		m_beam.SetActive( false );
	
		SpriteRenderer beamSR = m_beam.GetComponent<SpriteRenderer>();
		beamSR.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
		
		m_beamSpriteSize = m_beam.GetComponent<Renderer>().bounds.size.x;

		m_layerMask = ~( 1 << LayerMask.NameToLayer( "Projectiles" ) );

		m_ownCollider = transform.parent.GetComponent<Collider2D>();
	}
	
	public override void Fire()
	{
		if( !m_active )
		{
			// Early return
			return;
		}

		if( !m_soundSystem.IsPlaying() )
		{
			m_soundSystem.PlayLooping( fireSoundName );
		}
		
		// Doing a raycast jobbie
		RaycastHit2D[] hits = Physics2D.RaycastAll( fireFromPoint.position, transform.up, beamRange, m_layerMask );
		//RaycastHit2D hit = Physics2D.Raycast( fireFromPoint.position, transform.up, beamRange, m_layerMask );
		
		float distance = beamRange;

		for( int i = 0; i < hits.Length; i++ )
		{
			if( hits[ i ].collider != m_ownCollider )
			{
				// If the raycast hit something, set the distance to the distance to that something
				distance = Vector2.Distance( hits[ i ].point, (Vector2)fireFromPoint.position );
				
				HandleHit( hits[ i ] );
				break;
			}
		}
		
		// Scale the beam to the distance
		m_beam.transform.localScale = new Vector3( 1, distance / m_beamSpriteSize, 1 );
		m_beam.SetActive( true ); // And show the beam too, I guess
	}
	
	public override void OnRelease()
	{
		m_soundSystem.StopPlaying();
		m_beam.SetActive( false );
	}

	public override WeaponInfo ToInfo()
	{
		WeaponInfo info = new WeaponInfo( weaponType, modifier );
		info.AddAttribute( "Damage", damage );
		info.AddAttribute( "Range", beamRange );
		return info;
	}

	public override WeaponInfo ToInfo( WeaponModifier.ModifierNames mod )
	{
		float moddedDamage = damage * WeaponModifier.GetModifierValue( modifier, WeaponModifier.Stats.DAMAGE );
		float moddedRange = beamRange * WeaponModifier.GetModifierValue( modifier, WeaponModifier.Stats.BEAM_RANGE );
		WeaponInfo info = new WeaponInfo( weaponType, modifier );
		info.AddAttribute( "Damage", moddedDamage );
		info.AddAttribute( "Range", moddedRange );
		return info;
	}

	public override void ToggleActive()
	{
		base.ToggleActive();
		if( !m_active )
		{
			m_beam.SetActive( false );
			if( m_soundSystem.IsPlaying() )
			{
				m_soundSystem.StopPlaying();
			}
		}
	}

	protected override void ApplyModifier()
	{
		if( modifier == WeaponModifier.ModifierNames.DEFAULT )
		{
			// Early return
			return;
		}

		damage *= WeaponModifier.GetModifierValue( modifier, WeaponModifier.Stats.DAMAGE );
		beamRange *= WeaponModifier.GetModifierValue( modifier, WeaponModifier.Stats.BEAM_RANGE );
	}
	
	private void HandleHit( RaycastHit2D hit )
	{
		GameObject go = hit.collider.gameObject;
		Vector2 dir = ( hit.point - (Vector2)transform.position ).normalized;
		if( go.tag == "Ship" )
		{
			ShipScript ship = go.GetComponent<ShipScript>();
			ship.TakeHit( dir, hit.point );
			
			ship.ApplyDamage( damage * Time.deltaTime );
		}
		else if( go.tag == "Asteroid" || go.tag == "Satellite" || go.tag == "sAsteroid" )
		{
			// Do same sort of thing as with Ship
			Satellite sat = go.GetComponent<Satellite>();
			sat.ApplyDamage(damage, dir, hit.point, true);
		}
	}
}
