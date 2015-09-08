using UnityEngine;

[RequireComponent ( typeof ( AimScript ) ) ]
[RequireComponent ( typeof ( WeaponScript ) ) ]

// Abstract base class for telling a weapon how to aim
public abstract class TargetingBehavior : MonoBehaviour
{
	// Members
	protected AimScript m_aimScript;
	protected WeaponScript m_weaponScript;
	
	// Methods
	protected void Init()
	{
		m_aimScript = GetComponent<AimScript>();
		m_weaponScript = GetComponent<WeaponScript>();
	}
	
	protected abstract void SetAimTarget();
}
