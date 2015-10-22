using UnityEngine;
using System.Collections;

public class SplitSatellite : Satellite {
	private static GameObject m_dustplosion = null;
	// Use this for initialization
	public void Start () {
		float semiMajor;
		if (radius.x > radius.y)
			semiMajor = radius.x;
		else
			semiMajor = radius.y;
		//Load dustplosion
		if(m_dustplosion == null)
			m_dustplosion = Resources.Load("AsteroidExplosion") as GameObject;
	}
	

}
