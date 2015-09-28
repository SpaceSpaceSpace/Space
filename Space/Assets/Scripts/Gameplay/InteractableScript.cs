using UnityEngine;

public class InteractableScript : MonoBehaviour
{
	public PlayerShipScript playerShip; 

	void Start()
	{
		playerShip = GameObject.Find ("Player Ship").GetComponent<PlayerShipScript>();
	}
	public void OnInteract()
	{
		print( "this should do something" );
		//give contract script to player
		//print or display description of the quest
		playerShip.playerContracts.Add (new Contract());//Contract takes list of objectives (enemies at this point), list of rewards(gold?), a string description, and an image string URL
		print (playerShip.playerContracts);
		//This will be a UI Menu for the Space Station in the future, will be used just to spawn contracts for now
	}
}
