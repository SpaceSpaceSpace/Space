using UnityEngine;
using System.Collections;

public class WarpScript : MonoBehaviour
{
	public GameObject hangarPrefab;

	// Use this for initialization
	void Start () {
        GameObject levelObj = GameMaster.Sectors[GameMaster.Master.PlanetName].gameObject;
		GameObject planet = Instantiate (levelObj);
		planet.name = GameMaster.Master.PlanetName;
	}
	
	// Update is called once per frame
	void Update () {
	
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
		GameObject.Instantiate (hangarPrefab, new Vector3 (pointOffscreenX * 1.1f, 0.0f, 0.0f), Quaternion.identity);
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
		GameMaster.Master.PlanetName = prefabName;
		Application.LoadLevel ( "MainScene" );
	}
}
