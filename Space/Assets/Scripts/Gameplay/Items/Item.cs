using UnityEngine;
using System.Collections;

public class Item {
	public bool owned;
	public int cost;
	public string name;
	private string type;
	private float projectileSpeed;
	private float attackPower;

	/*private float timeLength; 
	private float attackPower;
	private float shieldPiercing;
	private float spread;
	private float chargeLengthRatio;
	private float width;
	private float cooldown;
	private string targeting;
	*/
	public Item()
	{
		owned = false;
		cost = 0;
		name = "Default";
		type = "None";
		projectileSpeed = 0.0f;
	}

	public Item(bool o, int c, string n, string t, float p)
	{
		owned = o;
		cost = c;
		name = n;
		type = t;
		projectileSpeed = p;
	}
}
