using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	[HideInInspector]
	public float ExpandSpeed = 5.0f;
	[HideInInspector]
	public float FadeSpeed = 1.0f;
	[HideInInspector]
	public Sprite ExplosionSprite;
	[HideInInspector]
	public Vector2 Velocity = new Vector2();

	private float m_scale = 0.4f;
	private float m_transparency = 1.0f;
	
	private SpriteRenderer m_spriteRenderer;
	private Material m_material;
	private Color m_color;
	
	void Start () 
	{
		m_spriteRenderer = GetComponent<SpriteRenderer>();
		m_material = m_spriteRenderer.material;
		m_color = m_material.color;

		m_spriteRenderer.sprite = ExplosionSprite;
	}
	
	void Update () 
	{
		float dt = Time.deltaTime;
		
		//Increment scale and transparency
		m_scale += ExpandSpeed * dt;
		m_transparency -= FadeSpeed * dt;
		
		//Set scale
		transform.localScale = new Vector3(m_scale, m_scale, 1);
		
		//Set transparency
		m_color.a = m_transparency;
		m_material.color = m_color;

		//Apply velocity
		transform.position += (Vector3)(Velocity * dt);

		//Finished when object will no longer be visible
		if(m_transparency <= 0)
			Destroy(gameObject);
	}
}
