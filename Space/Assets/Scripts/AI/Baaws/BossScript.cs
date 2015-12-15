using UnityEngine;
using System.Collections;

public class BossScript : MonoBehaviour
{
	public GameObject turretContainer;
	public GameObject explosion;

	private int m_generatorCount;

	void Start ()
	{
		m_generatorCount = turretContainer.transform.childCount; // 1 generator per turret container
		EventManager.AddEventListener( "GeneratorDestroyed", OnGeneratorDestroyed );
	
	}
	
	private void OnGeneratorDestroyed()
	{
		m_generatorCount--;
		if( m_generatorCount <= 0 )
		{
			BoomBoom();
		}
	}

	private void BoomBoom()
	{
		for( int i = 0; i < 20; i++ )
		{
			StartCoroutine( SpawnExplosion() );
		}
	}

	private IEnumerator SpawnExplosion()
	{
		yield return new WaitForSeconds( Random.Range( 0.0f, 5.0f ) );
		Instantiate( explosion, Random.insideUnitCircle * 20, Quaternion.identity );
	}
}
