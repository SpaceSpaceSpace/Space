using UnityEngine;
using WyrmTale;

public class ObjectiveKillTarget : Objective
{
    public int GuardCount = 4;

    private GameObject target;
    private static GameObject AISpawner = null;

    private float timeToObjUpdate = 5.0f;
    private SpriteRenderer masterSpriteRenderer;

    public ObjectiveKillTarget()
    {
        Position = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
    }

    public ObjectiveKillTarget(float minX, float maxX, float minY, float maxY)
    {
        Position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
    }

    public ObjectiveKillTarget(Vector3 position)
    {
        Position = position;
    }

    public override void SetupObjective(GameObject objectiveManager, int tier)
    {
        masterSpriteRenderer = objectiveManager.GetComponentInChildren<SpriteRenderer>();

        if(AISpawner == null)
            AISpawner = Resources.Load("AISpawner") as GameObject;

        GameObject spawner = (GameObject)GameObject.Instantiate(AISpawner, Position, Quaternion.identity);
		spawner.transform.parent = WarpScript.instance.currentPlanet.transform;
        AISpawnerScript aiSpawnerScript = spawner.GetComponent<AISpawnerScript>();
		aiSpawnerScript.tier = tier;
        aiSpawnerScript.startAI = GuardCount + 1;
        aiSpawnerScript.maxAI = GuardCount + 1;
        aiSpawnerScript.Init();
        target = aiSpawnerScript.squadLeader;
    }

    public override void ObjectiveUpdate()
    {
        //Reposition the objective marker occasionally
        timeToObjUpdate -= Time.deltaTime;
        if (timeToObjUpdate <= 0.0f)
        {
            Position = target.transform.position;
            timeToObjUpdate = 5.0f;
        }

        //Blink the objective markers
        Color c = masterSpriteRenderer.color;
        float alpha = timeToObjUpdate.Remap(0f, 5f, 0f, 1f);
        c.a = alpha;
        masterSpriteRenderer.color = c;

        //Debug.Log(gameObject.GetComponentInChildren<SpriteRenderer>().color.a);

        //If the target is dead, we've completed the objective
        if (target == null)
            completed = true;
    }

    public override void HitObjective(Collider2D collider){ }

    protected override JSON ToJSON()
    {
        JSON js = new JSON();
        js["Type"] = "KillTarget";
        js["GuardCount"] = GuardCount;

        return js;
    }
    protected override void FromJSON(JSON js)
    {
        GuardCount = js.ToInt("GuardCount");
    }
}
