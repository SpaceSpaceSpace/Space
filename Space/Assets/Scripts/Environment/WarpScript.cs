using UnityEngine;
using System.Collections;

public class WarpScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// This will eventually open the warp UI, for now just warps to space station
	void OnCollisionEnter2D(Collision2D coll)
	{
		Debug.Log ("Out of Bounds!");
		WarpToStation ();
	}

	void WarpToStation()
	{
		GameObject playerShip = (GameObject)GameObject.Find ("Player Ship");
		GameObject spaceStation = (GameObject)GameObject.Find ("SpaceStoreOrbit");

		StartCoroutine ("WarpWaitTime");

		playerShip.transform.position = spaceStation.transform.position;
	}

	IEnumerator WarpWaitTime()
	{
		GUI.Label (new Rect (0, 1, 1, 1), "Warping");
		yield return new WaitForSeconds (5.0f);
		
	}
}
