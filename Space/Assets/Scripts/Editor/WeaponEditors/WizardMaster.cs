using UnityEngine;
using UnityEditor;
using System.Collections;

public class WizardMaster : ScriptableWizard
{
	public static string WeaponPath = "Assets/Resources/ShipPrefabs/Weapons/";

	//Every weapon needs a name, image and projectile
	public string Name;
	public Sprite WeaponImage;
	public ProjectileScript ProjectilePrefab;

	public virtual void OnWizardUpdate()
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
		else if(ProjectilePrefab == null)
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
	
	protected bool ValidateInput()
	{
		//Make sure the prefab doesn't exist already
		if(AssetDatabase.LoadAssetAtPath<GameObject>(WeaponPath + Name + ".prefab"))
		{
			if(EditorUtility.DisplayDialog("Warning", "A prefab named " + Name + " already exists. Do you want to overwrite it?",
			                               "Yes I know what I'm doing", "No please don't"))
			{
				//Clear existing prefab
				FileUtil.DeleteFileOrDirectory(WeaponPath + Name + ".prefab");
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

	protected virtual void ReopenWindow(){}
}

