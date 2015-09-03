using UnityEngine;
using System.Collections;

public class Customize : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Ship ship = GetComponent<Ship>();

		//For each empty attachment point on the ship, draw an indicator that you can click to change the attachements
		foreach(Vector2 v in ship.AttachmentPoints){
			GameObject indicator = Instantiate((GameObject)Resources.Load("ShipPrefabs/AttachmentIndicator"));

			Vector3 pos = v;
			pos.z = ship.transform.position.z - .1f;

			indicator.transform.position = pos;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI() {
        
    }
}
