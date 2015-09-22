using UnityEngine;
using UnityEditor;
using System.Collections;

public class RailGunWizard : ScriptableWizard
{
	public string Name;
	
	public Sprite WeaponImage;
	public ProjectileScript ProjectilePrefab;

	public float ProjectileSpeed = 15;
	public float ProjectileLifetime = 4;

	public float ChargePowerRatio = 1;
	public float Cooldown = 0.8f;

	public float AttackPower = 3;
	public float ShieldPiercing = 5;

	[Range (0, 100)]
	public float Accuracy = 90; // In percentage; 100 has no spread, 0 has 180 degree spread
	
	private static string RailPath = "Assets/Resources/ShipPrefabs/Weapons/";
	
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
		PrefabUtility.CreatePrefab(RailPath + Name + ".prefab", railObject);
		
		//Delete game object from scene
		DestroyImmediate(railObject);
	}
	
	void OnWizardUpdate()
	{
		//Check for missing info
		if(string.IsNullOrEmpty(Name))
		{
			errorString = "You cannot create a weapon with a blank name.";
			isValid = false;
		}
		else if(WeaponImage == null)
		{
			errorString = "Every weapon needs an image.";
			isValid = false;
		}
		else if(WeaponImage == null)
		{
			errorString = "Every weapon needs a projectile prefab.";
			isValid = false;
		}
		else
		{
			errorString = "";
			isValid = true;
		}
	}
	
	bool ValidateInput()
	{
		//Make sure the prefab doesn't exist already
		if(AssetDatabase.LoadAssetAtPath<GameObject>(RailPath + Name + ".prefab"))
		{
			if(EditorUtility.DisplayDialog("Warning", "A prefab named " + Name + " already exists. Do you want to overwrite it?",
			                               "Yes I know what I'm doing", "No please don't"))
			{
				//Clear existing prefab
				FileUtil.DeleteFileOrDirectory(RailPath + Name + ".prefab");
				return true;
			}
			else
			{
				ReopenWindow();
				return false;
			}
		}
		
		return true;
	}
	
	void ReopenWindow()
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
	}
}
