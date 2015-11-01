using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explode : MonoBehaviour {

	public float ExpandSpeed = 5.0f;
	public float FadeSpeed = 1.0f;
	[Range(1,10)]
	public int MinExplosions = 1;
	[Range(1,10)]
	public int MaxExplosions = 4;

	public float MinSpeed = 0.1f;
	public float MaxSpeed = 2.0f;

	public List<Sprite> ExplosionSprites;
	
	private AudioSource m_audioSrc;

	void Start()
	{
		m_audioSrc = GetComponent<AudioSource>();

		StartExplosion();
	}

	void Update()
	{
		if(!m_audioSrc.isPlaying)
			Destroy(gameObject);
	}

	private void StartExplosion()
	{
		m_audioSrc.Play ();

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
			explosion.ExpandSpeed = ExpandSpeed;
			explosion.FadeSpeed = FadeSpeed;
			explosion.ExplosionSprite = sprite;
			explosion.MinSpeed = MinSpeed;
			explosion.MaxSpeed = MaxSpeed;
		}
	}


}
