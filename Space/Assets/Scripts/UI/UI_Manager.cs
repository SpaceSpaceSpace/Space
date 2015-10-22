using UnityEngine;
using System.Collections;

public class UI_Manager : MonoBehaviour {

	public static UI_Manager instance;
	public GameObject BountyBoard;
	public GameObject GameOverScreen;
	public GameObject spaceStation;
	public PlayerShipScript player;

	// Use this for initialization
	void Start () {
		instance = this;
		SetAllScreensToInactive ();
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

	public void ChangeUIStateWithButtonClick(int state)
	{
		ChangeUIState ((GameState)state);
	}

	public void ChangeUIState(GameState state)
	{
		SetAllScreensToInactive ();

		switch(state)
		{
			case GameState.GameOver:
				GameOverScreen.SetActive(true);
				break;
			case GameState.MainMenu:
				GameMaster.CurrentGameState = GameState.MainMenu;
				Application.LoadLevel("MainMenu");
				break;
					
		}
	}

	private void SetAllScreensToInactive()
	{
		BountyBoard.SetActive (false);
		GameOverScreen.SetActive (false);
	}
}
