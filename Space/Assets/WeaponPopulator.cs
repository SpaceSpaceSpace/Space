using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WeaponPopulator : MonoBehaviour {

    List<Sprite> WeaponList;

	// Use this for initialization
	void Start () {
        WeaponList = Resources.LoadAll<Sprite>("WeaponImages/").ToList<Sprite>();

        //For every weapon sprite, add a button to this game object
        foreach (Sprite s in WeaponList) {
            GameObject button = new GameObject();
            //button.AddComponent<Button>();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
