using UnityEngine;
using System.Collections;

public enum GameState
{
	MainMenu,
	Flying,
	Customization
}

public class GameMaster : MonoBehaviour {

	public static GameMaster Master;
	public static GameState CurrentGameState = GameState.MainMenu;

	void Awake ()
	{
		//There can be only one
		if(Master == null)
		{
			DontDestroyOnLoad(gameObject);
			Master = this;
		}
		else if(Master != this)
		{
			Destroy(gameObject);
		}
	}

	void Update () {

		Debug.Log (CurrentGameState);

		//Go to Main Menu
		if(Input.GetKey(KeyCode.F1))
		{
			CurrentGameState = GameState.MainMenu;
		}

		//Go to Flying mode
		if(Input.GetKey (KeyCode.F2))
		{
			CurrentGameState = GameState.Flying;
		}

		//Go to Customization Mode
		if(Input.GetKey(KeyCode.F3))
		{
			CurrentGameState = GameState.Customization;
		}
	}
}
