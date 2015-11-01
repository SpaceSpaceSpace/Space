using UnityEngine;
using UnityEditor;
using System.Collections;

public class ShieldGeneratorWizard : WizardMaster
{
	/*public float ShieldAmount = 10.0f;
	
	public float Cooldown = 0.8f;
	
	public float LifeTime = 10.0f;
	
	[MenuItem("Space/New/Weapon/ShieldGenerator")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard<ShieldGeneratorWizard>("New ShieldGenerator", "Create");
	}
	
	void OnWizardCreate()
	{ 
		if(!ValidateInput())
			return;
		
		//Create game object with weapon info
		GameObject shieldGeneratorObject = new GameObject(Name);
		
		SpriteRenderer renderer = shieldGeneratorObject.AddComponent<SpriteRenderer>();
		renderer.sprite = WeaponImage;
		renderer.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/WepMat.mat");
		
		WeaponScript shieldGeneratorScript = shieldGeneratorObject.AddComponent<WeaponScript>();
		shieldGeneratorScript.projectilePrefab = ProjectilePrefab;

		shieldGeneratorScript.cooldown = Cooldown;
		
		//Save game object to prefab
		PrefabUtility.CreatePrefab(WeaponPath + Name + ".prefab", shieldGeneratorObject);
		
		//Delete game object from scene
		DestroyImmediate(shieldGeneratorObject);
	}
	
	protected override void ReopenWindow()
	{
		ShieldGeneratorWizard newWindow = ScriptableWizard.DisplayWizard<ShieldGeneratorWizard>("New ShieldGenerator", "Create");
		
		newWindow.Name = Name;
		
		newWindow.WeaponImage = WeaponImage;
		newWindow.ProjectilePrefab = ProjectilePrefab;

		newWindow.ShieldAmount = ShieldAmount;

		newWindow.Cooldown = Cooldown;

		newWindow.LifeTime = LifeTime;
	}*/
}
