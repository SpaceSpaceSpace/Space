using UnityEngine;

public class InteractScript : MonoBehaviour
{
	public const string INTERACTABLE_TAG = "Interactable";
	public GameObject interactText;
	
	private InteractableScript m_targetInteractible;
	private Customize m_customization;
	
	void Update()
	{
		if( Input.GetButtonDown( "Interact" ) && GameMaster.CurrentGameState == GameState.Flying)
		{
			if( m_targetInteractible != null )
			{
				m_targetInteractible.OnInteract();
			}
		}

		if (Input.GetButtonDown ("Warp") && GameMaster.CurrentGameState == GameState.Flying) 
		{
			GameObject warpMngr = (GameObject)GameObject.Find("Warp Manager");
			WarpScript warpScript = warpMngr.GetComponent<WarpScript>();

			warpScript.CallHangar();
		}
		if (Input.GetButtonDown ("Pause") && GameMaster.CurrentGameState != GameState.Pause) 
		{
			//pause the game

			GameMaster.CurrentGameState = GameState.Pause;
			Time.timeScale = 0.0f;
		}
		if (Input.GetKeyDown(KeyCode.Escape) && GameMaster.CurrentGameState == GameState.Pause) 
		{
			//unpause the game
			GameMaster.CurrentGameState = GameState.Flying;
			Time.timeScale = 1.0f;
		}
		if (Input.GetButtonDown ("Help") && GameMaster.CurrentGameState == GameState.Customization) 
		{
			//show the customization help
			GameMaster.CurrentGameState = GameState.CHelp;
			Time.timeScale = 0.0f;
		}
		if (Input.GetButtonDown ("Help") && GameMaster.CurrentGameState == GameState.Flying) 
		{
			//show the general help
			GameMaster.CurrentGameState = GameState.H;
			Time.timeScale = 0.0f;
		}
		if ( Input.GetKeyDown(KeyCode.Escape) && GameMaster.CurrentGameState == GameState.H) 
		{
			//return the game from a help screen to gameplay
			GameMaster.CurrentGameState = GameState.Flying;
			Time.timeScale = 1.0f;
		}
		if ( Input.GetKeyDown(KeyCode.Escape) && GameMaster.CurrentGameState == GameState.CHelp) 
		{
			//return the game from a help screen to gameplay
			GameMaster.CurrentGameState = GameState.Flying;
			Time.timeScale = 1.0f;
		}
		//TEMPORARY -- This functionality will be replaced with the Space Station UI
		if(Input.GetKeyDown (KeyCode.E) && GameMaster.CurrentGameState != GameState.Customization)
		{
			if( m_targetInteractible != null )
			{
				GameMaster.CurrentGameState = GameState.Customization;
				interactText.SetActive(false);
			}
		}
		if(Input.GetKeyDown (KeyCode.Escape) && GameMaster.CurrentGameState == GameState.Customization)
		{
			if( m_targetInteractible != null )
			{
				GameMaster.CurrentGameState = GameState.Flying;
				interactText.SetActive(true);
			}
		}
	}
	
	void OnTriggerEnter2D( Collider2D col )
	{
		if( col.tag == INTERACTABLE_TAG )
		{
			// Temp ========================
			if( m_targetInteractible != null )
			{
				SpriteRenderer otherSr = m_targetInteractible.GetComponent<SpriteRenderer>();
				otherSr.color = Color.gray;
			}
			SpriteRenderer sr = col.GetComponent<SpriteRenderer>();
			sr.color = Color.green;
			// =============================
			
			m_targetInteractible = col.GetComponent<InteractableScript>();
			interactText.SetActive(true);
		}
	}
	
	// For cases where a larger collider hasn't been exited
	// and a smaller, overlapping collider has been entered and exited
	void OnTriggerStay2D( Collider2D col )
	{
		if( m_targetInteractible != null )
		{
			// Early return
			return;
		}
		
		if( col.tag == INTERACTABLE_TAG )
		{
			// Temp ========================
			SpriteRenderer sr = col.GetComponent<SpriteRenderer>();
			sr.color = Color.green;
			// =============================

			if(GameMaster.CurrentGameState == GameState.Customization)
				interactText.SetActive(false);
			
			m_targetInteractible = col.GetComponent<InteractableScript>();
		}
	}
	
	void OnTriggerExit2D( Collider2D col )
	{
		if( col.tag == INTERACTABLE_TAG )
		{
			InteractableScript interactible = col.GetComponent<InteractableScript>();
			if( interactible == m_targetInteractible )
			{
				m_targetInteractible = null;
				
				// Temp ========================
				SpriteRenderer sr = col.GetComponent<SpriteRenderer>();
				sr.color = Color.gray;
				// =============================
				interactText.SetActive(false);
			}
		}
	}
}
