using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponInfo {

	private string name;
	private GameObject weaponPrefab;
	private WeaponModifier.ModifierNames modifier; 
	public Dictionary<string,string> attributes;

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

	public GameObject WeaponPrefab
	{
		get{ return weaponPrefab; }
	}

	public GameObject SpawnWeapon()
	{
		GameObject weaponGO = GameObject.Instantiate( weaponPrefab );
		WeaponScript ws = weaponGO.GetComponent<WeaponScript>();
		ws.SetModifier( modifier );
		return weaponGO;
	}

	public void AddAttribute( string key, string value )
	{
		attributes.Add( key, value );
	}
    public float GetCost()
    {
        string costOut = "";
        float costF = 0.0f;
        attributes.TryGetValue("Cost", out costOut);
        costF = float.Parse(costOut);
        return costF;
    }
}
