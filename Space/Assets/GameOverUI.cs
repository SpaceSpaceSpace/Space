using UnityEngine;
using System.Collections;

public class GameOverUI : MonoBehaviour {
	
	public GameObject GameOverPanel;

	// Update is called once per frame
	void Update () 
	{
		if(PlayerShipScript.player.Alive == false)
			GameOverPanel.SetActive(true);
	}

}
