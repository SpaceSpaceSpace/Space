using UnityEngine;
using UnityEditor;
using System.Collections;

public class ArtilleryWizard : WizardMaster
{	
	/*public int ProjectilesPerShot = 1;
	public float ProjectileSpeed = 15;
	public float ProjectileLifetime = 4;
	
	public float ChargePowerRatio = 1;
	public float Cooldown = 2.0f;
	
	public float AttackPower = 10;
	public float ShieldPiercing = 1;
	public float Knockback = 1;
	
	[Range (0, 100)]
	public float Accuracy = 95; // In percentage; 100 has no spread, 0 has 180 degree spread
	
	[MenuItem("Space/New/Weapon/Artillery")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard<ArtilleryWizard>("New Artillery", "Create");
	}
	
	void OnWizardCreate()
	{ 
		if(!ValidateInput())
			return;
		
		//Create game object with weapon info
		GameObject artilleryObject = new GameObject(Name);
		
		SpriteRenderer renderer = artilleryObject.AddComponent<SpriteRenderer>();
		renderer.sprite = WeaponImage;
		renderer.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/WepMat.mat");

		//Create weapon script and fill with data from wizard
		WeaponScript artilleryScript = artilleryObject.AddComponent<WeaponScript>();
		artilleryScript.projectilePrefab = ProjectilePrefab;
		artilleryScript.projectilesPerShot = ProjectilesPerShot;

		artilleryScript.attackPower = AttackPower;
		artilleryScript.shieldPiercing = ShieldPiercing;
		artilleryScript.maxSpreadAngle = SpaceUtility.Remap(Accuracy, 0, 100, 90, 0);
		
		artilleryScript.cooldown = Cooldown;
		artilleryScript.shotsBeforeCooldown = 1;
		
		//Save game object to prefab
		PrefabUtility.CreatePrefab(WeaponPath + Name + ".prefab", artilleryObject);
		
		//Delete game object from scene
		DestroyImmediate(artilleryObject);
	}

	protected override void ReopenWindow()
	{
		ArtilleryWizard newWindow = ScriptableWizard.DisplayWizard<ArtilleryWizard>("New Artillery", "Create");
		
		newWindow.Name = Name;
		
		newWindow.WeaponImage = WeaponImage;
		newWindow.ProjectilePrefab = ProjectilePrefab;
		newWindow.ProjectilesPerShot = ProjectilesPerShot;

		newWindow.ChargePowerRatio = ChargePowerRatio;
		newWindow.Cooldown = Cooldown;
		
		newWindow.AttackPower = AttackPower;
		newWindow.ShieldPiercing = ShieldPiercing;
		newWindow.Accuracy = Accuracy; 
	}*/
}
