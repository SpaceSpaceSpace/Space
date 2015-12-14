using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BountyBoard : MonoBehaviour {

    public static int MaxBountyLevel = 1;

    public Text targetName;
    public Text title;
    public Text description;
    public Text objectives;
    public Text reward;
    public Image portrait;
    public Image shipImage;
    public GameObject scrollView;
    public GameObject buttonPrefab;
    private List<Contract> currentContracts;
    private int currentSelectedContract;

    void OnEnable()
    {
        currentContracts = new List<Contract>();

        for (int i = 0; i < 5; i++)
            currentContracts.Add(ContractUtils.GetRandomContract(Random.Range(1, MaxBountyLevel)));

        Contract latestStoryContract = ContractUtils.GetStoryContract(MaxBountyLevel);
        if (latestStoryContract != null)
            currentContracts.Add(latestStoryContract);
        else
            Debug.Log("Shit");

        Debug.Log(MaxBountyLevel);

        PopulateButtons();
    }

    private void PopulateButtons()
    {
        for (int i = 0; i < currentContracts.Count; i++)
        {
            int _i = i;
            GameObject button = Instantiate(buttonPrefab) as GameObject;
            button.name = i.ToString();
            button.transform.SetParent(scrollView.transform, false);
            button.gameObject.GetComponent<Button>().onClick.AddListener(() => SetBountyValues(_i));
            button.GetComponentInChildren<Text>().text = currentContracts[i].Name;
        }

        SetBountyValues(0);
    }

    public void DestroyButtons()
    {
        Button[] buttons = scrollView.GetComponentsInChildren<Button>();
        foreach (Button _button in buttons)
        {
            Destroy(_button.gameObject);
        }
    }

    public void AcceptContract()
    {
        if (currentSelectedContract != -1)
        {
            GameMaster.playerData.AcceptContract(currentContracts[currentSelectedContract]);

            GameObject button = scrollView.transform.FindChild(currentSelectedContract.ToString()).gameObject;

            if (button != null)
            {
                Destroy(button);
            }

            if (scrollView.transform.childCount > 1)
            {
                string indexString = scrollView.transform.GetChild(1).name;
                int index = int.Parse(indexString);

                SetBountyValues(index);
            }
            else
            {
                SetBlankValues();
            }
        }
    }

    public void SetBountyValues(int index)
    {
        currentSelectedContract = index;
        Contract contract = currentContracts[index];

        SetName(contract.TargetName);
        SetTitle(contract.Title);
        SetDescription(contract.Description, contract.TargetName);
        SetPortrait(contract.TargetImage);
        SetShipImage(contract.TargetShipImage);
        SetObjectives(contract.Objectives);
    }

    private void SetBlankValues()
    {
        currentSelectedContract = -1;
        SetName("-----");
        SetTitle("-----");
        SetDescription("-----", "");
        SetReward("-----");
    }

    public void SetName(string p_Name)
    {
        targetName.text = p_Name;
    }

    public void SetTitle(string p_Title)
    {
        title.text = p_Title;
    }

    public void SetDescription(string p_Des, string p_Name)
    {
        //Replace all instances of $name with p_Name
        string fixedDesc = p_Des.Replace("$name", p_Name);
        description.text = fixedDesc;
    }

    public void SetReward(string p_Reward)
    {
        reward.text = p_Reward;
    }

    public void SetPortrait(Sprite p_Portrait)
    {
        portrait.sprite = p_Portrait;
    }

    public void SetShipImage(Sprite p_ShipImage)
    {
        shipImage.sprite = p_ShipImage;
    }

    public void SetObjectives(List<Objective> p_Objectives)
    {
        objectives.text = "";

        for (int i = 0; i < p_Objectives.Count; i++)
        {
            Objective objective = p_Objectives[i];

            switch (objective.GetType().ToString())
            {
                case "ObjectiveKillTarget":
                    ObjectiveKillTarget kill = (ObjectiveKillTarget)objective;
                    objectives.text += "Kill Target - " + kill.GuardCount + " Guards";
                    break;
                case "ObjectiveEscortCargo":
                    ObjectiveEscortCargo escort = (ObjectiveEscortCargo)objective;
                    objectives.text += "Escort " + escort.CargoShipCount + " Cargo Ships";
                    break;
            }

            objectives.text += '\n';
        }
    }
}
