using UnityEngine;
using System.Collections;

public class UI_Manager : MonoBehaviour
{

    public static UI_Manager instance;
    public GameObject bountyBoard;
    public GameObject storeBoard;
    public GameObject gameOverScreen;
    public GameObject spaceStationUI;
	public GameObject hangerUI;
	public GameObject weaponToggles;
	public PlayerShipScript player;
	public WeaponDock weaponDockUI;

	public Camera otherGameCamera;

    private GameObject spaceStationObject;
	public GameObject SpaceStationObject
	{
		set{spaceStationObject = value;}
		get{return spaceStationObject;}
	}

    // Use this for initialization
    void Start()
    {
        instance = this;
		spaceStationObject = GameObject.Find ("SpaceStore");
        SetAllScreensToInactive();
    }
	
	void Update()
	{
		if(GameMaster.CurrentGameState == GameState.Station)
		{
			player.transform.position = spaceStationObject.transform.position;
		}
	}

    public void DisplaySpaceStationUI(bool active)
    {
		Camera.main.rect = new Rect (0f, 0f, 1f, 1f);
		otherGameCamera.rect = new Rect (0f, 0f, 1f, 1f);

        spaceStationUI.SetActive(active);
		GameMaster.CurrentGameState = GameState.Station;

        if (active)
        {
            player.Dock();
			player.transform.position = spaceStationObject.transform.position;
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        else
        {
			GameMaster.CurrentGameState = GameState.Flying;
            player.Undock();
        }
    }

    public void DisplayBountyBoard(bool active)
    {
        bountyBoard.GetComponent<BountyBoard>().DestroyButtons();
        bountyBoard.SetActive(active);

        if (active)
        {
            player.Dock();
			player.transform.position = spaceStationObject.transform.position;
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        else
        {
			GameMaster.CurrentGameState = GameState.Station;
        }
    }

    public void DisplayStoreBoard(bool active)
    {
        storeBoard.GetComponent<StoreBoard>().DestroyButtons();
        storeBoard.SetActive(active);

        if (active)
        {
            player.Dock();
			player.transform.position = spaceStationObject.transform.position;
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        else
        {
			GameMaster.CurrentGameState = GameState.Station;
        }
    }

	public void DisplayHangerUI(bool active)
	{
		hangerUI.SetActive (active);

		if(active)
		{
			player.Dock();
			player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		}
		else
		{
			player.Undock();
		}
	}

	public void UpdateWeaponDockUI()
	{
		weaponDockUI.UpdateWeaponDockUI ();
	}

    public void ChangeUIStateWithButtonClick(int state)
    {
        ChangeUIState((GameState)state);
    }

    public void ChangeUIState(GameState state)
    {
        SetAllScreensToInactive();

        switch (state)
        {
            case GameState.GameOver:
                gameOverScreen.SetActive(true);
                break;
            case GameState.MainMenu:
                GameMaster.CurrentGameState = GameState.MainMenu;
                Application.LoadLevel("MainMenu");
                break;
            case GameState.Customization:
                GameMaster.CurrentGameState = GameState.Customization;
                break;
        }
    }

	public void ToggleStationMarker()
	{
		if(PlayerShipScript.player.stationMarker.activeSelf)
		{
			PlayerShipScript.player.stationMarker.SetActive(false);
		}
		else
		{
			PlayerShipScript.player.stationMarker.SetActive(true);
		}
	}

    public void SetAllScreensToInactive()
    {
        bountyBoard.SetActive(false);
        gameOverScreen.SetActive(false);
        storeBoard.SetActive(false);
        spaceStationUI.SetActive(false);
    }
}