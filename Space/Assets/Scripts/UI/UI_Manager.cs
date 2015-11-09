using UnityEngine;
using System.Collections;

public class UI_Manager : MonoBehaviour
{

    public static UI_Manager instance;
    public GameObject bountyBoard;
    public GameObject storeBoard;
    public GameObject gameOverScreen;
    public GameObject spaceStationUI;
    public GameObject spaceStationObject;
    public PlayerShipScript player;

    // Use this for initialization
    void Start()
    {
        instance = this;
		spaceStationObject = GameObject.Find ("SpaceStore");
        SetAllScreensToInactive();
    }

    public void DisplaySpaceStationUI(bool active)
    {
        spaceStationUI.SetActive(active);

        if (active)
        {
            player.Dock();
			player.transform.position = spaceStationObject.transform.position;
			player.transform.SetParent(spaceStationUI.transform);
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        else
        {
            player.Undock();
			player.transform.SetParent(null);
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
            player.Undock();
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
            player.Undock();
        }
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

    public void SetAllScreensToInactive()
    {
        bountyBoard.SetActive(false);
        gameOverScreen.SetActive(false);
        storeBoard.SetActive(false);
        spaceStationUI.SetActive(false);
    }
}