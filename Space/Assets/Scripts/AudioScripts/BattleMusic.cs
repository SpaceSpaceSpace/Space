using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class BattleMusic : MonoBehaviour {
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

    // Use this for initialization
    void Start()
    {
        m_QuarterNote = 60 / bpm;
        m_TransitionIn = m_QuarterNote;
        m_TransitionOut = m_QuarterNote * 32;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship"))
        {
            inCombat.TransitionTo(m_TransitionIn);
            //PlayTransitionClip();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ship"))
        {
            outOfCombat.TransitionTo(m_TransitionOut);
        }
    }

    void PlayTransitionClip()
    {
        int randClip = Random.Range(0, transitionInstruments.Length);
        transitionSource.clip = transitionInstruments[randClip];
        transitionSource.Play();
    }

}
