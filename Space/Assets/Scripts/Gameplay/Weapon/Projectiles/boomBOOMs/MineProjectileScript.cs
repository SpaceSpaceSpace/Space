using UnityEngine;
using System.Collections;

public class MineProjectileScript : ExplosiveProjectile
{
	private Animator m_animator;
	private float m_plantedLifeTime;
	private float m_unplantedLifeTime = 3.0f;

	void Start()
	{
		m_plantedLifeTime = m_lifeTime - m_unplantedLifeTime;
		m_lifeTime = m_unplantedLifeTime;

		m_animator = GetComponent<Animator>();
	}

	void OnCollisionEnter2D( Collision2D collision )
	{
		PlantMine( collision.collider.transform );
	}
	
	void OnCollisionStay2D( Collision2D collision )
	{
		PlantMine( collision.collider.transform );
	}

	private void PlantMine( Transform target )
	{
		Destroy( GetComponent<Rigidbody2D>() );
		Destroy( GetComponent<Collider2D>() );
		m_lifeTime += m_plantedLifeTime;
		transform.parent = target;
	}

	public void TriggerDetonationAnim()
	{
		m_animator.SetTrigger( "Primed" );
	}
}
