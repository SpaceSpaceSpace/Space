using UnityEngine;
using System.Collections;

public class Satellite : MonoBehaviour {

	public bool artificial;

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
	
	}
}
