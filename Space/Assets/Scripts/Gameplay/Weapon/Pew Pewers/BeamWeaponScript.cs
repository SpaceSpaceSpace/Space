using UnityEngine;

// 'Imma firin' mah layzah' weapon
public class BeamWeaponScript : WeaponScript
{
	public float beamRange = 15.0f;

	private LayerMask m_layerMask;
	private float m_beamSpriteSize;
	private GameObject m_beam;
	 
	void Start ()
	{
		m_soundSystem = GetComponent<SoundSystemScript>();
		m_beam = (GameObject)Instantiate( projectilePrefab, transform.position, Quaternion.identity );
		m_beam.transform.parent = transform;
		m_beam.SetActive( false );
	
		SpriteRenderer beamSR = m_beam.GetComponent<SpriteRenderer>();
		beamSR.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
		
		m_beamSpriteSize = m_beam.GetComponent<Renderer>().bounds.size.x;

		m_layerMask = LayerMask.NameToLayer( "Projectiles" );
	}
	
	public override void Fire()
	{
		if( !m_soundSystem.IsPlaying() )
		{
			m_soundSystem.PlayLooping( fireSoundName );
		}
		
		// Doing a raycast jobbie
		RaycastHit2D hit = Physics2D.Raycast( transform.position, transform.up, beamRange, m_layerMask );
		
		float distance = beamRange;
		
		if( hit.collider != null )
		{
			// If the raycast hit something, set the distance to the distance to that something
			distance = Vector2.Distance( hit.point, (Vector2)transform.position );
			
			HandleHit( hit );
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
		else if( go.tag == "Asteroid" )
		{
			// Do same sort of thing as with Ship
			Satellite sat = go.GetComponent<Satellite>();
			sat.ApplyDamage(damage, dir);
		}
	}
}
