using UnityEngine;

// Basic projectile script
public class ProjectileScript : MonoBehaviour
{
	private Collider2D m_firingCollider;
	private Collider2D m_collider;
	private Rigidbody2D m_rigidbody;
	private Rigidbody2D m_firingRb;

	protected float m_speed = 10.0f;
	protected float m_damage = 10.0f;
	protected float m_lifeTime = 1.0f;
	protected float m_currLifeTime = 0;
	protected float m_shieldPenetration = 0.0f;

	public void Init( float damage, float speed, float lifeTime, GameObject firingShip )
	{
		m_damage = damage;
		m_speed = speed;
		m_lifeTime = lifeTime;
		m_currLifeTime = 0;

		m_firingCollider = firingShip.GetComponent<Collider2D>();
		m_collider = GetComponent<Collider2D>();
		m_firingRb = firingShip.GetComponent<Rigidbody2D>();
		m_rigidbody = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
	{	
		CountDownLifeTime();
	}
	
	void OnCollisionEnter2D( Collision2D collision )
	{
		HandleHit( collision.collider, collision.contacts[0].point );
	}

	void OnCollisionStay2D( Collision2D collision )
	{
		// Fail safe in case enter is skipped
		HandleHit( collision.collider, collision.contacts[0].point );
	}

	public void FireProj( float rotation )
	{
		gameObject.SetActive( true );
		Physics2D.IgnoreCollision( m_firingCollider, m_collider, true );
		transform.rotation = Quaternion.AngleAxis( rotation, Vector3.forward );
		m_rigidbody.velocity = (Vector2)transform.up * m_speed + m_firingRb.velocity;
	}

	protected void CountDownLifeTime()
	{
		m_currLifeTime += Time.deltaTime;
		
		if( m_currLifeTime >= m_lifeTime )
		{
			DisableProjectile();
		}
	}

	protected void DisableProjectile()
	{
		m_currLifeTime = 0;
		gameObject.SetActive( false );
	}

	private void HandleHit( Collider2D col, Vector2 hitPos )
	{
		if( col.tag == "Ship" )
		{
			ShipScript ship = col.GetComponent<ShipScript>();
			ship.TakeHit( Vector2.zero, hitPos );
			ship.ApplyDamage( m_damage, m_shieldPenetration );
		}
		else if( col.tag == "Asteroid" )
		{
			// Do same sort of thing as with Ship
			Satellite sat = col.GetComponent<Satellite>();
			sat.ApplyDamage( m_damage, Vector2.zero );
		}
		DisableProjectile();
	}
}
