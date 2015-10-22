using UnityEngine;
using System.Collections;

public class MainMenu_Manager : MonoBehaviour {

	public GameObject loadingText;
	public GameObject mainMenuGroup;

	public void LoadNewGame()
	{
		mainMenuGroup.SetActive (false);
		loadingText.SetActive (true);
		GameMaster.CurrentGameState = GameState.Flying;

		Application.LoadLevel ("MainScene");
	}

	public void Quit()
	{
		Application.Quit ();
	}
}
