using System.IO;
using UnityEngine;
using WyrmTale;
using System.Collections.Generic;

public class ContractUtils
{
    public static JSON LoadJSONFromFile(string filepath)
    {
        string contractsContent = "{}";

        try
        {
            contractsContent = File.ReadAllText(filepath);
        }
        catch (FileNotFoundException e) { Debug.Log("Exception: " + e.Message + " " + "Creating new JSON"); }

        JSON js = new JSON();
        js.serialized = contractsContent;

        return js;
    }

    public static void WriteJSONToFile(string filepath, JSON js)
    {
        File.WriteAllText(filepath, js.serialized);
    }

    public static Contract GetRandomContract(int Tier)
    {
        Contract contract = new Contract();

        //Get Contract element Tier lists
        int indexedTier = Tier - 1;

        List<ContractContent> contents = ContractManager.Contents[indexedTier];
        List<ContractTargetName> targetNames = ContractManager.TargetNames[indexedTier];
        List<ContractTargetImage> targetImages = ContractManager.TargetImages[indexedTier];
        List<ContractTargetShipImage> targetShipImages = ContractManager.TargetShipImages[indexedTier];

        ContractContent content = new ContractContent();
        ContractTargetName targetName = new ContractTargetName();
        ContractTargetImage targetImage = new ContractTargetImage();
        ContractTargetShipImage targetShipImage = new ContractTargetShipImage();

        //Get random content
        if (contents != null)
            content = contents[Random.Range(0, contents.Count)];

        if (targetNames != null)
            targetName = targetNames[Random.Range(0, targetNames.Count)];

        if (targetImages != null)
            targetImage = targetImages[Random.Range(0, targetImages.Count)];

        if (targetShipImages != null)
            targetShipImage = targetShipImages[Random.Range(0, targetShipImages.Count)];

        //Build contract
        contract = new Contract(targetName.TargetName, content.Description, content.Title, targetImage.TargetImagePath, targetShipImage.TargetShipImagePath, content.Objectives);

        return contract;
    }
}
