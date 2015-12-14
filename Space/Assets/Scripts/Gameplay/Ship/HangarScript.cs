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
		if(coll.gameObject.name == "Player Ship")
			UI_Manager.instance.DisplayHangerUI (true);
	}

	void GoToPlayer()
	{
		// Seek code for Hangar to drift toward player
		//gameObject.transform.LookAt (player.transform.position);
		if(player != null)
			gameObject.transform.position += (player.transform.position - transform.position) * 1.0f * Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {

		// Seeking code call
		GoToPlayer ();
	}
}
