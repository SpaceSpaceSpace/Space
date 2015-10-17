using UnityEngine;
using System.Collections.Generic;

public class InteractableScript : MonoBehaviour
{
	PlayerShipScript playerShip; 
	void Start()
	{
		playerShip = PlayerShipScript.player;
	}

	public void OnInteract()
	{
		//This will be a UI Menu for the Space Station in the future, will be used just to spawn contracts for now
		TurnInContracts ();
		CreateAndAcceptContract ();
	}

	public void CreateAndAcceptContract()
	{
		Contract newContract = new Contract ();
		playerShip.AcceptContract (newContract);
	}
	//NOTE - Contracts do not get removed when they're turned in
	public void TurnInContracts()
	{
		for (int i = 0; i < playerShip.playerContracts.Count; i++) 
		{
			if(playerShip.playerContracts[i].completed)
			{
				playerShip.playerContracts.Remove(playerShip.playerContracts[i]);
				i--;
				if(i < 0)
					i = 0;

				Debug.Log ("Contract Completed!");
			}
		}
		for (int i = 0; i < playerShip.playerContracts.Count; i++) 
		{
			if(playerShip.playerContracts[i].completed)
			{
				Debug.Log ("I should never happen, ever.");
			}
		}
	}
}
