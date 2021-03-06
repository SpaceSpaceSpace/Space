// =====================================================
// =====!!! This file was generated. No touchy. !!!=====
// =====================================================
public struct WeaponModifier
{
	public enum ModifierNames
	{
		DEFAULT,
		Crappy,
		Fast,
		Godly,
		of_Doom_suffix,
		Abundant,
		Lacking,
		Big_Boom_Boom,
		Rapid,
		Speedy,
		Blaarg,
		Reaching,
		Burny,

		NUM_MODIFIERS,

	}

	public const int PROJ_WEP_START = (int)ModifierNames.Crappy;
	public const int PROJ_WEP_END = (int)ModifierNames.Abundant;
	public const int SCATTER_WEP_START = PROJ_WEP_END;
	public const int SCATTER_WEP_END = (int)ModifierNames.Big_Boom_Boom;
	public const int MISSILE_WEP_START = PROJ_WEP_START;
	public const int MISSILE_WEP_END = PROJ_WEP_END;
	public const int MINE_WEP_START = SCATTER_WEP_END;
	public const int MINE_WEP_END = (int)ModifierNames.Blaarg;
	public const int BEAM_WEP_START = MINE_WEP_END;
	public const int BEAM_WEP_END = (int)ModifierNames.NUM_MODIFIERS;

	public enum Stats
	{
		DAMAGE,
		ACCURACY,
		FIRE_RATE,

		NUM_STATS,

		BONUS_PROJECTILES = DAMAGE,
		MINE_SPEED = ACCURACY,
		BEAM_RANGE = ACCURACY,

	}

	public static readonly float[,] modifiers = {
		{ 1.0f, 1.0f, 1.0f },
		{ 0.8f, 0.8f, 0.8f },
		{ 0.9f, 0.9f, 1.2f },
		{ 2.0f, 2.0f, 2.0f },
		{ 1.5f, 0.7f, 1.0f },
		{ 5, 1.0f, 1.0f },
		{ -2, 1.0f, 1.0f },
		{ 1.5f, 0.7f, 1.0f },
		{ 0.9f, 1.0f, 1.2f },
		{ 1.0f, 1.5f, 1.0f },
		{ 0.8f, 0.8f, 0.0f },
		{ 1.0f, 1.2f, 0.0f },
		{ 1.2f, 1.0f, 0.0f },

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
