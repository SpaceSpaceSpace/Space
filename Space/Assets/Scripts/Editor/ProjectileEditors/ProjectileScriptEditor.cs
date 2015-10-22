using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ProjectileScript))]
public class ProjectileScriptEditor : Editor {

	/*public override void OnInspectorGUI()
	{
		ProjectileScript projectile = (ProjectileScript)target;

		projectile.speed = EditorGUILayout.FloatField("Speed", projectile.speed);
		projectile.stayAlive = EditorGUILayout.Toggle("Don't Destroy", projectile.stayAlive);

		if(!projectile.stayAlive)
		{
			projectile.lifeTime = EditorGUILayout.FloatField("Life Time (Seconds)", projectile.lifeTime);
		}
	}*/
}
