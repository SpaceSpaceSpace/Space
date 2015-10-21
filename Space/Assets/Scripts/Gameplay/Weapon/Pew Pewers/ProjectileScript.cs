using UnityEngine;

// Basic projectile script
public class ProjectileScript : MonoBehaviour
{
	public float knockback = 1.0f;
	public float shieldPenetration = 0.0f;
	
	public float speed = 10.0f;
	public float lifeTime = 1.0f;
	public float damage = 10.0f;

	private Collider2D m_firingCollider;
	private Collider2D m_collider;
	private Rigidbody2D m_rigidbody;
	private Rigidbody2D m_firingRb;
	private float m_currLifeTime = 0;

	public void Init( float damage, float speed, float lifeTime, GameObject firingShip )
	{
		this.damage = damage;
		this.speed = speed;
		this.lifeTime = lifeTime;
		m_currLifeTime = lifeTime;

		m_firingCollider = firingShip.GetComponent<Collider2D>();
		m_collider = GetComponent<Collider2D>();
		m_firingRb = firingShip.GetComponent<Rigidbody2D>();
		m_rigidbody = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
	{	
		m_currLifeTime -= Time.deltaTime;
		
		if( m_currLifeTime <= 0 )
		{
			DisableProjectile();
		}
	}
	
	void OnCollisionEnter2D( Collision2D collision )
	{
		Collider2D col = collision.collider;

		if( col.tag == "Ship" )
		{
			ShipScript ship = col.GetComponent<ShipScript>();
			ship.TakeHit( Vector2.zero, collision.contacts[0].point );
			ship.ApplyDamage( damage, shieldPenetration );
		}
		else if( col.tag == "Asteroid" )
		{
			// Do same sort of thing as with Ship
			Satellite sat = col.GetComponent<Satellite>();
			sat.ApplyDamage( damage, Vector2.zero );
		}
		DisableProjectile();
	}

	void OnCollisionStay2D( Collision2D col )
	{
		// Fail safe in case enter is skipped
		DisableProjectile();
	}

	public void FireProj( float rotation )
	{
		gameObject.SetActive( true );
		Physics2D.IgnoreCollision( m_firingCollider, m_collider, true );
		transform.rotation = Quaternion.AngleAxis( rotation, Vector3.forward );
		m_rigidbody.velocity = (Vector2)transform.up * speed + m_firingRb.velocity;
	}

	private void DisableProjectile()
	{
		m_currLifeTime = lifeTime;
		gameObject.SetActive( false );
	}
}
