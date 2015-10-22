using UnityEngine;
using System.Collections;

public class ExplosiveProjectile : ProjectileScript
{
	public float blastRadius = 10.0f;
	public float blastForce = 10.0f;
	public float safetyTime = 0.5f;

	private bool m_safety = true; // Shouldn't detonate too soon after firing

	private static GameObject ms_explosion;

	public bool CanBoom
	{
		get { return !m_safety; }
	}

	void Awake()
	{
		if(ms_explosion == null)
			ms_explosion = Resources.Load("ShipPrefabs/ExplosiveExplosion") as GameObject;
	}

	void OnCollisionEnter2D( Collision2D collision )
	{
		HandleCollision( collision );
	}

	void OnCollisionStay2D( Collision2D collision )
	{
		HandleCollision( collision );
	}

	void Update ()
	{
		CountDownLifeTime();

		if( m_safety )
		{
			m_safety = ( m_currLifeTime < safetyTime );
		}
	}

	public void Detonate()
	{
		if( !m_safety )
		{
			Instantiate(ms_explosion, transform.position, Quaternion.identity);
			HandleExplosion();
		}

		DisableProjectile();
	}

	private void HandleCollision( Collision2D collision )
	{
		Detonate();
	}

	private void HandleExplosion()
	{
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll( transform.position, blastRadius );
		
		foreach( Collider2D col in hitColliders )
		{
			Vector3 pos = col.transform.position;
			Vector2 impulse = pos - transform.position;

			impulse /= impulse.magnitude;
			impulse *= blastForce;

			float percent = ( blastRadius - Vector3.Distance( pos, transform.position ) ) / blastRadius;

			if( col.tag == "Ship" )
			{
				ShipScript ship = col.GetComponent<ShipScript>();
				ship.TakeHit( impulse * percent, pos );
				ship.ApplyDamage( m_damage * percent, m_shieldPenetration );
			}
			else if( col.tag == "Asteroid" )
			{
				//Rigidbody2D roidRb = col.GetComponent<Rigidbody2D>();
				//roidRb.AddForce( impulse * percent, ForceMode2D.Impulse );
				Satellite sat = col.GetComponent<Satellite>();
				sat.ApplyDamage( m_damage * percent, Vector2.zero );
			}
		}
	}
}
