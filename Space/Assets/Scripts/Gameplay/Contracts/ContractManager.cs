using UnityEngine;
using System.Collections.Generic;
using WyrmTale;

public class ContractManager : MonoBehaviour
{
    //Arrays of lists to store data in
    List<ContractContent>[] Contents = new List<ContractContent>[10];
    List<ContractTargetName>[] TargetNames = new List<ContractTargetName>[10];
    List<ContractTargetImage>[] TargetImages = new List<ContractTargetImage>[10];
    List<ContractTargetShipImage>[] TargetShipImages = new List<ContractTargetShipImage>[10];
    List<ContractObjectives>[] Objectives = new List<ContractObjectives>[10];

    JSON elements;

    //Load contract JSON into data structures
	void Start ()
    {
        //Load all element JSON
        string filepath = ContractElement.ContractElementFilePath;
        elements = ContractUtils.LoadJSONFromFile(filepath);

        //Get various elements
        PopulateContents();
        PopulateTargetNames();
        PopulateTargetImages();
        PopulateTargetShipImages();
        PopulateObjectives();
    }

    void PopulateContents()
    {
        JSON[] contents = elements.ToArray<JSON>("ContractContents");
        for (int i = 0; i < contents.Length; i++)
        {
            ContractContent content = (ContractContent)contents[i];
            int tierIndex = content.Tier - 1;

            List<ContractContent> tierList = Contents[tierIndex];
            if (tierList == null)
            {
                tierList = new List<ContractContent>();
                Contents[tierIndex] = tierList;
            }

            tierList.Add(content);
        }
    }
    void PopulateTargetNames()
    {
        JSON[] targetNames = elements.ToArray<JSON>("ContractTargetNames");
        for (int i = 0; i < targetNames.Length; i++)
        {
            ContractTargetName targetName = (ContractTargetName)targetNames[i];
            int tierIndex = targetName.Tier - 1;

            List<ContractTargetName> tierList = TargetNames[tierIndex];
            if (tierList == null)
            {
                tierList = new List<ContractTargetName>();
                TargetNames[tierIndex] = tierList;
            }

            tierList.Add(targetName);
        }
    }
    void PopulateTargetImages()
    {
        JSON[] targetImages = elements.ToArray<JSON>("ContractTargetImages");
        for (int i = 0; i < targetImages.Length; i++)
        {
            ContractTargetImage targetImage = (ContractTargetImage)targetImages[i];
            int tierIndex = targetImage.Tier - 1;

            List<ContractTargetImage> tierList = TargetImages[tierIndex];
            if (tierList == null)
            {
                tierList = new List<ContractTargetImage>();
                TargetImages[tierIndex] = tierList;
            }

            tierList.Add(targetImage);
        }
    }
    void PopulateTargetShipImages()
    {
        JSON[] targetShipImages = elements.ToArray<JSON>("ContractTargetShipImages");
        for (int i = 0; i < targetShipImages.Length; i++)
        {
            ContractTargetShipImage targetShipImage = (ContractTargetShipImage)targetShipImages[i];
            int tierIndex = targetShipImage.Tier - 1;

            List<ContractTargetShipImage> tierList = TargetShipImages[tierIndex];
            if (tierList == null)
            {
                tierList = new List<ContractTargetShipImage>();
                TargetShipImages[tierIndex] = tierList;
            }

            tierList.Add(targetShipImage);
        }
    }
    void PopulateObjectives()
    {
        JSON[] objectives = elements.ToArray<JSON>("ContractObjectives");
        for (int i = 0; i < objectives.Length; i++)
        {
            ContractObjectives objective = (ContractObjectives)objectives[i];
            int tierIndex = objective.Tier - 1;

            List<ContractObjectives> tierList = Objectives[tierIndex];
            if (tierList == null)
            {
                tierList = new List<ContractObjectives>();
                Objectives[tierIndex] = tierList;
            }

            tierList.Add(objective);
        }
    }

}
