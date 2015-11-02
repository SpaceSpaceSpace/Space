using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
	public GameObject PlayerUIObject;
	public Image healthCicle;
	public Image shieldCicle;

	private PlayerShipScript playerShip;
	private ShieldScript m_playerShield;

	void Start ()
	{
		EventManager.AddEventListener( EventDefs.PLAYER_HEALTH_UPDATE, UpdateHealth );
		EventManager.AddEventListener( EventDefs.PLAYER_SHIELD_UPDATE, UpdateShield );

		playerShip = PlayerShipScript.player;
		healthCicle.fillAmount = playerShip.MaxHealth/100f;
		ShieldScript playerShield = playerShip.Shield;

		if( playerShield != null )
		{
			shieldCicle.fillAmount = playerShield.ShieldAmount.Remap(0f,playerShield.maxShieldAmount,0f,1f);
			m_playerShield = playerShield;
		}
		else
		{
			shieldCicle.gameObject.SetActive( false );
		}
	}

	//TODO Remove this
	void Update()
	{
		if(GameMaster.CurrentGameState != GameState.Flying)
			PlayerUIObject.SetActive(false);
		else
			PlayerUIObject.SetActive(true);
	}

	private void UpdateHealth()
	{
		healthCicle.fillAmount = playerShip.Health/100f;

		if(playerShip.Health <= 0)
			healthCicle.transform.parent.gameObject.SetActive(false);
	}

	private void UpdateShield()
	{
		shieldCicle.fillAmount = m_playerShield.ShieldAmount.Remap (0f, m_playerShield.maxShieldAmount, 0f, 1f);;
	}
}
