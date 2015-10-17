using UnityEngine;

// 'Imma firin' mah layzah' weapon
public class BeamWeaponScript : WeaponScript
{
	public GameObject beamPrefab;
	public float beamRange = 15.0f;
	public float damage = 10;
	public float rateOfDamage = 1.0f;
	public float beamWidth = 0.01f;
	
	private float m_beamSpriteSize;
	private GameObject m_beam;
	 
	void Start ()
	{
		m_soundSystem = GetComponent<SoundSystemScript>();
		m_beam = (GameObject)Instantiate( beamPrefab, transform.position, Quaternion.identity );
		m_beam.transform.parent = transform;
		m_beam.SetActive( false );
		
		m_beamSpriteSize = m_beam.GetComponent<Renderer>().bounds.size.x;
	}
	
	public override void Fire()
	{
		if( !m_soundSystem.IsPlaying() )
		{
			m_soundSystem.PlayLooping( fireSoundName );
		}
		
		// Doing a raycast jobbie
		RaycastHit2D hit = Physics2D.Raycast( transform.position, transform.up, beamRange );
		
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
	
	private void HandleHit( RaycastHit2D hit )
	{
		GameObject go = hit.collider.gameObject;
		Vector2 dir = ( hit.point - (Vector2)transform.position ).normalized;
		if( go.tag == "Ship" )
		{
			ShipScript ship = go.GetComponent<ShipScript>();
			//Vector2 dir = ( hit.point - (Vector2)transform.position ).normalized;
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
