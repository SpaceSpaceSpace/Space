using UnityEngine;

// Targeting Behavior for aiming at a transform
// How this script is actually supposed to get that target is TBD
public class TargetTransform : TargetingBehavior
{
	public Transform targetTransform; // public for now for Inspector set-ablity
	//private Transform m_targetTransform;
	
	void Start ()
	{
		Init();
	}
	
	void Update ()
	{
		SetAimTarget();
	}
	
	protected override void SetAimTarget()
	{
		m_aimScript.SetTarget( targetTransform.position );
	}
}
