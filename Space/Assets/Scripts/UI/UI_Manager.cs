using UnityEngine;
using System.Collections;

public class UI_Manager : MonoBehaviour {

	public static UI_Manager instance;
	public GameObject BountyBoard;
	public GameObject ShopBoard;

	// Use this for initialization
	void Start () {
		instance = this;
	}

	void PassContractsToSpaceStation()
	{

	}

	public void DisplayBountyBoard(bool active)
	{
		BountyBoard.GetComponent<BountyBoard> ().DestroyButtons ();
		BountyBoard.SetActive (active);
	}
	public void DisplayShopBoard(bool active)
	{
		ShopBoard.GetComponent<ShopBoard> ().DestroyButtons ();
		ShopBoard.SetActive (active);
	}
}
