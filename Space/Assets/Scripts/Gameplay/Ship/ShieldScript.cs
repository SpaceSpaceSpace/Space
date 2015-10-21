using UnityEngine;
using System.Collections;

// Halo-style regenerating shield
public class ShieldScript : MonoBehaviour
{
	public float maxShieldAmount = 100.0f;	// Max health for the shield
	public float rechargeDelay = 5.0f;		// How long does it take for the shield to start regening after taking damage?
	public float rechargeRate = 2.0f;		// Amount of shield regen'd per second
	public float shieldShowDuration = 1.0f;	// How long does the shield show for when damaged?

	private float m_shieldAmount;
	private float m_rechargeTime;

	private SpriteRenderer m_spriteRenderer;
	private Color m_showingColor;
	private Color m_hiddenColor;
	private float m_shieldShowTime;
	private bool m_isPlayerShield;

	public float ShieldAmount
	{
		get { return m_shieldAmount; }
	}

	void Awake ()
	{
		m_showingColor = Color.white;
		m_hiddenColor = m_showingColor;
		m_hiddenColor.a = 0;

		m_isPlayerShield = false;
		m_shieldAmount = maxShieldAmount;
		m_rechargeTime = 0.0f;
		m_shieldShowTime = 0.0f;
		m_spriteRenderer = GetComponent<SpriteRenderer>();
		m_spriteRenderer.color = m_hiddenColor;
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

		if( m_shieldShowTime > 0 )
		{
			m_shieldShowTime -= Time.deltaTime;
		}

		m_spriteRenderer.color = Color.Lerp( m_showingColor, m_hiddenColor, 1 - m_shieldShowTime / shieldShowDuration );
	}

	// Applies damage to the shield, returns the remainder of the damage if the shield is depleted
	public float ApplyDamage( float damage )
	{
		m_rechargeTime = 0.0f;

		if( m_shieldAmount <= 0 )
		{
			return damage;
		}

		m_shieldAmount -= damage;

		ShowShield();

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

	private void ShowShield()
	{
		m_shieldShowTime = shieldShowDuration;
		m_spriteRenderer.color = m_showingColor;
	}
}
