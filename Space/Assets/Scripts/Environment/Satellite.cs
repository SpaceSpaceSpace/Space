using UnityEngine;
using System.Collections;

public class Satellite : MonoBehaviour {

	public bool artificial;
	public bool inOrbit;
	public float mass;
	public const float MAX_VELOCITY = 2.0f;
	public const float GRAVITATION_MAGNITUDE = 0.9f;
	public const float STARTING_IMPULSE = 12f;
	public Vector2 radius;
	public float splitForce;
	public Vector3 velocity;
	public Vector3 centerOfOrbit;


	// Use this for initialization
	void Start () {
		//If it is a Satellite
		if(artificial)
		{
			Sprite image = Resources.Load<Sprite>("TempSat");
			GetComponent<SpriteRenderer>().sprite = image;
		}
		else
		{
			int randomSpriteNum = Random.Range (1, 9);
			string path = "AsteroidSprites/asteroid" + randomSpriteNum;
			Sprite image = Resources.Load<Sprite> (path);
			GetComponent<SpriteRenderer> ().sprite = image;
		}
		mass = Random.Range (1, 9);
		Vector3 toCenter = centerOfOrbit - transform.position;
		Vector2 tangential = new Vector2(-toCenter.y, toCenter.x);
		tangential.Normalize();
		velocity = new Vector3(tangential.x,tangential.y, 0.0f);
		centerOfOrbit = new Vector3 (0.0f, 0.0f, 0.0f);
		transform.GetComponent<Rigidbody2D>().AddForce(STARTING_IMPULSE * velocity / toCenter.magnitude * mass, ForceMode2D.Impulse);

	}
	
	// Update is called once per frame
	void Update () {

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
		toCenter.Normalize();
		Vector3 gravity = GRAVITATION_MAGNITUDE * toCenter;
		transform.GetComponent<Rigidbody2D> ().AddForce (new Vector2(gravity.x, gravity.y));
	}

	public void SetCenterOfOrbit(Transform cO){
		//centerOfOrbit = cO;
	}
}
