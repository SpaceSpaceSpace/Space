using UnityEngine;
using System.Collections;

public class WarpScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject levelObj = Resources.Load ( "Sectors/" + GameMaster.Master.PlanetName ) as GameObject;
		GameObject planet = Instantiate (levelObj);
		planet.name = GameMaster.Master.PlanetName;
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
		GameObject spaceStation = (GameObject)GameObject.Find ("SpaceStore");

		StartCoroutine ("WarpWaitTime");

		playerShip.transform.position = spaceStation.transform.position;
	}

	IEnumerator WarpWaitTime()
	{
		yield return new WaitForSeconds (5.0f);
	}

	public void WarpToPlanet(string prefabName)
	{
		GameMaster.Master.PlanetName = prefabName;
		Application.LoadLevel ( "MainScene" );
	}
}
