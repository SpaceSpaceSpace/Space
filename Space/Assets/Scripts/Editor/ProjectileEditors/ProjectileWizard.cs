using UnityEngine;
using UnityEditor;
using System.Collections;

public class ProjectileWizard : ScriptableWizard
{
	/*public string Name;
	
	public Sprite ProjectileImage;
	public float Speed = 10.0f;
	public bool StayAlive = false;
	public float LifeTime = 1.0f;
	
	private static string ProjectilePath = "Assets/Prefabs/Projectiles/";
	
	[MenuItem("Space/New/Projectile")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard<ProjectileWizard>("New Projectile", "Create");
	}
	
	void OnWizardCreate()
	{ 
		if(!ValidateInput())
			return;
		
		//Create game object with weapon info
		GameObject projectileObject = new GameObject(Name);
		
		SpriteRenderer renderer = projectileObject.AddComponent<SpriteRenderer>();
		renderer.sprite = ProjectileImage;
		renderer.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/WepMat.mat");
		
		ProjectileScript projectileScript = projectileObject.AddComponent<ProjectileScript>();
		projectileScript.speed = Speed;
		projectileScript.stayAlive = StayAlive;
		projectileScript.lifeTime = LifeTime;
		
		//Save game object to prefab
		PrefabUtility.CreatePrefab(ProjectilePath + Name + ".prefab", projectileObject);
		
		//Delete game object from scene
		DestroyImmediate(projectileObject);
	}
	
	void OnWizardUpdate()
	{
		//Don't allow blank names
		if(string.IsNullOrEmpty(Name))
		{
			errorString = "You cannot create a projectile with a blank name.";
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
		if(AssetDatabase.LoadAssetAtPath<GameObject>(ProjectilePath + Name + ".prefab"))
		{
			if(EditorUtility.DisplayDialog("Warning", "A prefab named " + Name + " already exists. Do you want to overwrite it?",
			                               "Yes I know what I'm doing", "No please don't"))
			{
				//Clear existing prefab
				FileUtil.DeleteFileOrDirectory(ProjectilePath + Name + ".prefab");
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
		ProjectileWizard newWindow = ScriptableWizard.DisplayWizard<ProjectileWizard>("New Laser", "Create");
		
		newWindow.Name = Name;
		
		newWindow.ProjectileImage = ProjectileImage;
		newWindow.Speed = Speed;
		newWindow.StayAlive = StayAlive;
		newWindow.LifeTime = LifeTime;
	}*/
}
