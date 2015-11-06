using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	
	[HideInInspector]
	public float SpriteRadius = 2.0f;
	[HideInInspector]
	public float FadeSpeed = 1.0f;
	[HideInInspector]
	public Sprite ExplosionSprite;
	[HideInInspector]
	public float MinSpeed;
	[HideInInspector]
	public float MaxSpeed;

	private float m_scale = 0.4f;
	private float m_transparency = 1.0f;
	
	private SpriteRenderer m_spriteRenderer;
	private Material m_material;
	private Color m_color;

	private Vector2 direction;
	private float speed;

	bool m_doneFading = false;
	
	void Start () 
	{
		m_spriteRenderer = GetComponent<SpriteRenderer>();
		m_material = m_spriteRenderer.material;
		m_color = m_material.color;

		m_spriteRenderer.sprite = ExplosionSprite;

		float x = Random.Range(-1f, 1f);
		float y = Random.Range(-1f, 1f);
		direction = new Vector2(x, y).normalized;
		speed = Random.Range(MinSpeed, MaxSpeed);

		StartCoroutine(Explode());
	}

	IEnumerator Explode()
	{
		float elapsedTime = 0;

		while(elapsedTime < FadeSpeed)
		{
			m_scale = Mathf.Lerp(m_scale, SpriteRadius, elapsedTime);
			m_transparency = Mathf.Lerp(m_transparency, 0, elapsedTime);

			//Set transparency
			m_color.a = m_transparency;
			m_material.color = m_color;

			//Set Scale
			transform.localScale = new Vector3(m_scale,m_scale,1);

			//Add to time
			elapsedTime += (Time.deltaTime / FadeSpeed);
			
			yield return null;
		}

		m_doneFading = true;
		yield return null;
	}

	void Update () 
	{
		//Apply velocity
		transform.position += (Vector3)(direction * speed * Time.deltaTime);

		//Finished when object will no longer be visible
		if(m_doneFading)
			Destroy(gameObject);
	}
}
