using UnityEngine;
using System.Collections;

public class HitParticleSpawner : MonoBehaviour {
	
	private ParticleSystem m_hitParticles;

	void Start()
	{
		m_hitParticles = GetComponent<ParticleSystem>();
	}

	public void ReactToHit(Vector2 hitPoint)
	{
		//Rotate HitPS to face collision
		Transform hitTrans = m_hitParticles.gameObject.transform;
		hitTrans.LookAt(hitPoint);
			
		m_hitParticles.Play ();
	}
}
