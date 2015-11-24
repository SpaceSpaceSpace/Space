using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory
{
	private List<WeaponInfo> weapons;

	public Inventory()
	{
		weapons = new List<WeaponInfo>();
	}

	public List<WeaponInfo> Weapons
	{
		get { return weapons; }
	}

	public void AddWeapon( WeaponInfo weaponToAdd )
	{
		weapons.Add( weaponToAdd );
	}

	public void RemoveWeapon( WeaponInfo weaponToRemove )
	{
		weapons.Remove( weaponToRemove );
	}
}
