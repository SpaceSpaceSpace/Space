using UnityEngine;
using System.Collections;

public class WeaponInfo {

	private string name;
	private PlayerShipScript player;

	public WeaponInfo()
	{
		name = "Unknown";
		player = GameObject.Find ("Player Ship").GetComponent<PlayerShipScript>();
	}
	public WeaponInfo(string p_Name)
	{
		name = p_Name;
		player = GameObject.Find ("Player Ship").GetComponent<PlayerShipScript>();
	}

	public string Name
	{
		get{ return name;}
	}
	
	public PlayerShipScript Player
	{
		get{return player;}
	}
}
