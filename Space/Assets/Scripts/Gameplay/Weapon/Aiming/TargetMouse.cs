using UnityEngine;

// Targeting Behavior for aiming at the mouse
public class TargetMouse : TargetingBehavior
{
	private float m_mouseZ;
	
	void Start ()
	{
		Init();
		
		m_mouseZ = -Camera.main.transform.position.z; // Pray this doesn't change
	}
	
	void Update ()
	{
		SetAimTarget();
	}
	
	protected override void SetAimTarget()
	{
		// Pretty standard Screen pos to World pos for the mouse position
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = m_mouseZ; 
		m_aimScript.SetTarget( Camera.main.ScreenToWorldPoint( mousePos ) );
	}
}
