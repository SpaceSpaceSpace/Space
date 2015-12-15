using UnityEngine;
using System.Collections;


public class BattleMusic : MonoBehaviour {

    public GameObject musicManager;
	private bool exploring;
	private bool fighting;
    // Use this for initialization
    void Start()
    {
		exploring = true;
		fighting = false;
    }
	
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ship") && exploring)
        {
            musicManager.GetComponent<MusicManager>().TransitionToCombat();
			exploring = false;
			fighting = true;
            Debug.Log("Transition To Combat");
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ship") && exploring)
        {
            musicManager.GetComponent<MusicManager>().TransitionToCombat();
            exploring = false;
            fighting = true;
            Debug.Log("Transition To Combat");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        //may be throwing errors when you destroy all of the ships.
        if (fighting)
        {
            musicManager.GetComponent<MusicManager>().TransitionToExploration();
			exploring = true;
			fighting = false;
            Debug.Log("Transition To Environment");
        }
    }
    

    

}
