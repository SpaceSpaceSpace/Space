using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof ( Rigidbody2D ) ) ]
[RequireComponent ( typeof ( ThrustScript ) ) ]

// I'm sailing away...
public class ShipScript : MonoBehaviour
{
	protected ThrustScript m_thrust;
	protected WeaponScript[] m_weapons;
	
	// Basically the Start method of the script,
	// since the Start of a base class script will not be called
	protected void InitShip()
	{
		m_thrust = GetComponent<ThrustScript>();
		InitWeapons();
	}

	// Checks which weapons are attached and loads them into m_weapons
	protected void InitWeapons()
	{
		//Loop through children and see which ones are weapons
		List<WeaponScript> tempWeapons = new List<WeaponScript>();

		for( int i = 0; i < transform.childCount; i++ )
		{
			WeaponScript weapon = transform.GetChild( i ).GetComponent<WeaponScript>();

			if(weapon != null)
				tempWeapons.Add(weapon);			
		}

		m_weapons = tempWeapons.ToArray();
	}

	protected void FireWeapons()
	{
		for( int i = 0; i < m_weapons.Length; i++ )
		{			
			m_weapons[ i ].Fire();
		}
	}
	
	protected void ReleaseFire()
	{
		for( int i = 0; i < m_weapons.Length; i++ )
		{
			m_weapons[ i ].OnRelease();
		}
	}
}
