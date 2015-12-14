// =====================================================
// =====!!! This file was generated. No touchy. !!!=====
// =====================================================
public struct WeaponModifier
{
	public enum ModifierNames
	{
		DEFAULT,
		Crappy,
		Godly,
		Fast,
		of_Doom_suffix,
		Abundant,
		Lacking,
		Big_Boom_Boom,
		Rapid,
		Focused,
		Cheap,
		Reaching,
		Burny,

		NUM_MODIFIERS
	}

	public enum Stats
	{
		DAMAGE,
		ACCURACY,
		FIRE_RATE,
		COST_MOD,

		NUM_STATS,

		BONUS_PROJECTILES = DAMAGE,
		MINE_SPEED = ACCURACY,
		BEAM_RANGE = ACCURACY,

	}

	public const int GENERIC_START = 0;
	public const int GENERIC_END = (int)ModifierNames.Godly + 1;
	public const int PROJ_WEP_START = GENERIC_END;
	public const int PROJ_WEP_END = (int)ModifierNames.of_Doom_suffix + 1;
	public const int SCATTER_WEP_START = PROJ_WEP_END;
	public const int SCATTER_WEP_END = (int)ModifierNames.Lacking + 1;
	public const int MISSILE_WEP_START = PROJ_WEP_START;
	public const int MISSILE_WEP_END = PROJ_WEP_END;
	public const int MINE_WEP_START = SCATTER_WEP_END;
	public const int MINE_WEP_END = (int)ModifierNames.Focused + 1;
	public const int BEAM_WEP_START = MINE_WEP_END;
	public const int BEAM_WEP_END = (int)ModifierNames.NUM_MODIFIERS;
	
	public static readonly float[,] modifiers = {
		{ 1f, 1f, 1f, 1f },
		{ 0.5f, 0.5f, 0.5f, 1.0f },
		{ 1.5f, 1.5f, 1.5f, 1.0f },
		{ 0.9f, 0.9f, 1.2f, 1.0f },
		{ 1.5f, 0.7f, 1f, 1.0f },
		{ 2f, 1f, 1f, 1.0f },
		{ 0.8f, 1f, 1f, 1.0f },
		{ 1.5f, 0.7f, 1f, 1.0f },
		{ 0.9f, 1f, 1.2f, 1.0f },
		{ 1f, 1.5f, 1f, 1.0f },
		{ 0.8f, 0.8f, 0f, 1.0f },
		{ 1f, 1.2f, 0f, 1f },
		{ 1.2f, 1f, 0f, 1.0f },

	};

	public static void GetModifiedName( ModifierNames modName, string weaponName, out string outputString )
	{
		if( modName == ModifierNames.DEFAULT )
		{
			outputString = weaponName;
			return;
		}
		
		string name = modName.ToString();
		bool isPrefix = ( name.IndexOf( "_suffix" ) == -1 );
		name = name.Replace( "_", " " );

		if( isPrefix )
		{
			outputString = name + " " + weaponName;
		}
		else
		{
			name = name.Replace( " suffix", "" );
			outputString = weaponName + " " + name;
		}
	}
	
	public static float GetModifierValue( ModifierNames modName, Stats stat )
	{
		return modifiers[ (int)modName, (int)stat ];
	}
}