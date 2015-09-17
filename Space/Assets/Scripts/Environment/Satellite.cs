using UnityEngine;
using System.Collections;

public class Satellite : MonoBehaviour {

	public bool artificial;
	public float mass;
	public float radius;
	public float splitForce;
	public float GRAV_CONSTANT  = 6.673f * (1/Mathf.Pow(10,11)); 
	public float ORBITAL_CENTER_MASS = 100.0f;
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
	}
	
	// Update is called once per frame
	void Update () {
		velocity += acceleration;
		transform.Translate(velocity);
	}

	void CalculateGravitationalForce(){
		float gravForce;
		Vector3 distanceBetween = centerOfOrbit.position - transform.position;
		float distance = Vector3.Distance(centerOfOrbit.position, transform.position);
		Vector3 unitVec = distanceBetween.Normalize;
		gravForce = -GRAV_CONSTANT * (ORBITAL_CENTER_MASS * mass)/distance * unitVec;
		acceleration = unitVec * gravForce;
	}

	public void SetCenterOfOrbit(Transform cO){
		centerOfOrbit = cO;
	}
}
