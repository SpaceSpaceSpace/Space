using UnityEngine;
using System.Collections;

public class SpaceStation : InteractableScript {

	public PlayerShipScript playerShip;

	public override void OnInteract ()
	{
		UI_Manager.instance.DisplayBountyBoard (true);
	}
	 
	//NOTE - Contracts do not get removed when they're turned in
	public void TurnInContracts()
	{
		for (int i = 0; i < GameMaster.playerData.playerContracts.Count; i++) 
		{
			if(GameMaster.playerData.playerContracts[i].completed)
			{
				GameMaster.playerData.playerContracts.Remove(GameMaster.playerData.playerContracts[i]);
				i--;
				if(i < 0)
					i = 0;
			}
		}
		for (int i = 0; i < GameMaster.playerData.playerContracts.Count; i++) 
		{
			if(GameMaster.playerData.playerContracts[i].completed)
			{
				Debug.Log ("I should never happen, ever.");
			}
		}
	}
}
