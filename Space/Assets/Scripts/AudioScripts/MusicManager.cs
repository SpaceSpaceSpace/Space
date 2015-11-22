using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {
    /* 
 *  Based on CombatMusicControl found in the Unity Tutorial: 
 *  Audio/Adding Music To Your Game 
 * 
 * 
 */


    public AudioMixerSnapshot outOfCombat;
    public AudioMixerSnapshot inCombat;
    public AudioClip[] transitionInstruments;
    public AudioSource transitionSource;
    public float bpm = 128;


    private float m_TransitionIn;
    private float m_TransitionOut;
    private float m_QuarterNote;
    void Start()
    {
        m_QuarterNote = 60 / bpm;
        m_TransitionIn = m_QuarterNote;
        m_TransitionOut = m_QuarterNote * 32;

    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void PlayTransitionClip()
    {
        int randClip = Random.Range(0, transitionInstruments.Length);
        transitionSource.clip = transitionInstruments[randClip];
        transitionSource.Play();
    }
    public void TransitionToCombat()
    {
        inCombat.TransitionTo(m_TransitionIn * 2.0f);
        //PlayTransitionClip();
    }
    public void TransitionToExploration()
    {
        outOfCombat.TransitionTo(m_TransitionOut);
    }
}
