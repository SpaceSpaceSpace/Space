using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponInfo {

	private string name;
	WeaponModifier.ModifierNames modifier; 
	Dictionary<string,float> attributes;

	public WeaponInfo(string p_Name,WeaponModifier.ModifierNames p_Modifier)
	{
		name = p_Name;
		modifier = p_Modifier;

		//Switch
	}

	public string Name
	{
		get{ return name;}
	}

	void CreateAttributeDictionary()
	{

	}
}
