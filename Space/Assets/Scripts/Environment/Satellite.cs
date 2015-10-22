using UnityEngine;
using System.Collections;

public class Satellite : MonoBehaviour {

	public bool artificial;
	public bool inOrbit;
	public float mass;
	public GameObject satPrefab;
	public GameObject satPrefab1;
	public const float MAX_VELOCITY = 1000.0f;
	public const float GRAVITATION_MAGNITUDE = 10.0f;
	//public const float STARTING_IMPULSE = 12f;
	public Vector2 radius;
	public float splitForce;
	public Vector3 velocity;
	public Vector3 centerOfOrbit;
	public float health;

	private static GameObject m_dustplosion = null;
	
	// Use this for initialization
	void Start () {
		float semiMajor;
		if (radius.x > radius.y)
			semiMajor = radius.x;
		else
			semiMajor = radius.y;

		/*if(!artificial)
		{
			mass = Random.Range (1, 9);
			transform.localScale = new Vector3 (mass, mass, 1);
			transform.GetComponent<Rigidbody2D> ().mass = mass * 100.0f;
			health = 5.0f * mass;
		}*/
		centerOfOrbit = new Vector3 (0.0f, 0.0f, 0.0f);
		Vector3 toCenter = centerOfOrbit - transform.position;
		Vector2 tangential = new Vector2(-toCenter.y, toCenter.x);
		tangential.Normalize();
		velocity = new Vector3(tangential.x,tangential.y, 0.0f);
		velocity *= Mathf.Sqrt (GRAVITATION_MAGNITUDE * (10000.0f / toCenter.magnitude - 1.0f / semiMajor));

		transform.GetComponent<Rigidbody2D>().AddForce( new Vector2(velocity.x,velocity.y), ForceMode2D.Impulse);

		//Load dustplosion
		if(m_dustplosion == null)
			m_dustplosion = Resources.Load("AsteroidExplosion") as GameObject;
	}

	// Update is called once per frame
	void FixedUpdate () {

		//transform.Rotate (0.0f, 0.0f, rotationFactor);
		//transform.Rotate (0.0f,rotationFactor, 0.0f);
		//transform.Rotate (rotationFactor, 0.0f, 0.0f);
		CalculateOrbitalForce ();
		while (velocity.magnitude > MAX_VELOCITY) {
			velocity /= 2.0f;
		}
		//transform.GetComponent<Rigidbody2D> ().AddForce (new Vector2(velocity.x, velocity.y));

	}

	void CalculateOrbitalForce(){
		velocity = new Vector3 (transform.forward.x, transform.forward.y, 0.0f);
		Vector3 toCenter = centerOfOrbit - transform.position;
		float radius = toCenter.magnitude;
		toCenter.Normalize();
		Vector3 gravity = GRAVITATION_MAGNITUDE * toCenter / (radius * radius);
		transform.GetComponent<Rigidbody2D> ().AddForce (new Vector2(gravity.x, gravity.y));
	}
	public void ApplyDamage(float damage, Vector2 impulse){
		health -= damage;
		Vector3 imp = new Vector3(impulse.x,impulse.y,0.0f);
		transform.GetComponent<Rigidbody2D>().AddForce(imp*10.0f, ForceMode2D.Impulse);
		if (health <= 0) {
			/*GameObject split1 = (GameObject) Instantiate(satPrefab, transform.position, Quaternion.identity);
			split1.GetComponent<Satellite>().ScaleMass(mass/2);
			GameObject split2 = (GameObject) Instantiate(satPrefab, transform.position, Quaternion.identity);
		 	split2.GetComponent<Satellite>().ScaleMass(mass/2);*/
			Split (mass, imp);

		}
	}
	public void ScaleMass(float m, bool split){
		mass = m;
		transform.localScale = new Vector3 (m, m, 1);
		transform.GetComponent<Rigidbody2D> ().mass = mass * 75.0f;
		if(split){		health = 10.0f * m;}
		else { health = 20.0f * m;}
	}
	public void OnCollisionStay2D(Collision2D coll) {
		//damage asteroids that remain in contact with eachother
		if (coll.gameObject.tag == "Asteroid")
			ApplyDamage (coll.relativeVelocity.magnitude, Vector2.zero);
		
	}
	public void Split(float m, Vector3 impulse)
	{
		//destroy the asteroid if it is too small to split
		if(mass <  1.0f){
			Destroy(gameObject);
		}
		//otherwise, split it
		else
		{
			//spawn asteroids apart from each other
			Vector3 offset1 = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),0.0f);
			Vector3 offset2 = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),0.0f);
			GameObject split1 = (GameObject)Instantiate(satPrefab,transform.position + offset1,Quaternion.identity);
			split1.GetComponent<Satellite>().ScaleMass(m/2, true);
			GameObject split2 = (GameObject)Instantiate(satPrefab,transform.position + offset2,Quaternion.identity);
			split2.GetComponent<Satellite>().ScaleMass(m/2, true);

			//add forces that push them apart and in the direction of impact
			split1.GetComponent<Rigidbody2D>().AddForce(new Vector2(impulse.x * 20.0f,impulse.y* 20.0f), ForceMode2D.Impulse);
			split2.GetComponent<Rigidbody2D>().AddForce(new Vector2(impulse.x* 20.0f,impulse.y* 20.0f), ForceMode2D.Impulse);
			split1.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-50,0),Random.Range(-50,0)));
			split2.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(0,50),Random.Range(0,50)));

			//destroy the parent asteroid gameobject
			Destroy(gameObject);
		}

		//Spawn a little dust poof
		float rotation = Random.Range(0, 360);
		Vector3 rotVector = new Vector3(0,0,rotation);
		GameObject dust = Instantiate(m_dustplosion,transform.position, Quaternion.Euler(rotVector)) as GameObject;

		//Set the fade speed of the dust poof to be inversely proportional to the mass of the satellite
		Explode explosion = dust.GetComponent<Explode>();
		explosion.FadeSpeed = 1/m;
		Instantiate(explosion, transform.position, Quaternion.identity);
	}

}
