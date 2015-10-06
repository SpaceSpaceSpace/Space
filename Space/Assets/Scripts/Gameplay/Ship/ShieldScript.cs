using UnityEngine;
using System.Collections;

// Halo-style regenerating shield
public class ShieldScript : MonoBehaviour
{
	public float maxShieldAmount = 100.0f;	// Max health for the shield
	public float rechargeDelay = 5.0f;		// How long does it take for the shield to start regening after taking damage?
	public float rechargeRate = 2.0f;		// Amount of shield regen'd per second

	private float m_shieldAmount;
	private float m_rechargeTime;

	private bool m_isPlayerShield;

	public float ShieldAmount
	{
		get { return m_shieldAmount; }
	}

	void Awake ()
	{
		m_isPlayerShield = false;
		m_shieldAmount = maxShieldAmount;
		m_rechargeTime = 0.0f;
	}

	void Update ()
	{
		if( m_shieldAmount == maxShieldAmount )
		{
			// Early return
			return;
		}

		if( m_rechargeTime >= rechargeDelay )
		{
			m_shieldAmount = Mathf.Min( maxShieldAmount, m_shieldAmount + rechargeRate * Time.deltaTime );
			if( m_isPlayerShield )
			{
				EventManager.TriggerEvent( EventDefs.PLAYER_SHIELD_UPDATE );
			}
		}
		else
		{
			m_rechargeTime += Time.deltaTime;
		}
	}

	// Applies damage to the shield, returns the remainder of the damage if the shield is depleted
	public float ApplyDamage( float damage )
	{
		m_shieldAmount -= damage;
		m_rechargeTime = 0.0f;

		if( m_isPlayerShield )
		{
			EventManager.TriggerEvent( EventDefs.PLAYER_SHIELD_UPDATE );
		}

		if( m_shieldAmount < 0.0f )
		{
			float remainder = -m_shieldAmount;
			m_shieldAmount = 0.0f;
			return remainder;
		}

		return 0;
	}

	public void SetAsPlayerShield()
	{
		m_isPlayerShield = true;
	}
}
