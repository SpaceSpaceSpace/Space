using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponInfo {

	private string name;
	WeaponModifier.ModifierNames modifier; 
	public Dictionary<string,float> attributes;

	public WeaponInfo(WeaponManager.Weapons weaponType, WeaponModifier.ModifierNames p_Modifier)
	{
		GameObject prefab = GameMaster.WeaponMngr.GetWeaponPrefab( weaponType );
		WeaponModifier.GetModifiedName( p_Modifier, prefab.name, out name );
		modifier = p_Modifier;

		attributes = new Dictionary<string, float>();
	}

	public string Name
	{
		get{ return name; }
	}
	
	public GameObject SpawnWeapon()
	{
		return null;
	}

	public void AddAttribute( string key, float value )
	{
		attributes.Add( key, value );
	}
}
