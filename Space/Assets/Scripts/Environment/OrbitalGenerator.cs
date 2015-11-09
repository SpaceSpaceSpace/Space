using UnityEngine;
using System.Collections;

public class OrbitalGenerator : MonoBehaviour {

	//Orbital Generator that uses a center point and radius to create a random
	//object in space or an asteroid belt
	public Transform centerPoint;
	//public Vector2 radius;
	public GameObject satPrefab;
	//public int numChunks;
	public int objectCount = 0;

	public GameObject[] asteroidPrefabs = new GameObject[9];
    public GameObject[] satellitePrefabs = new GameObject[2];

	// Use this for initialization
	void Start () {


		generateBelt (24, new Vector2 (90.0f, 90.0f), true);
		//generateBelt (48, new Vector2 (105.0f, 105.0f), false);
		generateBelt (24, new Vector2 (130.0f, 130.0f), false);
		generateBelt (32, new Vector2 (170.0f, 170.0f), false);
		generateBelt (32, new Vector2 (210.0f, 210.0f), false);
		generateBelt (48, new Vector2 (250.0f, 250.0f), false);
		generateBelt (56, new Vector2 (290.0f, 290.0f), false);

		//generateBelt (64, new Vector2 (4.0f, 4.0f), false);
	}

	public void generateObjectAtRandom(GameObject objectPrefab, Vector2 radius)
	{
		float angle = Random.Range (0.0f, (2.0f * Mathf.PI));

		float x = Mathf.Sin(angle) * radius.x;
		float y = Mathf.Cos(angle) * radius.y;
		
		Vector3 pos = new Vector3(x,y,0) + centerPoint.position;

		GameObject obj = (GameObject)Instantiate(objectPrefab,pos,Quaternion.identity);
		obj.GetComponent<Satellite> ().ScaleMass (Random.Range (1, 12), false);
	}

	/// <summary>
	/// Generates a belt of satellite objects
	/// </summary>
	/// <param name="numChunks">Number of groups in the orbital belt. (Example: 2 would generate chunks at the north and south pole of the center)</param>
	/// <param name="radius">Radius of spawn from center point</param>
	/// <param name="artificial">If set to <c>true</c> satellites span, if <c>false</c> asteroids spawn.</param>
	public void generateBelt(int numChunks, Vector2 radius, bool artificial)
	{
        string beltName;
        GameObject[] prefabs;

        if (artificial)
        {
            beltName = "Satellite Belt";
            prefabs = satellitePrefabs;
        }
        else
        {
            beltName = "Asteroid Belt";
            prefabs = asteroidPrefabs;
        }

		GameObject beltMaster = new GameObject ();
		beltMaster.name = beltName;
		
		for(int i = 0; i < numChunks; i++)
		{
			float angle = i * ((Mathf.PI *2)/numChunks);
			
			float x = Mathf.Sin(angle) * radius.x;
			float y = Mathf.Cos(angle) * radius.y;
			
			Vector3 chunkCenter = new Vector3(x,y,0) + centerPoint.position;
				
			int numOfAsteroids = Random.Range(2,4);
			
			for(int j = 0; j < numOfAsteroids; j++)
			{
				float chunkAngle = j * ((Mathf.PI *2)/numOfAsteroids);
				
				float chunkX = Mathf.Sin(chunkAngle) * 16.0f;
				float chunkY = Mathf.Cos(chunkAngle) * 16.0f;
				
				Vector3 pos = new Vector3(chunkX,chunkY,0) + chunkCenter;
				
				int randomSpriteNum = Random.Range (0, prefabs.Length - 1);
				
				GameObject prefabGenerated = (GameObject) Instantiate(prefabs[randomSpriteNum],pos,Quaternion.identity);

                prefabGenerated.GetComponent<Satellite>().artificial = artificial;
                prefabGenerated.GetComponent<Satellite>().radius = radius;
                prefabGenerated.GetComponent<Satellite> ().ScaleMass (Random.Range (.5f, 8f), false);
                //prefabGenerated.GetComponent<Satellite>().SetCenterOfOrbit(centerPoint);
                prefabGenerated.transform.parent = beltMaster.transform;
				
				objectCount++;
			
			}
		}
	}

}

