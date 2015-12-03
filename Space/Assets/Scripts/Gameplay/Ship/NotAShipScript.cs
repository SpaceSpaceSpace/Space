using UnityEngine;
using System.Collections;

public class NotAShipScript : ShipScript {

	public Sprite DestoryedSprite;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected virtual void Die()
	{
		Instantiate(m_exploder, transform.position, Quaternion.identity);
		Destroy( gameObject );
	}
	
}
