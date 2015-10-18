using UnityEngine;
using System.Collections;

public class Explode : MonoBehaviour {

	public float ExpandSpeed = 5.0f;
	public float FadeSpeed = 1.0f;

	private float m_scale = 0.4f;
	private float m_transparency = 1.0f;

	private Material m_material;
	private Color m_color;
	private AudioSource m_audioSrc;

	void Start()
	{
		m_audioSrc = GetComponent<AudioSource>();
		m_material = GetComponent<SpriteRenderer>().material;
		m_color = m_material.color;

		m_audioSrc.Play ();
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

		//Finished when object will no longer be visible
		if(m_transparency <= 0)
			Destroy(gameObject);
	}
}
