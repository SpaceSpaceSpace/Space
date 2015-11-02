using UnityEngine;
using System.Collections;

public class ProximityDetonator : MonoBehaviour
{
	public float boomBoomDelay = 1.0f;

	private MineProjectileScript m_mine;

	void Start ()
	{
		m_mine = transform.parent.GetComponent<MineProjectileScript>();
	}

	void OnTriggerEnter2D( Collider2D col )
	{
		if( col.tag == "Ship" && m_mine.CanBoom )
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
