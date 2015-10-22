using UnityEngine;
using UnityEditor;
using System.Collections;

public class LaserWizard : WizardMaster
{
	/*public float ProjectileSpeed = 10;
	public float ProjectileLifetime = 1;

	[Range (0,5)]
	public float RateOfFire = 0.5f;

	public float AttackPower = 2;
	public float ShieldPiercing = 1;
	
	[Range (0, 100)]
	public float Accuracy = 90; // In percentage; 100 has no spread, 0 has 180 degree spread

	[MenuItem("Space/New/Weapon/Laser")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard<LaserWizard>("New Laser", "Create");
	}

	void OnWizardCreate()
	{ 
		if(!ValidateInput())
			return;

		//Create game object with weapon info
		GameObject laserObject = new GameObject(Name);

		SpriteRenderer renderer = laserObject.AddComponent<SpriteRenderer>();
		renderer.sprite = WeaponImage;
		renderer.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/WepMat.mat");

		WeaponScript laserScript = laserObject.AddComponent<WeaponScript>();
		laserScript.projectilePrefab = ProjectilePrefab;

		laserScript.projectileSpeed = ProjectileSpeed;
		laserScript.projectileLifeTime = ProjectileLifetime;

		laserScript.attackPower = AttackPower;
		laserScript.fireTime = RateOfFire;
		laserScript.shieldPiercing = ShieldPiercing;
		laserScript.maxSpreadAngle = SpaceUtility.Remap(Accuracy, 0, 100, 90, 0);

		//Save game object to prefab
		PrefabUtility.CreatePrefab(WeaponPath + Name + ".prefab", laserObject);

		//Delete game object from scene
		DestroyImmediate(laserObject);
	}

	protected override void ReopenWindow()
	{
		LaserWizard newWindow = ScriptableWizard.DisplayWizard<LaserWizard>("New Laser", "Create");

		newWindow.Name = Name;

		newWindow.WeaponImage = WeaponImage;
		newWindow.ProjectilePrefab = ProjectilePrefab;
	 	
		newWindow.ProjectileSpeed = ProjectileSpeed;
		newWindow.ProjectileLifetime = ProjectileLifetime;

		newWindow.RateOfFire = RateOfFire;
		newWindow.AttackPower = AttackPower;
		newWindow.ShieldPiercing = ShieldPiercing;
		newWindow.Accuracy = Accuracy; 
	}*/
}
