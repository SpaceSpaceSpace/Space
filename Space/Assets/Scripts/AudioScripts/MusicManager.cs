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
    public AudioMixerSnapshot transitionToBattle;
    public AudioMixerSnapshot transitionFromBattle;
    public AudioMixerSnapshot outOfCombat2;
    public AudioMixerSnapshot inCombat2;
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
    void PlayTransitionIntoBattle()
    {
        transitionToBattle.TransitionTo(0f);
    }
	void PlayTransitionOutOfBattle()
	{
        transitionFromBattle.TransitionTo(0f);
	}
    public void TransitionToCombat()
    {
        PlayTransitionIntoBattle();
        if (Random.Range(0, 1) <= 0.5)
            inCombat.TransitionTo(m_TransitionIn * 2.0f);
        else
            inCombat2.TransitionTo(m_TransitionIn * 2.0f);
        //PlayTransitionClip();
    }
    public void TransitionToExploration()
    {
        PlayTransitionOutOfBattle();
        if (Random.Range(0, 2) <= 0.5)
            outOfCombat.TransitionTo(m_TransitionOut);
        else
            outOfCombat2.TransitionTo(m_TransitionOut);
    }
}
