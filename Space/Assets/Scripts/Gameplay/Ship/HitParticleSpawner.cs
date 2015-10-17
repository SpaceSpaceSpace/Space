using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitParticleSpawner : MonoBehaviour {
	
	public ParticleSystem hitParticleSystem;

	private List<ParticleSystem> m_particleSystems;

	void Start()
	{
		m_particleSystems = new List<ParticleSystem>();
	}

	void Update()
	{
		//Clean up particle systems when they die
		for(int i = 0; i < m_particleSystems.Count; i++)
		{
			ParticleSystem ps = m_particleSystems[i];
			//Destroy the PS when it's done playing
			if(!ps.IsAlive())
			{
				m_particleSystems.Remove(ps);
				Destroy (ps.gameObject);
			}
		}
	}

	void OnCollisionEnter2D( Collision2D collision )
	{
		ReactToHit(collision.contacts[0].point);
	}

	public void ReactToHit(Vector2 hitPoint)
	{
		Vector2 pos = this.gameObject.transform.position;

		//Create a new object with the particle system we've already defined
		GameObject hitObject = (GameObject)Instantiate(hitParticleSystem.gameObject, hitPoint, Quaternion.identity);
		Transform hitTrans = hitObject.transform;

		//It should be parented to this object
		hitTrans.parent = this.gameObject.transform;

		//Make it spawn where the collision occured
		hitTrans.position = hitPoint;

		//Add a copy of the particle system to this new object
		ParticleSystem hitPS = hitObject.GetComponent<ParticleSystem>();

		if(m_particleSystems == null)
		{
			Destroy (hitObject);
			return;
		}

		//Rotate PS to face collision
		Vector2 difference = hitPoint - (Vector2)pos;
		hitTrans.LookAt(hitPoint + difference);
			
		//Add PS to list of PSs
		m_particleSystems.Add(hitPS);

		//Play the particle system
		hitPS.Play();
	}
}
