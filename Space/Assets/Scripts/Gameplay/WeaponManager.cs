using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{
	public enum Weapons
	{
		LASER_MACHINE_GUN,
		LASER_SNIPER,
		LASER_SHOTGUN,
		BEAM,
		MISSILE_LAUNCHER,
		MINE_LAUNCHER,
		NUM_WEAPONS
	}

	public GameObject[] weaponPrefabs;

	public GameObject GetWeaponPrefab( Weapons type )
	{
		if( (int)type < (int)Weapons.NUM_WEAPONS )
		{
			return weaponPrefabs[ (int)type ];
		}
		else
		{
			return weaponPrefabs[ (int)Weapons.LASER_MACHINE_GUN ];
		}
	}
}
