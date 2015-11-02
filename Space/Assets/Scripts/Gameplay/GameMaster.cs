using UnityEngine;
using System.Collections;

public enum GameState
{
	MainMenu,
	Flying,
	Customization,
	GameOver
}

public class GameMaster : MonoBehaviour {

	public static GameMaster Master;
	public static GameState CurrentGameState = GameState.Flying;
	public static PlayerData playerData;
	public static WeaponManager WeaponMngr;

	void Awake ()
	{
		//There can be only one
		if(Master == null)
		{
			DontDestroyOnLoad(gameObject);
			Master = this;

			playerData = transform.GetComponent<PlayerData> ();
			WeaponMngr = GetComponent<WeaponManager>();
		}
		else if(Master != this)
		{
			Destroy(gameObject);
		}
	}

	void Update () {

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
	}
}
