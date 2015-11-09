using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explode : MonoBehaviour {

	public float ParticleRadius = 5.0f;
	public int ParticleCount = 80;
	public float SpriteRadius = 2.0f;
	public float FadeSpeed = 1.0f;
	[Range(1,10)]
	public int MinExplosions = 1;
	[Range(1,10)]
	public int MaxExplosions = 4;

	public float MinSpeed = 0.1f;
	public float MaxSpeed = 2.0f;

	public List<Sprite> ExplosionSprites;

	private ParticleSystem m_explosionParticles;
	private AudioSource m_audioSrc;

	void Start()
	{
		m_audioSrc = GetComponent<AudioSource>();
		m_explosionParticles = GetComponentInChildren<ParticleSystem>();

		if(m_explosionParticles != null)
		{
			m_explosionParticles.maxParticles = ParticleCount;
			m_explosionParticles.startSpeed = ParticleRadius;
		}

		StartExplosion();
	}

	void Update()
	{
		if(!m_audioSrc.isPlaying)
		{
			if(m_explosionParticles != null && m_explosionParticles.isStopped)
				Destroy(gameObject);
			else
				Destroy(gameObject);
		}
	}

	private void StartExplosion()
	{
		m_audioSrc.Play ();
		if(m_explosionParticles)
			m_explosionParticles.Play();

		//Pick a random explosion sprite and velocity
		Sprite sprite = ExplosionSprites[Random.Range(0, ExplosionSprites.Count - 1)];

		int explosionCount = Random.Range (MinExplosions, MaxExplosions);
		for(int i = 0; i < explosionCount; i++)
		{
			GameObject obj = new GameObject();
			float rotation = Random.Range(0, 360);
			Vector3 rotVector = new Vector3(0,0,rotation);
			obj.transform.rotation = Quaternion.Euler(rotVector);
			obj.transform.position = transform.position;

			obj.name = "Dustplosion";
			SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
			sr.sortingOrder = 5;

			Explosion explosion = obj.AddComponent<Explosion>();
			explosion.SpriteRadius = SpriteRadius;
			explosion.FadeSpeed = FadeSpeed;
			explosion.ExplosionSprite = sprite;
			explosion.MinSpeed = MinSpeed;
			explosion.MaxSpeed = MaxSpeed;
		}
	}


}
