using UnityEngine;
using UnityEditor;
using System.Collections;

public class MineWizard : WizardMaster
{
	/*[Range (0,5)]
	public float RateOfFire = 1.0f;
	
	public float AttackPower = 10;
	public float ShieldPiercing = 2;
	public float DamageRadius = 2.0f; // in meters
	
	[MenuItem("Space/New/Weapon/Mine")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard<MineWizard>("New Mine", "Create");
	}
	
	void OnWizardCreate()
	{ 
		if(!ValidateInput())
			return;
		
		//Create game object with weapon info
		GameObject mineObject = new GameObject(Name);
		
		SpriteRenderer renderer = mineObject.AddComponent<SpriteRenderer>();
		renderer.sprite = WeaponImage;
		renderer.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/WepMat.mat");
		
		WeaponScript mineScript = mineObject.AddComponent<WeaponScript>();
		mineScript.projectilePrefab = ProjectilePrefab;
		
		mineScript.projectileSpeed = 0;
		
		mineScript.attackPower = AttackPower;
		mineScript.fireTime = RateOfFire;
		mineScript.shieldPiercing = ShieldPiercing;
		
		//Save game object to prefab
		PrefabUtility.CreatePrefab(WeaponPath + Name + ".prefab", mineObject);
		
		//Delete game object from scene
		DestroyImmediate(mineObject);
	}
	
	protected override void ReopenWindow()
	{
		MineWizard newWindow = ScriptableWizard.DisplayWizard<MineWizard>("New Mine", "Create");
		
		newWindow.Name = Name;
		
		newWindow.WeaponImage = WeaponImage;
		newWindow.ProjectilePrefab = ProjectilePrefab;
		
		newWindow.RateOfFire = RateOfFire;
		
		newWindow.AttackPower = ShieldPiercing;
		newWindow.ShieldPiercing = ShieldPiercing;
		newWindow.DamageRadius = DamageRadius;
	}*/
}
