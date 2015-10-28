using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour {

	public GameObject GameOverUI;

	void Update()
	{
		if(GameMaster.CurrentGameState != GameState.GameOver)
			GameOverUI.SetActive(false);
		else
			GameOverUI.SetActive(true);
	}
}
