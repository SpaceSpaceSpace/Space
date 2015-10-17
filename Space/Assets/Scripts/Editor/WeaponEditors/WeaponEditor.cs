using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(WeaponScript))]
public class WeaponEditor : Editor {
	
	/*public override void OnInspectorGUI()
	{
		WeaponScript weapon = (WeaponScript)target;

		EditorGUILayout.LabelField("Projectile Settings");

		weapon.projectilePrefab = (ProjectileScript)EditorGUILayout.ObjectField("Projectile Prefab", weapon.projectilePrefab, typeof(ProjectileScript), true);
		ProjectileScript projectile = weapon.projectilePrefab;
		if(projectile)
		{
			weapon.projectileSpeed = EditorGUILayout.FloatField("Projectile Speed", weapon.projectileSpeed);

			projectile.stayAlive = EditorGUILayout.Toggle("Don't Destroy Projectile", projectile.stayAlive);
			if(!projectile.stayAlive)
				weapon.projectileLifeTime = EditorGUILayout.FloatField("Projectile LifeTime (seconds)", weapon.projectileLifeTime);
		}

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Weapon Settings");
		weapon.attackPower = EditorGUILayout.FloatField("Attack Power", weapon.attackPower);
		weapon.shieldPiercing = EditorGUILayout.FloatField("Shield Piercing", weapon.shieldPiercing);
		weapon.fireTime = EditorGUILayout.Slider("Rate Of Fire (shots per sec)", weapon.fireTime, 0, 5);

		weapon.cooldown = EditorGUILayout.FloatField("Cooldown (in seconds)", weapon.cooldown);
		weapon.shotsBeforeCooldown = EditorGUILayout.IntField("Shots before Cooldown", weapon.shotsBeforeCooldown);
		weapon.projectilesPerShot = EditorGUILayout.IntField("Projectiles per Shot", weapon.projectilesPerShot);
		weapon.shotsPerClip = EditorGUILayout.FloatField("Shots per Clip", weapon.shotsPerClip);
		weapon.maxReserveClips = EditorGUILayout.FloatField("Max Reserve Clips", weapon.maxReserveClips);
		weapon.knockback = EditorGUILayout.FloatField("Knockback", weapon.knockback);

		float accuracy = SpaceUtility.Remap(weapon.maxSpreadAngle, 90, 0, 0, 100);
		accuracy = EditorGUILayout.Slider("Accuracy (percentage)", accuracy, 0, 100);
		weapon.maxSpreadAngle = SpaceUtility.Remap(accuracy, 0, 100, 90, 0);

		if (GUI.changed)
			EditorUtility.SetDirty(target);
	}*/
}
