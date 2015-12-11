using UnityEngine;
using System.Collections.Generic;
using WyrmTale;

public class ContractManager : MonoBehaviour
{
    //Arrays of lists to store data in
    public static List<ContractModel> StoryContracts = new List<ContractModel>();
    public static List<ContractContent>[] Contents = new List<ContractContent>[10];
    public static List<ContractTargetName>[] TargetNames = new List<ContractTargetName>[10];
    public static List<ContractTargetImage>[] TargetImages = new List<ContractTargetImage>[10];

    JSON elements;

    //Load contract JSON into data structures
	void Start ()
    {
        //Load all element JSON
        string filepath = ContractUtils.ContractElementFilePath;
        elements = ContractUtils.LoadJSONFromAsset(filepath);

        //Load story contracts once
        PopulateStoryContracts();

        //Get various elements
        PopulateContents();
        PopulateTargetNames();
        PopulateTargetImages();
    }

    void PopulateStoryContracts()
    {
        string filepath = ContractUtils.StoryContractFilePath;
        JSON js = ContractUtils.LoadJSONFromAsset(filepath);

        JSON[] rawContracts = js.ToArray<JSON>("Contracts");

        foreach (JSON rawContract in rawContracts)
        {
            ContractModel contract = (ContractModel)rawContract;

            StoryContracts.Add(contract);
        }

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

}
