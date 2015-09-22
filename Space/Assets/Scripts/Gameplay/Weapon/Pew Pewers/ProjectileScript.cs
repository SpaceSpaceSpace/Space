using UnityEngine;

// Basic projectile script
public class ProjectileScript : MonoBehaviour
{
	// Pubbies
	public float speed = 10.0f;
	public float lifeTime = 1.0f;
	public float knockback = 1.0f;
	public bool stayAlive = false;
	
	// In case we want the player to be hit by their own projectiles
	/* private bool m_detectOwnCollider = false;
	private float m_detectOwnDelay = 0.1f;
	private Collider2D m_firingCollider; */
	
	public void Init( Collider2D firingCollider )
	{
		//m_firingCollider = firingCollider;
		Physics2D.IgnoreCollision( firingCollider, GetComponent<Collider2D>(), true );
	}
	
	void Update ()
	{
		// Using translate because using rigidbodies for movement caused jittering
		transform.Translate( Vector3.up * speed * Time.deltaTime );
		
		lifeTime -= Time.deltaTime;
		
		if( lifeTime <= 0 && !stayAlive)
		{
			// Ideally we'll use object pooling rather than spawn/destroy
			// But that comes later
			Destroy( gameObject );
		}
		
		/*if( !m_detectOwnCollider )
		{
			m_detectOwnDelay -= Time.deltaTime;
			
			if( m_detectOwnDelay <= 0 )
			{
				Physics2D.IgnoreCollision( m_firingCollider, GetComponent<Collider2D>(), false );
			}
		}*/
	}
	
	void OnTriggerEnter2D( Collider2D col )
	{	
		if( col.tag == "Ship" )
		{
			Vector2 colPos = col.transform.position;
			Vector2 offset = (Vector2)transform.position - colPos;
			ShipScript ship = col.GetComponent<ShipScript>();
			ship.TakeHit( transform.up * speed * knockback, offset + colPos );
		}
		
		Destroy( gameObject );
	}
}
