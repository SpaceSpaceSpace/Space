using UnityEngine;
using UnityEditor;
using System.Collections;

public class BeamWizard : WizardMaster
{
	public float ProjectileSpeed = 10;
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
		ScriptableWizard.DisplayWizard<BeamWizard>("New Laser", "Create");
	}

	void OnWizardCreate()
	{ 
		if(!ValidateInput())
			return;

		//Create game object with weapon info
		GameObject beamObject = new GameObject(Name);

		SpriteRenderer renderer = beamObject.AddComponent<SpriteRenderer>();
		renderer.sprite = WeaponImage;
		renderer.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/WepMat.mat");

		WeaponScript beamScript = beamObject.AddComponent<WeaponScript>();
		beamScript.projectilePrefab = ProjectilePrefab;

		beamScript.projectileSpeed = ProjectileSpeed;
		beamScript.projectileLifeTime = ProjectileLifetime;

		beamScript.attackPower = AttackPower;
		beamScript.fireTime = RateOfFire;
		beamScript.shieldPiercing = ShieldPiercing;
		beamScript.maxSpreadAngle = SpaceUtility.Remap(Accuracy, 0, 100, 90, 0);

		//Save game object to prefab
		PrefabUtility.CreatePrefab(WeaponPath + Name + ".prefab", beamObject);

		//Delete game object from scene
		DestroyImmediate(beamObject);
	}

	protected override void ReopenWindow()
	{
		BeamWizard newWindow = ScriptableWizard.DisplayWizard<BeamWizard>("New Laser", "Create");

		newWindow.Name = Name;

		newWindow.WeaponImage = WeaponImage;
		newWindow.ProjectilePrefab = ProjectilePrefab;
	 	
		newWindow.ProjectileSpeed = ProjectileSpeed;
		newWindow.ProjectileLifetime = ProjectileLifetime;

		newWindow.RateOfFire = RateOfFire;
		newWindow.AttackPower = AttackPower;
		newWindow.ShieldPiercing = ShieldPiercing;
		newWindow.Accuracy = Accuracy; 
	}
}
