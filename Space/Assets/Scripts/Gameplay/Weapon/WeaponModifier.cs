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
		NUM_MODIFIERS
	}

	public enum Stats
	{
		Damage,
		Accuracy,
		Fire_Rate,
		Range,
		NUM_STATS
	}

	public static readonly float[,] modifiers = {
		{ 1.0f, 1.0f, 1.0f, 1.0f },
		{ 0.8f, 0.8f, 0.8f, 0.8f },
		{ 0.9f, 0.9f, 1.2f, 1.0f },
		{ 2.0f, 2.0f, 2.0f, 2.0f },
	};

	public static void GetModifierName( ModifierNames modName, string weaponName, out string outputString )
	{
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
}
