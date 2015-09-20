using UnityEngine;
using System.Collections;

public class Satellite : MonoBehaviour {

	public bool artificial;
	public float mass;
	public float radius;
	public float splitForce;
	private float rotationFactor;
	private Vector2 velocity;
	private Vector2 acceleration;
	private Transform centerOfOrbit;


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
		velocity = new Vector2(0, Random.Range(1, 2));
	}
	
	// Update is called once per frame
	void Update () {
		//velocity += acceleration;
		//transform.Rotate (0.0f, 0.0f, rotationFactor);
		transform.Rotate (0.0f,rotationFactor, 0.0f);
		//transform.Rotate (rotationFactor, 0.0f, 0.0f);
		transform.Translate(velocity);
	}

	void CalculateGravitationalForce(){
		float gravForce;
		Vector3 distanceBetween = centerOfOrbit.position - transform.position;
		rotationFactor = Mathf.Sin (Time.deltaTime * 2 * Mathf.PI);
		acceleration = distanceBetween * velocity.magnitude;
	}

	public void SetCenterOfOrbit(Transform cO){
		centerOfOrbit = cO;
	}
}
