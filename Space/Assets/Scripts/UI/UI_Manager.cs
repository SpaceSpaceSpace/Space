using UnityEngine;
using System.Collections;

public class UI_Manager : MonoBehaviour {

	public static UI_Manager instance;
	public GameObject BountyBoard;
	public GameObject spaceStation;
	public PlayerShipScript player;

	// Use this for initialization
	void Start () {
		instance = this;
	}

	public void DisplayBountyBoard(bool active)
	{
		BountyBoard.GetComponent<BountyBoard> ().DestroyButtons ();
		BountyBoard.SetActive (active);

		if(active)
		{
			player.Dock();
			player.transform.position = spaceStation.transform.position;
			player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		}
		else
		{
			player.Undock();
		}
	}
}
