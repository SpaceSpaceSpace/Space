using UnityEngine;
using UnityEditor;
using System.Collections;

public class LaserEditor : ScriptableWizard
{
	public string Name;

	public Sprite WeaponImage;
	public ProjectileScript ProjectilePrefab;

	public float ProjectileSpeed;
	public float ProjectileLifetime;

	public float RateOfFire;
	public float AttackPower;
	public float ShieldPiercing;
	[Range (0, 100)]
	public float Accuracy; // In percentage; 100 has no spread, 0 has 180 degree spread

	private static string LaserPath = "Assets/Resources/ShipPrefabs/Weapons/";

	[MenuItem("Space/New/Weapon/Laser")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard<LaserEditor>("New Laser", "Create");
	}

	void OnWizardCreate()
	{
		//Validate input from the user first
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
		laserScript.maxSpreadAngle = SpaceUtility.Remap(Accuracy, 0, 100, 180, 0);

		//Save game object to prefab
		PrefabUtility.CreatePrefab(LaserPath + Name + ".prefab", laserObject);

		//Delete game object from scene
		DestroyImmediate(laserObject);
	}

	bool ValidateInput()
	{
		//Don't allow blank names
		if(string.IsNullOrEmpty(Name))
		{
			EditorUtility.DisplayDialog("Invalid Name!", "You cannot create a weapon with a blank name.", "Ok");
			return false;
		}

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
				return false;
			}
		}

		return true;
	}
}
