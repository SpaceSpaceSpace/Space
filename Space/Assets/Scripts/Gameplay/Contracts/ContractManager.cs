using UnityEngine;
using System.Collections.Generic;
using WyrmTale;

public class ContractManager : MonoBehaviour
{
    //Arrays of lists to store data in
    List<ContractContent>[] Contents = new List<ContractContent>[10];

    //Load contract JSON into data structures
	void Start ()
    {
        //Load all element JSON
        string filepath = ContractElement.ContractElementFilePath;
        JSON elements = ContractUtils.LoadJSONFromFile(filepath);

        //Get Contents
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
	
}
