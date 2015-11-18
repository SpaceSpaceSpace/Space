using UnityEngine;
using System.Collections;


public class BattleMusic : MonoBehaviour {

    public GameObject musicManager;
    // Use this for initialization
    void Start()
    {

    }
	
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ship"))
        {
            musicManager.GetComponent<MusicManager>().TransitionToCombat();
            Debug.Log("Transition To Combat");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ship"))
        {
            musicManager.GetComponent<MusicManager>().TransitionToExploration();
            Debug.Log("Transition To Environment");
        }
    }

    

}
