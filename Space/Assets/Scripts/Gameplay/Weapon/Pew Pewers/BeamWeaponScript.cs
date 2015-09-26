using UnityEngine;

// 'Imma firin' mah layzah' weapon
public class BeamWeaponScript : WeaponScript
{
	public float beamRange = 15.0f;
	public float damage = 10;
	public float rateOfDamage = 1.0f;
	public float beamWidth = 0.01f;
	
	private float m_beamSpriteSize;
	private GameObject m_beam;
	 
	void Start ()
	{
		m_beam = (GameObject)Instantiate( projectilePrefab.gameObject, transform.position, Quaternion.identity );
		m_beam.transform.parent = transform;
		m_beam.SetActive( false );
		
		m_beamSpriteSize = m_beam.GetComponent<Renderer>().bounds.size.x;
	}
	
	public override void Fire()
	{
		if( !m_active )
		{
			// Early return
			return;
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
		m_beam.SetActive( false );
	}
	
	public override void ToggleActive()
	{
		base.ToggleActive();
		if( !m_active )
		{
			m_beam.SetActive( false );
		}
	}
	
	private void HandleHit( RaycastHit2D hit )
	{
		GameObject go = hit.collider.gameObject;
		if( go.tag == "Ship" )
		{
			ShipScript ship = go.GetComponent<ShipScript>();
			Vector2 dir = ( hit.point - (Vector2)transform.position ).normalized;
			ship.TakeHit( dir, hit.point );
			
			ship.ApplyDamage( damage * Time.deltaTime );
		}
	}
}
