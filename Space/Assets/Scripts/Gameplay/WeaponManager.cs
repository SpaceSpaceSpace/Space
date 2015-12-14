using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{
	public GameObject[] weaponPrefabs;
	public int[] costs;

	void Start()
	{
		costs = new int[ (int)WeaponScript.WeaponType.NUM_WEAPONS ];
		costs[(int)WeaponScript.WeaponType.LASER_MACHINE_GUN] = 25;
		costs[(int)WeaponScript.WeaponType.SNIPER] = 25;
		costs[(int)WeaponScript.WeaponType.SCATTER_SHOT] = 25;
		costs[(int)WeaponScript.WeaponType.BEAM] = 25;
		costs[(int)WeaponScript.WeaponType.MISSILE_LAUNCHER] = 50;
		costs[(int)WeaponScript.WeaponType.MINE_LAUNCHER] = 50;
	}


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

	public void GetModifierRangeForWeapon( WeaponScript.WeaponType weapon, out int start, out int end )
	{
		switch( weapon )
		{
		case WeaponScript.WeaponType.BEAM:
			start = (int)WeaponModifier.BEAM_WEP_START;
			end = (int)WeaponModifier.BEAM_WEP_END;
			break;
		case WeaponScript.WeaponType.LASER_MACHINE_GUN:
			start = (int)WeaponModifier.PROJ_WEP_START;
			end = (int)WeaponModifier.PROJ_WEP_END;
			break;
		case WeaponScript.WeaponType.MINE_LAUNCHER:
			start = (int)WeaponModifier.MINE_WEP_START;
			end = (int)WeaponModifier.MINE_WEP_END;
			break;
		case WeaponScript.WeaponType.MISSILE_LAUNCHER:
			start = (int)WeaponModifier.MISSILE_WEP_START;
			end = (int)WeaponModifier.MISSILE_WEP_END;
			break;
		case WeaponScript.WeaponType.SCATTER_SHOT:
			start = (int)WeaponModifier.SCATTER_WEP_START;
			end = (int)WeaponModifier.SCATTER_WEP_END;
			break;
		case WeaponScript.WeaponType.SNIPER:
			start = (int)WeaponModifier.PROJ_WEP_START;
			end = (int)WeaponModifier.PROJ_WEP_END;
			break;
		default:
			start = (int)WeaponModifier.PROJ_WEP_START;
			end = (int)WeaponModifier.PROJ_WEP_END;
			break;
		}
	}
}
