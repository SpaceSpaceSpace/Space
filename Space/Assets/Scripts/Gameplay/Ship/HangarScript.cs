using UnityEngine;
using System.Collections;

public class HangarScript : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player Ship");
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		// Hangar UI
		GameObject level = GameObject.Find("Planet1");
		
		GameObject warpMngr = GameObject.Find("Warp Manager");
		WarpScript warpScript = warpMngr.GetComponent<WarpScript>();
		
		if(level != null)
		{
			print( "to planet 2" );
			warpScript.WarpToPlanet("Planet2");
		}
		else
		{
			print( "to planet 1" );
			warpScript.WarpToPlanet("Planet1");
		}
	}

	void GoToPlayer()
	{
		// Seek code for Hangar to drift toward player
		//gameObject.transform.LookAt (player.transform.position);

		gameObject.transform.position += (player.transform.position - transform.position) * 1.0f * Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {

		// Seeking code call
		GoToPlayer ();
	}
}
