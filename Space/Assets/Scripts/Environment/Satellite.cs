using UnityEngine;
using System.Collections;

public class Satellite : MonoBehaviour {

	public bool artificial;
	public bool inOrbit;
	public float mass;
	public const float MAX_VELOCITY = 1000.0f;
	public const float GRAVITATION_MAGNITUDE = 15.0f;
	//public const float STARTING_IMPULSE = 12f;
	public Vector2 radius;
	public float splitForce;
	public Vector3 velocity;
	public Vector3 centerOfOrbit;

	// Use this for initialization
	void Start () {
		float semiMajor;
		if (radius.x > radius.y)
			semiMajor = radius.x;
		else
			semiMajor = radius.y;

		if(!artificial)
		{
			mass = Random.Range (1, 9);
			transform.localScale = new Vector3 (mass, mass, 1);
			transform.GetComponent<Rigidbody2D> ().mass = mass * 100.0f;
		}

		Vector3 toCenter = centerOfOrbit - transform.position;
		Vector2 tangential = new Vector2(-toCenter.y, toCenter.x);
		tangential.Normalize();
		velocity = new Vector3(tangential.x,tangential.y, 0.0f);
		velocity *= Mathf.Sqrt (GRAVITATION_MAGNITUDE * (10000.0f / toCenter.magnitude - 1.0f / semiMajor));
		centerOfOrbit = new Vector3 (0.0f, 0.0f, 0.0f);
		transform.GetComponent<Rigidbody2D>().AddForce( velocity, ForceMode2D.Impulse);

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

	public void SetCenterOfOrbit(Transform cO){
		//centerOfOrbit = cO;
	}
}
