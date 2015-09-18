using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(WeaponScript))]
public class LaserEditor : Editor {
	
	public override void OnInspectorGUI()
	{
		WeaponScript weapon = (WeaponScript)target;

		EditorGUILayout.LabelField("Projectile Settings");
		weapon.projectilePrefab = (ProjectileScript)EditorGUILayout.ObjectField("Projectile Prefab", weapon.projectilePrefab, typeof(ProjectileScript), false);
		weapon.projectileSpeed = EditorGUILayout.FloatField("Projectile Speed", weapon.projectileSpeed);
		weapon.projectilePrefab.stayAlive = EditorGUILayout.Toggle("Don't Destroy Projectile", weapon.projectilePrefab.stayAlive);
		if(!weapon.projectilePrefab.stayAlive)
			weapon.projectileLifeTime = EditorGUILayout.FloatField("Projectile LifeTime (seconds)", weapon.projectileLifeTime);

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Weapon Settings");
		weapon.attackPower = EditorGUILayout.FloatField("Attack Power", weapon.attackPower);
		weapon.shieldPiercing = EditorGUILayout.FloatField("Shield Piercing", weapon.shieldPiercing);
		weapon.fireTime = EditorGUILayout.FloatField("Rate Of Fire (shots per second)", weapon.fireTime);

		float accuracy = SpaceUtility.Remap(weapon.maxSpreadAngle, 90, 0, 0, 100);
		accuracy = EditorGUILayout.Slider("Accuracy (percentage)", accuracy, 0, 100);
		weapon.maxSpreadAngle = SpaceUtility.Remap(accuracy, 0, 100, 90, 0);
	}
}
