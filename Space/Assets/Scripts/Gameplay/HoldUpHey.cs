using UnityEngine;
using System.Collections;

public class HoldUpHey : MonoBehaviour {

	private bool paused;
	private bool cHelp;
	private bool help;
	void Start()
	{
		paused = false;
		cHelp = false;
		help = false;
	}

	void Update()
	{
		if(GameMaster.CurrentGameState == GameState.Pause && !paused) 
		{ 
			Debug.Log("PAUSED");
			//UI_Manager.instance.DisplayPauseScreen(true); 
			paused = true;
		}
		if(GameMaster.CurrentGameState != GameState.Pause && paused)
		{
			paused = false;
			Debug.Log("UNPAUSED");
		}
		if(GameMaster.CurrentGameState == GameState.H && !help) 
		{ 
			Debug.Log("HELP");
			//UI_Manager.instance.DisplayGeneralHelp(true); 
			help = true;
		}
		if(GameMaster.CurrentGameState != GameState.H && help)
		{
			help = false;
			Debug.Log("UNHELP");
		}
		if(GameMaster.CurrentGameState == GameState.CHelp && !cHelp) 
		{ 
			Debug.Log("CustomizeHelp");
			//UI_Manager.instance.DisplayCustomizeHelp(true); 
			cHelp = true;
		}
		if(GameMaster.CurrentGameState != GameState.CHelp && cHelp)
		{
			cHelp = false;
			Debug.Log("UNCustomizeHelp");
		}

	}
}
