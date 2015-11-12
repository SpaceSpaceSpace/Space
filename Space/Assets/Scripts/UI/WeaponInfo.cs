using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponInfo {

	private string name;
	private GameObject weaponPrefab;
	private WeaponModifier.ModifierNames modifier; 
	private Dictionary<string,string> attributes;

	public WeaponInfo(WeaponScript.WeaponType weaponType, WeaponModifier.ModifierNames p_Modifier)
	{
		GameObject prefab = GameMaster.WeaponMngr.GetWeaponPrefab( weaponType );
		weaponPrefab = prefab;
		WeaponModifier.GetModifiedName( p_Modifier, prefab.name, out name );
		modifier = p_Modifier;

		attributes = new Dictionary<string, string>();
	}

	public string Name
	{
		get{ return name; }
	}
	
	public GameObject SpawnWeapon()
	{
		return null;
	}

	public void AddAttribute( string key, string value )
	{
		attributes.Add( key, value );
	}
}
