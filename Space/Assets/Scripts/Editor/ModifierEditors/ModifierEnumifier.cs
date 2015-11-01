using UnityEngine;
using UnityEditor;
using System.IO;

public class ModifierEnumifier : MonoBehaviour
{
	public const string DATA_PATH = "Assets/Scripts/Editor/ModifierEditors/Data/WeaponModifiers.csv";
	public const string CODE_PATH = "Assets/Scripts/Gameplay/Weapon/WeaponModifier.cs";

	[MenuItem("Space/Generate Modifier Code")]
	static void GenerateFile()
	{
		string[,] data;
		LoadAndParseData( out data );
		WriteFile( data );
	}

	private static void LoadAndParseData( out string[,] data )
	{
		if( File.Exists( DATA_PATH ) )
		{
			string[] dataStr = File.ReadAllLines( DATA_PATH );
			string[] lineData = dataStr[ 0 ].Split( ',' );

			int numLines = dataStr.Length;
			int numIndices = lineData.Length;

			data = new string[ numLines, numIndices ];

			for( int i = 0; i < numLines; i++ )
			{
				lineData = dataStr[ i ].Split( ',' );
				for( int j = 0; j < numIndices; j++ )
				{
					if( j < lineData.Length )
					{
						data[ i, j ] = lineData[ j ];
					}
					else
					{
						print( "ERROR: Data missing at line " + i + "m index " + j );
						data = null;
						return;
					}
				}
			}
		}
		else
		{
			print( "ERROR: Couldn't find data file." );
			data = null;
		}
	}

	private static void WriteFile( string[,] data )
	{
		if( data == null )
		{
			print( "Code generation cancelled." );
			return;
		}

		int dataWidth = data.GetLength( 0 );
		int dataHeight = data.GetLength( 1 );

		/*for( int i = 0; i < dataWidth; i++ )
		{
			for( int j = 0; j < dataHeight; j++ )
			{
				print( data[ i, j ] );
			}
		}*/

		print( "Writing Code File..." );
		
		StreamWriter sw = new StreamWriter( CODE_PATH );
		// Warning people not to edit the file
		sw.WriteLine( "// =====================================================" );
		sw.WriteLine( "// =====!!! This file was generated. No touchy. !!!=====" );
		sw.WriteLine( "// =====================================================" );

		// Buckle your pants
		sw.WriteLine( "public struct WeaponModifier" );
		sw.WriteLine( "{" );

		// Modifiers enum declaration
		sw.WriteLine( "\tpublic enum ModifierNames" );
		sw.WriteLine( "\t{" );

		// Inserts the first element of each row, starting with the second row
		for( int i = 1; i < dataHeight; i++ )
		{
			sw.WriteLine( "\t\t" + data[ i, 0 ] + "," );
		}

		// Add in a final enumerator for a length property
		sw.WriteLine( "\t\tNUM_MODIFIERS" );
		sw.WriteLine( "\t}" );

		sw.Write( "\n" );

		// Stats enum declaration
		sw.WriteLine( "\tpublic enum Stats" );
		sw.WriteLine( "\t{" );

		// Inserts the elements from the first row, starting with the second element
		for( int i = 1; i < dataWidth; i++ )
		{
			sw.WriteLine( "\t\t" + data[ 0, i ] + "," );
		}

		// Add in a final enumerator for a length property
		sw.WriteLine( "\t\tNUM_STATS" );
		sw.WriteLine( "\t}" );

		sw.Write( "\n" );

		// Modifier lookup table declaration
		sw.Write( "\tpublic static readonly float[,] modifiers = {" );

		for( int i = 1; i < dataWidth; i++ )
		{
			// Write each array of stats
			sw.Write( "\n\t\t{ " );
			for( int j = 1; j < dataHeight - 1; j++ )
			{
				sw.Write( data[ i, j ] + ", " );
			}

			sw.Write( data[ i, dataHeight - 1 ] + " }," );
		}

		sw.WriteLine( "\n\t};" );

		sw.WriteLine( "}" );

		// And done
		sw.Close();
		print( "Finished writing file to " + CODE_PATH );
	}
}
