using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MenuMusic : MonoBehaviour {

    public AudioMixerSnapshot mainTheme;
    public float bpm = 128;


    private float m_TransitionIn;
    private float m_TransitionOut;
    private float m_QuarterNote;
	// Use this for initialization
	void Start () {
        m_QuarterNote = 60 / bpm;
        m_TransitionIn = m_QuarterNote;
        m_TransitionOut = m_QuarterNote * 32;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void TransitionToMenu()
    {
        mainTheme.TransitionTo(m_TransitionOut);
    }
}
