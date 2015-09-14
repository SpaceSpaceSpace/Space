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

	// Use this for initialization
	void Start () {
		generateBelt (36, new Vector2 (15.0f, 12.0f), false);
	}

	public void generateObjectAtRandom(GameObject objectPrefab, Vector2 radius)
	{
		float angle = Random.Range (0.0f, (2.0f * Mathf.PI));

		float x = Mathf.Sin(angle) * radius.x;
		float y = Mathf.Cos(angle) * radius.y;
		
		Vector3 pos = new Vector3(x,y,0) + centerPoint.position;

		Instantiate(objectPrefab,pos,Quaternion.identity);
	}

	/// <summary>
	/// Generates a belt of satellite objects
	/// </summary>
	/// <param name="numChunks">Number of groups in the orbital belt. (Example: 2 would generate chunks at the north and south pole of the center)</param>
	/// <param name="radius">Radius of spawn from center point</param>
	/// <param name="artificial">If set to <c>true</c> satellites span, if <c>false</c> asteroids spawn.</param>
	public void generateBelt(int numChunks, Vector2 radius, bool artificial)
	{
		for(int i = 0; i < numChunks; i++)
		{
			float angle = i * ((Mathf.PI *2)/numChunks);
			
			float x = Mathf.Sin(angle) * radius.x;
			float y = Mathf.Cos(angle) * radius.y;
			
			Vector3 chunkCenter = new Vector3(x,y,0) + centerPoint.position;
			
			int numOfAsteroids = Random.Range(1,4);
			
			for(int j = 0; j < numOfAsteroids; j++)
			{
				float chunkAngle = j * ((Mathf.PI *2)/numOfAsteroids);
				
				float chunkX = Mathf.Sin(chunkAngle) * 0.65f;
				float chunkY = Mathf.Cos(chunkAngle) * 0.65f;
				
				Vector3 pos = new Vector3(chunkX,chunkY,0) + chunkCenter;
				
				GameObject asteroidGenerated = (GameObject) Instantiate(satPrefab,pos,Quaternion.identity);
				
				asteroidGenerated.GetComponent<Satellite>().artificial = artificial;
				
				objectCount++;
			}
		}
	}
}
