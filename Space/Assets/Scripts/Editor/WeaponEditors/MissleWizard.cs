using UnityEngine;
using UnityEditor;
using System.Collections;

public class MissleWizard : WizardMaster
{
	/*public float ProjectileSpeed = 10;
	public float ProjectileLifetime = 1;
	public int ProjectilesPerShot = 1;

	[Range (0,5)]
	public float RateOfFire = 1.0f;
	
	public float AttackPower = 10;
	public float ShieldPiercing = 2;
	public float DamageRadius = 2.0f; // in meters

	public float Cooldown = 0.8f;

	[Range (0, 100)]
	public float Accuracy = 90; // In percentage; 100 has no spread, 0 has 180 degree spread
	
	public float Knockback = 0.8f;

	[MenuItem("Space/New/Weapon/Missle")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard<MissleWizard>("New Missle", "Create");
	}
	
	void OnWizardCreate()
	{ 
		if(!ValidateInput())
			return;
		
		//Create game object with weapon info
		GameObject missleObject = new GameObject(Name);
		
		SpriteRenderer renderer = missleObject.AddComponent<SpriteRenderer>();
		renderer.sprite = WeaponImage;
		renderer.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/WepMat.mat");
		
		WeaponScript missleScript = missleObject.AddComponent<WeaponScript>();
		missleScript.projectilePrefab = ProjectilePrefab;

		missleScript.projectileSpeed = ProjectileSpeed;
		missleScript.projectileLifeTime = ProjectileLifetime;
		missleScript.projectilesPerShot = ProjectilesPerShot;

		missleScript.attackPower = AttackPower;
		missleScript.fireTime = RateOfFire;
		missleScript.shieldPiercing = ShieldPiercing;

		missleScript.cooldown = Cooldown;

		missleScript.maxSpreadAngle = SpaceUtility.Remap(Accuracy, 0, 100, 90, 0);

		missleScript.knockback = Knockback;

		//Save game object to prefab
		PrefabUtility.CreatePrefab(WeaponPath + Name + ".prefab", missleObject);
		
		//Delete game object from scene
		DestroyImmediate(missleObject);
	}
	
	protected override void ReopenWindow()
	{
		MissleWizard newWindow = ScriptableWizard.DisplayWizard<MissleWizard>("New Missle", "Create");
		
		newWindow.Name = Name;
		
		newWindow.WeaponImage = WeaponImage;
		newWindow.ProjectilePrefab = ProjectilePrefab;
		newWindow.ProjectilesPerShot = ProjectilesPerShot;

		newWindow.RateOfFire = RateOfFire;
		
		newWindow.AttackPower = ShieldPiercing;
		newWindow.ShieldPiercing = ShieldPiercing;
		newWindow.DamageRadius = DamageRadius;

		newWindow.Cooldown = Cooldown;

		newWindow.Accuracy = Accuracy;

		newWindow.Knockback = Knockback;
	}*/
}
