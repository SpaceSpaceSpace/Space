// =====================================================
// =====!!! This file was generated. No touchy. !!!=====
// =====================================================
public struct WeaponModifier
{
	public enum ModifierNames
	{
		ModifierName,
		DEFAULT,
		Crappy,
		Fast,
		Godly,
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
}
