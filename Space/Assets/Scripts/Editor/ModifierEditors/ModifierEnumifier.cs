using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
public class ModifierEnumifier : MonoBehaviour
{
    //Static so that they can be easily accessed from other editors
    public static string DATA_PATH = "Assets/Scripts/Editor/ModifierEditors/Data/WeaponModifiers.csv";
    public static string WEP_TYPE_DATA_PATH = "Assets/Scripts/Editor/ModifierEditors/Data/WeaponTypeData.txt";
    public static string STAT_ALIAS_DATA_PATH = "Assets/Scripts/Editor/ModifierEditors/Data/StatAliasData.txt";

    public const string TEMPLATE_PATH = "Assets/Scripts/Editor/ModifierEditors/Data/WeaponModifierTemplate.txt";
    public const string CODE_PATH = "Assets/Scripts/Gameplay/Weapon/WeaponModifier.cs";

    [MenuItem("Space/Generate Modifier Code")]
    public static void GenerateFile()
    {
        string[,] data;
        LoadAndParseData(out data);
        WriteFile(data);
    }

    public static void LoadAndParseData(out string[,] data, int typeIndex = -1)
    {
        string[] dataStr;
        ReadLines(DATA_PATH, out dataStr);

        if (dataStr == null)
        {
            data = null;
            return;
        }

        string[] lineData = dataStr[0].Split(',');

        int numLines = dataStr.Length;
        int numIndices = lineData.Length;

        data = new string[numLines, numIndices];

        for (int i = 0; i < numLines; i++)
        {
            lineData = dataStr[i].Split(',');
            for (int j = 0; j < numIndices; j++)
            {
                if (j < lineData.Length)
                {
                    data[i, j] = lineData[j];
                }
                else
                {
                    print("ERROR: Data missing at line " + i + "m index " + j);
                    data = null;
                    return;
                }
            }
        }

        //If we're given a type index, we'll look up where we should start and end for that type
        if (typeIndex > -1)
        {
            int startIndex = 0;
            int endIndex = numLines - 1;

            //Load the type data for examination
            string[] typeStr;
            ReadLines(WEP_TYPE_DATA_PATH, out typeStr);

            string startStr = null;
            string endStr = null;

            if (typeIndex > 0)
                startStr = typeStr[typeIndex];
            else
                startStr = "ModifierName";
            endStr = typeStr[typeIndex + 1];

            for (int i = 0; i < numLines; i++)
            {
                string name = data[i, 0];
                if (name == startStr)
                    startIndex = i + 1;
                if (name == endStr)
                    endIndex = i;
            }

            int newRowCount = (1 + endIndex) - startIndex;

            string[,] filteredData = new string[newRowCount, numIndices];
            for (int i = 0; i < newRowCount; i++)
            { 
                for (int j = 0; j < numIndices; j++)
                    filteredData[i, j] = data[startIndex + i, j];
            }

            data = filteredData;
        }
    }

    private static void ReadLines(string path, out string[] dataStr)
    {
        if (File.Exists(path))
        {
            dataStr = File.ReadAllLines(path);
        }
        else
        {
            print("Could not find file at path " + path);
            dataStr = null;
        }
    }

    //Writes out a new modifier or a replacement for an existing modifer
    //Modifier = the modifier to write out
    //replacementIndex = the index of the modifier to replace or -1 if to append
    //typeIndex = the index of the type to replace based off the weapon types file
    public static bool WriteNewModifier(Modifier modifier, int replacementIndex, int typeIndex)
    {
        //Append CSV of modifier to the end of the data
        //Modify the stat alias data to match the new file

        if (!File.Exists(DATA_PATH))
        {
            Debug.Log("ERROR: Could not find Weapon Modifier Data");
            return false;
        }
        if (!File.Exists(WEP_TYPE_DATA_PATH))
        {
            Debug.Log("ERROR: Could not find Weapon Type Data");
            return false;
        }

        //Load Data
        string allData = File.ReadAllText(DATA_PATH);
        string[] dataLines = allData.Replace("\r", "").Split('\n');

        //Load Types
        string allTypes = File.ReadAllText(WEP_TYPE_DATA_PATH);
        string[] typeLines = allTypes.Replace("\r", "").Split('\n');

        //Test if a modifier with this name exists already
        for (int i = 0; i < dataLines.Length; i++)
        {
            string line = dataLines[i];
            if (line.StartsWith(modifier.Name + ','))
            {
                if (EditorUtility.DisplayDialog("Conflict", "A Modifier with this name already exists. Do you want to overwrite?", "Yes", "Oops, Cancel"))
                    replacementIndex = i - 1;
                else
                    return false;
            }
        }

        //Determine end point of this type
        string lastEndName = typeLines[typeIndex];
        string currentEndName = typeLines[typeIndex + 1];

        int currentEndIndexInData = 0;
        int lastEndIndex = 0;
        for (int i = 0; i < dataLines.Length; i++)
        {
            string line = dataLines[i];
            if (line.Contains(currentEndName))
                currentEndIndexInData = i - 1;
            if (line.Contains(lastEndName))
                lastEndIndex = i - 1;
        }

        //Just insert to the end of this type data
        if (replacementIndex < 0)
        {
            WriteType(typeIndex, modifier.Name);

            List<string> dataList = dataLines.ToList();

            dataList.Insert(currentEndIndexInData + 2, modifier.ToString());

            allData = "";
            for (int i = 0; i < dataList.Count; i++)
            {
                string line = dataList[i];

                if (i < dataList.Count - 1)
                    allData += line + '\n';
                else
                    allData += line;
            }
        } //Overwrite
        else
        {
            replacementIndex += lastEndIndex;
            if (typeIndex > 0)
                replacementIndex++;

            //If we're replacing the current end name, we need to edit the type data file
            if (replacementIndex == currentEndIndexInData)
            {
                WriteType(typeIndex, modifier.Name);
            }

            dataLines[replacementIndex + 1] = modifier.ToString();

            allData = "";
            for (int i = 0; i < dataLines.Length; i++)
            {
                string line = dataLines[i];

                if (i < dataLines.Length - 1)
                    allData += line + '\n';
                else
                    allData += line;
            }
        }

        File.WriteAllText(DATA_PATH, allData);

        return true;
    }

    public static void WriteTypeModifiers(int typeIndex, List<Modifier> modifiers)
    {
        //Append CSV of modifier to the end of the data
        if (!File.Exists(DATA_PATH))
        {
            Debug.Log("ERROR: Could not find Weapon Modifier Data");
            return;
        }

        if (!File.Exists(WEP_TYPE_DATA_PATH))
        {
            Debug.Log("ERROR: Could not find Weapon Type Data");
            return;
        }

        //Load Data
        string allData = File.ReadAllText(DATA_PATH);
        string[] dataLines = allData.Replace("\r", "").Split('\n');

        //Load Types
        string allTypes = File.ReadAllText(WEP_TYPE_DATA_PATH);
        string[] typeLines = allTypes.Replace("\r", "").Split('\n');

        //Find the start and end of this type in the data file
        string lastEndName = typeLines[typeIndex];
        string currentEndName = typeLines[typeIndex + 1];

        int currentEndIndexInData = 0;
        int lastEndIndex = 0;
        for (int i = 0; i < dataLines.Length; i++)
        {
            string line = dataLines[i];
            if (line.Contains(currentEndName))
                currentEndIndexInData = i - 1;
            if (line.Contains(lastEndName))
                lastEndIndex = i;
        }

        string newData = "";

        int sizeChange = modifiers.Count - (currentEndIndexInData - lastEndIndex) - 1;

        //Write unchanged mods before this type
        for (int i = 0; i < lastEndIndex + 1; i++)
            newData += dataLines[i] + '\n';

        //Write changed mods
        for (int i = 0; i < modifiers.Count; i++)
        {
            Modifier mod = modifiers[i];

            newData += mod.ToString() + '\n';
        }

        //Write unchanged mods after this type
        for (int i = lastEndIndex + modifiers.Count - sizeChange + 1; i < dataLines.Length; i++)
        {
            if (i < dataLines.Length - 1)
                newData += dataLines[i] + '\n';
            else
                newData += dataLines[i];
        }

        //Write out changed end type
        WriteType(typeIndex, modifiers.Last().Name);

        File.WriteAllText(DATA_PATH, newData);
    }

    private static void WriteType(int index, string name)
    {
        //Load Types
        string allTypes = File.ReadAllText(WEP_TYPE_DATA_PATH);
        string[] typeLines = allTypes.Replace("\r", "").Split('\n');

        typeLines[index + 1] = name;
        allTypes = "";
        for (int i = 0; i < typeLines.Length; i++)
        {
            string line = typeLines[i];

            if (i < typeLines.Length - 1)
                allTypes += line + '\n';
            else
                allTypes += line;
        }
        File.WriteAllText(WEP_TYPE_DATA_PATH, allTypes);
    }

	private static void WriteFile( string[,] data )
	{
		if( data == null )
		{
			print( "Code generation cancelled." );
			return;
		}

		if( !File.Exists( TEMPLATE_PATH ) )
		{
			print( "ERROR: Could not find WeaponModifier template" );
			return;
		}

		int rows = data.GetLength( 0 );
		int columns = data.GetLength( 1 );

		/*for( int i = 0; i < rows; i++ )
		{
			for( int j = 0; j < columns; j++ )
			{
				print( data[ i, j ] );
			}
		}*/
		/*
		 * [r,c]	col1	col2	col3	col4
		 * row1		mods	stat1 	stat2	stat3
		 * row2		mod1	stat1	stat2	stat3
		 * row3		mod2	stat1	stat2	stat3
		 * 
		 */

		print( "Writing Code File..." );

		string code = File.ReadAllText( TEMPLATE_PATH );

		// Write in the modifier names
		string modifierNames = "";
		for( int i = 1; i < rows; i++ )
		{
			modifierNames += "\t\t" + data[ i, 0 ] + ",\n";
		}
		code = code.Replace( "\\ModifierNames\\", modifierNames );

		// Write in the stats
		string statNames = "";
		for( int i = 1; i < columns; i++ )
		{
			statNames += "\t\t" + data[ 0, i ] + ",\n";
		}
		code = code.Replace( "\\StatNames\\", statNames );

		// Write in Stat aliases
		string[] dataStr;
		ReadLines( STAT_ALIAS_DATA_PATH, out dataStr );
		
		string aliasData = "";
		if( dataStr != null )
		{
			aliasData += "\n";
			for( int i = 1; i < dataStr.Length; i++ )
			{
				aliasData += "\t\t" + dataStr[ i ] + ",\n";
			}
		}
		else
		{
			print( "WARNING: Failed to load aliasData" );
		}
		code = code.Replace( "\\AltStatNames\\", aliasData );

		// Write in Type data
		ReadLines( WEP_TYPE_DATA_PATH, out dataStr );

		if( dataStr == null || dataStr.Length < 5 )
		{
			print( "Error: Failed to load WeaponTypeData" );
			print( "Cancelled writing file" );
			return;
		}

		code = code.Replace( "\\GENERIC_END\\", dataStr[ 1 ] );
		code = code.Replace( "\\PROJ_WEP_END\\", dataStr[ 2 ] );
		code = code.Replace( "\\SCATTER_WEP_END\\", dataStr[ 3 ] );
		code = code.Replace( "\\MINE_WEP_END\\", dataStr[ 4 ] );

		// Write in the stat values
		string statValues = "";
		for( int i = 1; i < rows; i++ )
		{
			statValues += "\t\t{ ";
			for( int j = 1; j < columns - 1; j++ )
			{
				statValues += data[ i, j ] + ", ";
			}
			
			statValues += data[ i, columns - 1 ] + " },\n";
		}
		code = code.Replace( "\\StatValues\\", statValues );

		File.WriteAllText( CODE_PATH, code );
		print( "Finished writing file to " + CODE_PATH );
	}
}
