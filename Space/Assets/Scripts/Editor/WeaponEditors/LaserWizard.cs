using UnityEngine;
using UnityEditor;
using System.Collections;

public class LaserWizard : ScriptableWizard
{
	public string Name;

	public Sprite WeaponImage;
	public ProjectileScript ProjectilePrefab;

	public float ProjectileSpeed = 10;
	public float ProjectileLifetime = 1;

	[Range (0,5)]
	public float RateOfFire = 0.5f;

	public float AttackPower = 2;
	public float ShieldPiercing = 1;


	[Range (0, 100)]
	public float Accuracy = 90; // In percentage; 100 has no spread, 0 has 180 degree spread

	private static string LaserPath = "Assets/Resources/ShipPrefabs/Weapons/";

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
		PrefabUtility.CreatePrefab(LaserPath + Name + ".prefab", laserObject);

		//Delete game object from scene
		DestroyImmediate(laserObject);
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
		if(AssetDatabase.LoadAssetAtPath<GameObject>(LaserPath + Name + ".prefab"))
		{
			if(EditorUtility.DisplayDialog("Warning", "A prefab named " + Name + " already exists. Do you want to overwrite it?",
			                               "Yes I know what I'm doing", "No please don't"))
			{
				//Clear existing prefab
				FileUtil.DeleteFileOrDirectory(LaserPath + Name + ".prefab");
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
	}
}
