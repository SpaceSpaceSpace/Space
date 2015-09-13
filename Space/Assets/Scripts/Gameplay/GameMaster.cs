using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public static GameMaster master;

	void Awake ()
	{
		//There can be only one
		if(master == null)
		{
			DontDestroyOnLoad(gameObject);
			master = this;
		}
		else if(master != this)
		{
			Destroy(gameObject);
		}
	}

	void Update () {
		//Swap to Customization scene
		if(Input.GetKey(KeyCode.F2))
		{
			Application.LoadLevel("CustomizationTest");
		}
	}
}
