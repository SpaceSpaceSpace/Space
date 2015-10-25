using UnityEngine;
using System.Collections;

public class MineProjectileScript : ExplosiveProjectile
{
	private Animator m_animator;

	void Start()
	{
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
		transform.parent = target;
	}

	public void TriggerDetonationAnim()
	{
		m_animator.SetTrigger( "Primed" );
	}
}
