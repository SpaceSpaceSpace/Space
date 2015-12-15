using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Manager : MonoBehaviour
{

    public static UI_Manager instance;
    public GameObject bountyBoard;
    public GameObject storeBoard;
    public GameObject gameOverScreen;
	public GameObject pauseUI;
    public GameObject spaceStationUI;
	public GameObject hangerUI;
	public GameObject weaponToggles;
	public GameObject pauseScreen;
	public GameObject generalHelp;
	public GameObject customizationHelp;
	public PlayerShipScript player;
	public WeaponDock weaponDockUI;

	public ObjectivesUIController objectivesUIController;

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
        spaceStationUI.SetActive(active);

        if (active)
        {
			GameMaster.CurrentGameState = GameState.Station;
            player.Dock();
			player.transform.position = spaceStationObject.transform.position;
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			Camera.main.rect = new Rect (0f, 0f, 1f, 1f);
			otherGameCamera.rect = new Rect (0f, 0f, 1f, 1f);
        }
        else
        {
			GameMaster.CurrentGameState = GameState.Flying;
            player.Undock();
			Camera.main.rect = new Rect (0f, .2f, 1f, 1f);
			otherGameCamera.rect = new Rect (0f, .2f, 1f, 1f);
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
	public void DisplayPauseScreen(bool active)
	{
		pauseUI.SetActive (active);

		if(!active)
		{
			Time.timeScale = 1.0f;
			GameMaster.CurrentGameState = GameState.Flying;

		}
	}
	public void DisplayGeneralHelp(bool active)
	{
		
	}
	public void DisplayCustomizationHelp(bool active)
	{
		
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
				Camera.main.rect = new Rect (0f, 0f, 1f, 1f);
				otherGameCamera.rect = new Rect (0f, 0f, 1f, 1f);
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

	public void PopulateObjectiveUI(List<ObjectiveEvent> objectiveEvents)
	{
		objectivesUIController.PopulateUIObjectives (objectiveEvents);
	}

    public void SetAllScreensToInactive()
    {
        bountyBoard.SetActive(false);
        gameOverScreen.SetActive(false);
        storeBoard.SetActive(false);
        spaceStationUI.SetActive(false);
		pauseUI.SetActive (false);
    }
}