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
	public List<Sprite> ExplosionSprites;
	
	private AudioSource m_audioSrc;

	void Start()
	{
		m_audioSrc = GetComponent<AudioSource>();
	}

	public void StartExplosion()
	{
		m_audioSrc.Play ();

		//Pick a random explosion sprite and velocity
		Sprite sprite = ExplosionSprites[Random.Range(0, ExplosionSprites.Count - 1)];
		Vector2 velocity = new Vector2(Random.Range(0, 2.0f), Random.Range(0, 2.0f));

		int explosionCount = Random.Range (MinExplosions, MaxExplosions);
		for(int i = 0; i < explosionCount; i++)
		{
			GameObject obj = new GameObject();
			float rotation = Random.Range(0, 360);
			Vector3 rotVector = new Vector3(0,0,rotation);
			obj.transform.rotation = Quaternion.Euler(rotVector);

			Explosion explosion = obj.AddComponent<Explosion>();
			explosion.ExpandSpeed = ExpandSpeed;
			explosion.FadeSpeed = FadeSpeed;
			explosion.ExplosionSprite = sprite;
			explosion.Velocity = velocity;
		}
	}



}
