using UnityEngine;
using UnityEditor;
using System.Collections;

public class BeamWizard : WizardMaster
{
	/*public float ProjectileSpeed = 10;
	
	public float RateOfDamage = 0.5f;
	public float AttackPower = 0.4f;
	public float ShieldPiercing = 0.6f;

	public float LengthOfShotTilCooldown = 2.0f;
	public float Cooldown = 0.3f;

	public float BeamWidth = 0.4f;
	
	[MenuItem("Space/New/Weapon/Beam")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard<BeamWizard>("New Beam", "Create");
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
		
		beamScript.attackPower = AttackPower;
		beamScript.shieldPiercing = ShieldPiercing;
		
		//Save game object to prefab
		PrefabUtility.CreatePrefab(WeaponPath + Name + ".prefab", beamObject);
		
		//Delete game object from scene
		DestroyImmediate(beamObject);
	}
	
	protected override void ReopenWindow()
	{
		BeamWizard newWindow = ScriptableWizard.DisplayWizard<BeamWizard>("New Beam", "Create");
		
		newWindow.Name = Name;
		
		newWindow.WeaponImage = WeaponImage;
		newWindow.ProjectilePrefab = ProjectilePrefab;
		
		newWindow.ProjectileSpeed = ProjectileSpeed;

		newWindow.RateOfDamage = RateOfDamage;
		newWindow.AttackPower = AttackPower;
		newWindow.ShieldPiercing = ShieldPiercing;
		
		newWindow.LengthOfShotTilCooldown = LengthOfShotTilCooldown;
		newWindow.Cooldown = Cooldown;
		
		newWindow.BeamWidth = BeamWidth;
	}*/
}
