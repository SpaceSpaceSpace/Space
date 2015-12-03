using UnityEngine;
using System.Collections;

public class BlackHole : MonoBehaviour
{
	public float rotationSpeed = 200;
	public float suckDelay = 1.0f;
	public float speed = 10.0f;
	public float radius = 10.0f;

	private Rigidbody2D m_rigidbody;
	private ParticleSystem m_particleSys;
	private bool m_sucking = false;

	void Start()
	{
		m_rigidbody = GetComponent<Rigidbody2D>();
		m_particleSys = GetComponent<ParticleSystem>();

		m_particleSys.startSpeed = -radius;
		transform.localScale *= radius / 10;
	}

	void Fire( Vector2 inheritedVelocity )
	{
		m_rigidbody.velocity = (Vector2)transform.up * speed + inheritedVelocity;
	}

	void Update()
	{
		transform.Rotate( 0, 0, rotationSpeed * Time.deltaTime );

		if( !m_sucking )
		{
			suckDelay -= Time.deltaTime;
			
			if( suckDelay <= 0 )
			{
				m_sucking = true;
				Destroy( m_rigidbody );
				m_particleSys.Play();
			}
		}
	}

	void FixedUpdate()
	{
		if( m_sucking )
		{
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll( transform.position, radius );
			
			foreach( Collider2D col in hitColliders )
			{
				Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
				
				if( rb != null )
				{
					Vector3 toCenter =  transform.position - col.transform.position;

					float percent = Mathf.Max( 0,  radius - toCenter.magnitude ) / radius;

					//Vector3 tangential = new Vector3( -toCenter.y, toCenter.x );
					Vector3 force = toCenter * ( 1 - percent ) * 10 * rb.mass;
					rb.AddForce( force, ForceMode2D.Force );
				}
			}
		}
	}
}
