using System.Collections.Generic;
using System.IO;
using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using WyrmTale;

public abstract class ContractElement
{
    public int Tier;

    public static string ContractElementPath = "Assets/Resources/Contracts/";
    public static string ContractElementName = "ContractElements";
    public static string ContractElementExt = ".json";

    protected static List<T> LoadElements<T>()
    {
        string contractsContent = "{}";
        List<T> elements = new List<T>();

        try
        {
            contractsContent = File.ReadAllText(ContractElementPath + ContractElementName + ContractElementExt);
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("Exception: " + e.Message + " " + "Creating new JSON");
        }

        JSON js = new JSON();
        js.serialized = contractsContent;

        Type elementType = typeof(T);
        Type jsonType = typeof(JSON);

        JSON[] rawElements = js.ToArray<JSON>(elementType.ToString() + "s");

        //Do a bit of reflection to call a method to apply json to the object
        MethodInfo method = elementType.GetMethod("FromJSON", BindingFlags.Instance | BindingFlags.NonPublic);
        MethodInfo generic = method.MakeGenericMethod();

        foreach (JSON rawElement in rawElements)
        {
            //Perform a deep copy to the generic type
            T element = (T)Activator.CreateInstance(elementType, new object[] { });
            generic.Invoke(element, new object[] { rawElement });

            elements.Add(element);
        }

        return elements;
    }

    protected static void WriteElement<T>(List<T> elements)
    {
        Type elementType = typeof(T);

        //Explicitly cast the List of ContractElements to an array of JSON objects
        JSON contractJSON = new JSON();
        JSON[] contractsListJSON = new JSON[elements.Count];

        MethodInfo method = typeof(T).GetMethod("ToJSON",BindingFlags.Instance | BindingFlags.NonPublic);
        MethodInfo generic = method.MakeGenericMethod();

        for (int i = 0; i < elements.Count; i++)
        {
            //Perform a deep copy from the generic type to JSON using reflection
            contractsListJSON[i] = (JSON)generic.Invoke(elements[i], null);
        }

        contractJSON[elementType.ToString() + "s"] = contractsListJSON;

        File.WriteAllText(ContractElementPath + ContractElementName + ContractElementExt, contractJSON.serialized);
        AssetDatabase.Refresh();
    }

    protected abstract ContractElement FromJSON(JSON js);
    protected abstract JSON ToJSON();
}
    