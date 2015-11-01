using UnityEngine;
using System.Collections;

public class ProximityDetonator : MonoBehaviour
{
	public float boomBoomDelay = 1.0f;

	private MineProjectileScript m_mine;
	private bool m_triggered = false;

	void Start ()
	{
		m_mine = transform.parent.GetComponent<MineProjectileScript>();
	}

	void OnTriggerEnter2D( Collider2D col )
	{
		TriggerDetonation( col.tag );
	}

	void OnTriggerStay2D( Collider2D col )
	{
		TriggerDetonation( col.tag );
	}

	private void TriggerDetonation( string tag )
	{
		if( m_triggered )
		{
			// Early return
			return;
		}
		if( tag == "Ship" && m_mine.CanBoom )
		{
			m_mine.TriggerDetonationAnim();
			StartCoroutine( DetonationDelay() );
		}
	}

	private IEnumerator DetonationDelay()
	{
		yield return new WaitForSeconds( boomBoomDelay );
		m_mine.Detonate();
	}
}
