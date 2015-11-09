using UnityEngine;
using System.Collections;

public class WarpScript : MonoBehaviour {

	public GameObject hangarPrefab;
	public GameObject currentPlanet;

	// Use this for initialization
	void Start () {
		LoadSector (GameMaster.Master.PlanetName);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LoadSector(string sectorName)
	{
		GameObject levelObj = Resources.Load ( "Sectors/" + sectorName ) as GameObject;
		currentPlanet = Instantiate (levelObj);
		currentPlanet.name = GameMaster.Master.PlanetName;
	}

	// This will eventually open the warp UI, for now just warps to space station
	void OnCollisionEnter2D(Collision2D coll)
	{
        if(coll.collider.gameObject == PlayerShipScript.player.gameObject)
            WarpToStation ();
	}

	public void CallHangar()
	{
		// Spawns the hangar off to the right of the screen
		float pointOffscreenX = Camera.main.ViewportToWorldPoint (new Vector3 (1.0f, 0.0f, 0.0f)).x;

		GameObject hangar = (GameObject)GameObject.Find ("Hangar");
		if (hangar == null) 
		{
			GameObject g = GameObject.Instantiate (hangarPrefab, new Vector3 (pointOffscreenX * 1.1f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
			g.name = "Hangar";
			g.transform.SetParent (currentPlanet.transform);
		} 
		else 
		{
			GameObject.Destroy(hangar);
		}

	}

	void WarpToStation()
	{
		GameObject playerShip = (GameObject)GameObject.Find ("Player Ship");
		GameObject spaceStation = (GameObject)GameObject.Find ("SpaceStore");

		StartCoroutine ("WarpWaitTime");

        if(playerShip != null && spaceStation != null)
            playerShip.transform.position = spaceStation.transform.position;
	}

	IEnumerator WarpWaitTime()
	{
		yield return new WaitForSeconds (5.0f);
	}

	public void WarpToPlanet(string prefabName)
	{
		GameObject playerShip = (GameObject)GameObject.Find ("Player Ship");
		GameObject spaceStation = (GameObject)GameObject.Find ("SpaceStore");

		GameMaster.Master.PlanetName = prefabName;
		playerShip.transform.position = new Vector3(spaceStation.transform.position.x - 10.0f, spaceStation.transform.position.y, spaceStation.transform.position.z);
		//Application.LoadLevel ( "MainScene" );
		if (currentPlanet != null) 
		{
			GameObject.Destroy(currentPlanet);
			LoadSector(GameMaster.Master.PlanetName);
		}
	}
}
