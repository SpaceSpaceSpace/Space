using UnityEngine;
using System.Collections;

public class SplitSatellite : Satellite {
	private static GameObject m_dustplosion = null;
	// Use this for initialization
	public void Start () {
		
		//Load dustplosion
		if(m_dustplosion == null)
			m_dustplosion = Resources.Load("AsteroidExplosion") as GameObject;
	}
	

}
