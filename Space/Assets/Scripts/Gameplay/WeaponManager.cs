using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{
	public GameObject[] weaponPrefabs;

	public GameObject GetWeaponPrefab( WeaponScript.WeaponType type )
	{
		if( (int)type < (int)WeaponScript.WeaponType.NUM_WEAPONS )
		{
			return weaponPrefabs[ (int)type ];
		}
		else
		{
			return weaponPrefabs[ (int)WeaponScript.WeaponType.LASER_MACHINE_GUN ];
		}
	}
}
