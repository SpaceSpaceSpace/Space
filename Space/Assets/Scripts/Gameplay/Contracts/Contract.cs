using UnityEngine;
using System.Collections;

public class Contract : MonoBehaviour {

	public bool completed;
	public GameObject[] objectives;
	public GameObject[] rewards;
	private string description;
	private string targetImage;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public string GetDescription()
	{
		return description;
	}
	public void SetDescription(string d)
	{
		description = d;
	}
}
