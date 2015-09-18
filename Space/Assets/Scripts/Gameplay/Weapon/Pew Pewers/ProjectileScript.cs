using UnityEngine;

// Basic projectile script
public class ProjectileScript : MonoBehaviour
{
	// Pubbies
	public float speed = 10.0f;
	public float lifeTime = 1.0f;
	public bool stayAlive = false;
	
	void Update ()
	{
		// Using translate because using rigidbodies for movement caused jittering
		transform.Translate( Vector3.up * speed * Time.deltaTime );
		
		lifeTime -= Time.deltaTime;
		
		if( lifeTime <= 0 && !stayAlive)
		{
			// Ideally we'll use object pooling rather than spawn/destroy
			// But that comes later
			Destroy( gameObject );
		}
	}
}
