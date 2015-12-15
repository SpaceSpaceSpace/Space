using UnityEngine;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour
{
	
	public List<Contract> playerContracts = new List<Contract>();
	public Inventory playerInventory = new Inventory ();
	public PlayerShipScript player;
	public int playerMoney = 25;

	//Accepts contract and spawns the objective in world space
	public void AcceptContract(Contract contract)
	{
		playerContracts.Add (contract);
		contract.SpawnContract ();
	}
}
