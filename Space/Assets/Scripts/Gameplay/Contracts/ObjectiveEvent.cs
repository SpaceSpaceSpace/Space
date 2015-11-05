using UnityEngine;
using System.Collections;

public enum ObjectiveType
{
    GoTo,
    KillTarget,
    TurnInContract,
    EscortCargo
}

public class ObjectiveEvent : MonoBehaviour
{


    public GameObject AISpawner;
    public GameObject spaceStation;

    private Contract objectiveContract;
    private GameObject target;
    private GameObject nextObjective;
    private bool firstActivation = true;
    private float timeToObjUpdate = 5.0f;
    ObjectiveType type;


    //Look into enums for different objective types

    public Contract ObjectiveContract
    {
        get { return objectiveContract; }
        set { objectiveContract = value; }
    }

    public GameObject NextObjective
    {
        get { return nextObjective; }
        set { nextObjective = value; }
    }

    void OnEnable()
    {
        if (firstActivation)
        {
            firstActivation = false;
            spaceStation = GameObject.Find("SpaceStore");
        }
        else
        {
            if (type == ObjectiveType.TurnInContract)
            {

                transform.position = spaceStation.transform.position;
                transform.parent = spaceStation.transform;
            }
        }
    }

    public bool CheckIfNextObjective()
    {
        if (nextObjective == null)
        {
            return false;
        }

        return true;
    }

    public void init(ObjectiveType p_Type)
    {
        type = p_Type;

        GameObject spawner;
        //Spawn Correct Objective
        switch (type)
        {
            case ObjectiveType.GoTo:
                break;
            case ObjectiveType.KillTarget:
                spawner = (GameObject)GameObject.Instantiate(AISpawner, transform.position, Quaternion.identity);
                spawner.GetComponent<AISpawnerScript>().Init();
                target = spawner.GetComponent<AISpawnerScript>().squadLeader;
                break;
            case ObjectiveType.TurnInContract:
                break;
            case ObjectiveType.EscortCargo:
                Vector2 spawnPos = new Vector2(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f));
                spawner = (GameObject)GameObject.Instantiate(AISpawner, spawnPos, Quaternion.identity);
                float xPos = Random.Range(0.01f, 2.0f);
                float yPos = Random.Range(0.01f, 2.0f);
                if (Random.Range(-1.0f, 1.0f) > 0.0f)
                {
                    xPos = Mathf.Ceil(xPos);
                    // Get xPos to be equal to 1 or 2
                    xPos -= 1.0f;
                    if (xPos == 0.0f)
                        xPos = -1.0f;
                    yPos -= 1.0f;
                }
                else
                {
                    yPos = Mathf.Ceil(yPos);
                    // Get yPos to be equal to 1 or 2
                    yPos -= 1.0f;
                    if (yPos == 0.0f)
                        yPos = -1.0f;
                    xPos -= 1.0f;
                }

                Vector2 objectivePos = new Vector2(xPos, yPos) * 350.0f;
                transform.position = objectivePos;
                spawner.GetComponent<AISpawnerScript>().Objective = transform;
                spawner.GetComponent<AISpawnerScript>().Init();
                break;
        }
    }

    private void CompleteTask()
    {
        objectiveContract.CompleteContractObjective(this.gameObject);
        PlayerShipScript.player.ObjectiveMarker.GetComponent<UIMarker>().removeTargetFromStack(this.gameObject);
        GameObject.Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case ObjectiveType.KillTarget:
                if (target == null)
                {
                    if (CheckIfNextObjective())
                    {
                        //Set the next objective to active and update the minimap
                        nextObjective.SetActive(true);
                        objectiveContract.SetUIMarker(nextObjective);
                    }
                    CompleteTask();
                }
                else
                {
                    timeToObjUpdate -= Time.deltaTime;
                    if (timeToObjUpdate <= 0.0f)
                    {
                        transform.position = target.transform.position;
                        timeToObjUpdate = 5.0f;
                    }
                    Color c = gameObject.GetComponentInChildren<SpriteRenderer>().color;
                    float alpha = timeToObjUpdate.Remap(0f, 5f, 0f, 1f);
                    c.a = alpha;
                    gameObject.GetComponentInChildren<SpriteRenderer>().color = c;
                    //Debug.Log(gameObject.GetComponentInChildren<SpriteRenderer>().color.a);
                }
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player Ship" && type != ObjectiveType.KillTarget)
        {
            //objectiveContract.completed = true;//Make boolean an array for multi-mission contracts
            CompleteTask();
        }
        else if (col.gameObject.name == "CargoShip" && type == ObjectiveType.EscortCargo)
        {
            //objectiveContract.completed = true;//Make boolean an array for multi-mission contracts
            CompleteTask();
        }
    }
}

public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}