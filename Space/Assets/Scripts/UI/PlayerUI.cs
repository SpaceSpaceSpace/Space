using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
	public PlayerShipScript playerShip;
	public Slider healthBar;
	public Slider shieldBar;

	private ShieldScript m_playerShield;

	void Start ()
	{
		EventManager.AddEventListener( EventDefs.PLAYER_HEALTH_UPDATE, UpdateHealth );
		EventManager.AddEventListener( EventDefs.PLAYER_SHIELD_UPDATE, UpdateShield );

		healthBar.maxValue = playerShip.MaxHealth;
		healthBar.value = playerShip.Health;


		ShieldScript playerShield = playerShip.Shield;

		if( playerShield != null )
		{
			shieldBar.maxValue = playerShield.maxShieldAmount;
			shieldBar.value = playerShield.ShieldAmount;
			m_playerShield = playerShield;
		}
		else
		{
			shieldBar.gameObject.SetActive( false );
		}
	}

	private void UpdateHealth()
	{
		healthBar.value = playerShip.Health;
	}

	private void UpdateShield()
	{
		shieldBar.value = m_playerShield.ShieldAmount;
	}
}
