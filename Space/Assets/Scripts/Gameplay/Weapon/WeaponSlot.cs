using UnityEngine;
using System.Collections;

public class WeaponSlot : MonoBehaviour
{
	private WeaponScript m_weapon;

	public WeaponScript Weapon
	{
		get { return m_weapon; }
	}

	public void Fire()
	{
		if( m_weapon != null )
		{
			m_weapon.Fire();
		}
	}

	public void OnRelease()
	{
		if( m_weapon != null )
		{
			m_weapon.OnRelease();
		}
	}

	public void SetWeapon( GameObject weapon )
	{
		RemoveWeapon();

		weapon.transform.position = transform.position;
		weapon.transform.rotation = transform.rotation;
		weapon.transform.SetParent( transform );

		m_weapon = weapon.GetComponent<WeaponScript>();
	}

	public void RemoveWeapon()
	{
		if( m_weapon != null )
		{
			GameMaster.playerData.playerInventory.AddWeapon( m_weapon.ToInfo() );
			EventManager.TriggerEvent( "RefreshCustomizeWeps" );
			Destroy( m_weapon.gameObject );
		}
	}
}
