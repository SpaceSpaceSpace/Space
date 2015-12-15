using UnityEngine;
using System.Collections.Generic;

public enum GameState
{
	MainMenu,
	Flying,
	Station,
	Customization,
	GameOver,
	Pause,
	H,
	CHelp,
	Warping
}

public class GameMaster : MonoBehaviour {

	public static GameMaster Master;
	public static GameState CurrentGameState = GameState.Flying;
	public static PlayerData playerData;
	public static WeaponManager WeaponMngr;

    public static Dictionary<string, Sector> Sectors = new Dictionary<string, Sector>();

	public string PlanetName = "Stan C8";

	void Awake ()
	{
		//There can be only one
		if(Master == null)
		{
			DontDestroyOnLoad(gameObject);
			Master = this;

			playerData = transform.GetComponent<PlayerData> ();
			WeaponMngr = GetComponent<WeaponManager>();

            //Load all sectors
            Sector[] sectorObjects = Resources.LoadAll<Sector>("Sectors/");
            foreach (Sector s in sectorObjects)
                Sectors[s.name] = s;
		}
		else if(Master != this)
		{
			Destroy(gameObject);
		}
	}

	void OnLevelWasLoaded(int level)
	{
		if(level == 1)
		{
			Time.timeScale = 1.0f;
			PlanetName = "Stan C8";
			playerData.playerInventory = new Inventory();
			playerData.playerContracts = new List<Contract>();
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
