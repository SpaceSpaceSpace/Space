using UnityEngine;
using UnityEditor;
using System.Collections;

public class RailGunWizard : WizardMaster
{
	/*public float ProjectileSpeed = 15;
	public float ProjectileLifetime = 4;

	public float ChargePowerRatio = 1;
	public float Cooldown = 0.8f;

	public float AttackPower = 3;
	public float ShieldPiercing = 5;

	[Range (0, 100)]
	public float Accuracy = 90; // In percentage; 100 has no spread, 0 has 180 degree spread
	
	[MenuItem("Space/New/Weapon/Rail Gun")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard<RailGunWizard>("New Rail Gun", "Create");
	}
	
	void OnWizardCreate()
	{ 
		if(!ValidateInput())
			return;
		
		//Create game object with weapon info
		GameObject railObject = new GameObject(Name);
		
		SpriteRenderer renderer = railObject.AddComponent<SpriteRenderer>();
		renderer.sprite = WeaponImage;
		renderer.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/WepMat.mat");
		
		WeaponScript railScript = railObject.AddComponent<WeaponScript>();
		railScript.projectilePrefab = ProjectilePrefab;
		
		railScript.attackPower = AttackPower;
		railScript.shieldPiercing = ShieldPiercing;
		railScript.maxSpreadAngle = SpaceUtility.Remap(Accuracy, 0, 100, 90, 0);

		railScript.cooldown = Cooldown;
		railScript.shotsBeforeCooldown = 1;
		
		//Save game object to prefab
		PrefabUtility.CreatePrefab(WeaponPath + Name + ".prefab", railObject);
		
		//Delete game object from scene
		DestroyImmediate(railObject);
	}
	
	protected override void ReopenWindow()
	{
		RailGunWizard newWindow = ScriptableWizard.DisplayWizard<RailGunWizard>("New Rail Gun", "Create");
		
		newWindow.Name = Name;
		
		newWindow.WeaponImage = WeaponImage;
		newWindow.ProjectilePrefab = ProjectilePrefab;

		newWindow.ChargePowerRatio = ChargePowerRatio;
		newWindow.Cooldown = Cooldown;

		newWindow.AttackPower = AttackPower;
		newWindow.ShieldPiercing = ShieldPiercing;
		newWindow.Accuracy = Accuracy; 
	}*/
}
