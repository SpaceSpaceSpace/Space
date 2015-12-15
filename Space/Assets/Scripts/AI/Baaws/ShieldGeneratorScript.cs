using UnityEngine;
using System.Collections;

public class ShieldGeneratorScript : ShipScript
{
	public Sprite destroyedSprite;

	void Start()
	{
		InitShip();
	}

	protected override void Die()
	{
		EventManager.TriggerEvent( "GeneratorDestroyed" );

		GetComponent<SpriteRenderer>().sprite = destroyedSprite;
		GetComponent<CircleCollider2D>().enabled = false;
		Instantiate(m_exploder, transform.position, Quaternion.identity);
	}
}
