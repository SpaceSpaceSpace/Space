using UnityEngine;
using System.Collections;

public class WarpScript : MonoBehaviour
{
	public GameObject hangarPrefab;
	public GameObject currentPlanet;
	private GameObject starBackground;
	public GameObject warpEffect;

	// Use this for initialization
	void Start () {
		starBackground = (GameObject)GameObject.Find ("StarBackground");
		LoadSector (GameMaster.Master.PlanetName);
	}

	void LoadSector(string sectorName)
	{
		GameObject levelObj = Resources.Load ( "Sectors/" + sectorName ) as GameObject;
		currentPlanet = Instantiate (levelObj);
		currentPlanet.name = GameMaster.Master.PlanetName;

		if (!starBackground.activeInHierarchy)
			starBackground.SetActive (true);
		if (GameMaster.CurrentGameState == GameState.Warping)
			GameMaster.CurrentGameState = GameState.Flying;
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

		StartCoroutine ("WarpWait");

        if(playerShip != null && spaceStation != null)
            playerShip.transform.position = spaceStation.transform.position;
	}

	IEnumerator WarpWait()
	{
		warpEffect.SetActive (true);
		yield return new WaitForSeconds (3.0f);

		warpEffect.SetActive(false);
		LoadSector(GameMaster.Master.PlanetName);
	}

	public void WarpToPlanet(string prefabName)
	{
		GameObject playerShip = (GameObject)GameObject.Find ("Player Ship");
		GameObject spaceStation = (GameObject)GameObject.Find ("SpaceStore");

		starBackground.SetActive (false);
		GameMaster.CurrentGameState = GameState.Warping;
		UI_Manager.instance.DisplayHangerUI (false);

		GameMaster.Master.PlanetName = prefabName;
		playerShip.transform.position = new Vector3(spaceStation.transform.position.x - 10.0f, spaceStation.transform.position.y, spaceStation.transform.position.z);
		playerShip.transform.rotation = Quaternion.identity;

		warpEffect.transform.position = starBackground.transform.position;
		StartCoroutine ("WarpWait");
		if(currentPlanet != null)
		{
			GameObject.Destroy(currentPlanet);
		}
	}
}
