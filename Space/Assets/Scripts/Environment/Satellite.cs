using UnityEngine;
using System.Collections;

public class Satellite : MonoBehaviour {

	public bool artificial;
	public bool inOrbit;
	public float mass;
	public const float MAX_VELOCITY = 2.0f;
	public Vector2 radius;
	public float splitForce;
	public Vector3 velocity;
	public Vector3 acceleration;
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
		velocity = new Vector3(Random.Range(0,0.2f),Random.Range(0,0.2f), 0.0f);
		acceleration = new Vector3 (0, 0);
		centerOfOrbit = new Vector3 (0.0f, 0.0f, 0.0f);

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
		transform.GetComponent<Rigidbody2D> ().AddForce (new Vector2(velocity.x, velocity.y));
	}

	void CalculateOrbitalForce(){
		float gravForce;
		velocity = new Vector3 (transform.forward.x, transform.forward.y, 0.0f);
	}

	public void SetCenterOfOrbit(Transform cO){
		//centerOfOrbit = cO;
	}
}
