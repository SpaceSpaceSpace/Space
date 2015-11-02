using UnityEngine;
using System.Collections;

public class Satellite : MonoBehaviour {

	public bool artificial;
	public bool inOrbit;
	public float mass;
	public GameObject satPrefab;
	public GameObject satPrefab1;
	public const float MAX_VELOCITY = 100.0f;
	public const float GRAVITATION_MAGNITUDE = 10.0f;
	//public const float STARTING_IMPULSE = 12f;
	public Vector2 radius;
	public float splitForce;
	public Vector3 velocity;
	public Vector3 centerOfOrbit;
	public float health;
    public bool invincible;

	private static GameObject m_dustplosion = null;
	private static GameObject ms_explosion = null;
	
	// Use this for initialization
	void Start () {
		float semiMajor;
		if (radius.x > radius.y)
			semiMajor = radius.x;
		else
			semiMajor = radius.y;
        invincible = false;

		centerOfOrbit = new Vector3 (0.0f, 0.0f, 0.0f);
		Vector3 toCenter = centerOfOrbit - transform.position;
		Vector2 tangential = new Vector2(-toCenter.y, toCenter.x);
		tangential.Normalize();
		velocity = new Vector3(tangential.x,tangential.y, 0.0f);
		velocity *= Mathf.Sqrt (GRAVITATION_MAGNITUDE * (100.0f / toCenter.magnitude - 1.0f / semiMajor));

		transform.GetComponent<Rigidbody2D>().AddForce( new Vector2(velocity.x,velocity.y), ForceMode2D.Impulse);

		if(!artificial)
		{
			//Load dustplosion
			if(m_dustplosion == null)
				m_dustplosion = Resources.Load("AsteroidExplosion") as GameObject;
		}
		else{
			if(ms_explosion == null)
				ms_explosion = Resources.Load("ShipPrefabs/ExplosiveExplosion") as GameObject;
		}

	}

	// Update is called once per frame
	void FixedUpdate () {

		//transform.Rotate (0.0f, 0.0f, rotationFactor);
		//transform.Rotate (0.0f,rotationFactor, 0.0f);
		//transform.Rotate (rotationFactor, 0.0f, 0.0f);
		CalculateOrbitalForce ();
		while (velocity.magnitude > MAX_VELOCITY) {
			velocity /= 2.0f;
		}
		//transform.GetComponent<Rigidbody2D> ().AddForce (new Vector2(velocity.x, velocity.y));

	}

	void CalculateOrbitalForce(){
		velocity = new Vector3 (transform.forward.x, transform.forward.y, 0.0f);
		Vector3 toCenter = centerOfOrbit - transform.position;
		float radius = toCenter.magnitude;
		toCenter.Normalize();
		Vector3 gravity = GRAVITATION_MAGNITUDE * toCenter / (radius * radius);
		transform.GetComponent<Rigidbody2D> ().AddForce (new Vector2(gravity.x, gravity.y));
	}
	public void ApplyDamage(float damage, Vector2 impulse, Vector2 collPosition, bool fromLaser){
        if (!invincible)
        {
            health -= damage;
            Vector3 imp = new Vector3(impulse.x, impulse.y, 0.0f);
            transform.GetComponent<Rigidbody2D>().AddForceAtPosition(imp, collPosition, ForceMode2D.Impulse);
            if (health <= 0)
            {
                if (!artificial) { Split(mass, imp, collPosition, fromLaser); }
                else
                {

                    Instantiate(ms_explosion, transform.position, Quaternion.identity);
                    HandleExplosion();
                    Destroy(gameObject);
                }

            }
        }

	}
	public void ScaleMass(float m, bool split){
		mass = m;
		transform.localScale = new Vector3 (m, m, 1);
		transform.GetComponent<Rigidbody2D> ().mass = mass * 5.0f;
		if(split){		health = 15.0f * m;}
		if(artificial){ health = 10.0f * m;}
		else { health = 30.0f * m;}
	}
	public void OnCollisionStay2D(Collision2D coll) {
		//damage asteroids that remain in contact with eachother
		//if (coll.gameObject.tag == "Asteroid")
			//ApplyDamage (coll.relativeVelocity.magnitude, Vector2.zero);
		if(coll.gameObject.tag == "Asteroid" || coll.gameObject.tag == "Satellite"){
			/*need to check if one asteroid is entirely contained in another
			if so, destroy the smaller asteroid.*/
			ApplyDamage(.05f,Vector2.zero,coll.transform.position, false);

		}

		
	}
	public void Split(float m, Vector3 impulse, Vector2 collPosition, bool fromLaser)
	{
        
		//Spawn a little dust poof
		float rotation = Random.Range(0, 360);
		Vector3 rotVector = new Vector3(0,0,rotation);
		GameObject dust = Instantiate(m_dustplosion,transform.position, Quaternion.Euler(rotVector)) as GameObject;
		
		//Set the fade speed of the dust poof to be inversely proportional to the mass of the satellite
		Explode explosion = dust.GetComponent<Explode>();
		explosion.FadeSpeed = 1/m;
		Instantiate(explosion, transform.position, Quaternion.identity);

		//destroy the asteroid if it is too small to split
		if(mass <  1.0f){
			Destroy(gameObject);
		}
		//otherwise, split it
		else
		{
			Vector3 normalImpulse = Vector3.Normalize(impulse);
			//spawn asteroids apart from each other based on the location of the collision
			Vector2 offset1 = new Vector2(-m * normalImpulse.x,m *normalImpulse.y);
			Vector2 offset2 = new Vector2(m* normalImpulse.x,-m*normalImpulse.y);
            GameObject split1 = (GameObject)Instantiate(satPrefab, new Vector2(transform.position.x, transform.position.y) + offset1, Quaternion.identity);
			split1.GetComponent<Satellite>().ScaleMass(Random.Range(m/4,m/2), true);
            split1.GetComponent<Satellite>().invincible = true;
            split1.GetComponent<Satellite>().StartCoroutine("InvincibleTimer");
			GameObject split2 = (GameObject)Instantiate(satPrefab1, new Vector2(transform.position.x, transform.position.y) + offset2,Quaternion.identity);
			split2.GetComponent<Satellite>().ScaleMass(Random.Range(m/4,m/2), true);
            split2.GetComponent<Satellite>().invincible = true;
            split2.GetComponent<Satellite>().StartCoroutine("InvincibleTimer");

			//add forces that push the splits apart from eachother
			split1.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(impulse.x * -80.0f,impulse.y* 80.0f), ForceMode2D.Impulse);
			split2.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(impulse.x* 80.0f,impulse.y* -80.0f), ForceMode2D.Impulse);

			//add some random perpindicular forces so explosions look more asymmetric
			split1.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(Random.Range(-80,-40) * m,Random.Range(40,80) * m));
			split2.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(Random.Range(40,80) * m,Random.Range(-80,-40) * m));

			//maintain the current momentum of the asteroid by applying the current velocity to the splits
			Vector2 currVelocity = gameObject.GetComponent<Rigidbody2D>().velocity;
			split1.GetComponent<Rigidbody2D>().AddForce(currVelocity * m, ForceMode2D.Impulse);
			split2.GetComponent<Rigidbody2D>().AddForce(currVelocity * m, ForceMode2D.Impulse);

			//spin them a little bit to sell the asymmetry between splits a bit more
			split1.GetComponent<Rigidbody2D>().AddTorque(Mathf.PI/Random.Range (1,8));
			split2.GetComponent<Rigidbody2D>().AddTorque(Mathf.PI/Random.Range (1,8));

			if(!fromLaser)
			{
				HandleAsteroidSplitExplosion();
			}

			//destroy the parent asteroid gameobject
			Destroy(gameObject);
		}
        


	}
	private void HandleExplosion()
	{
		float blastRadius = 16.0f;
		float blastForce = 50f*mass;
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll( transform.position, blastRadius );
		
		foreach( Collider2D col in hitColliders )
		{
			Vector3 pos = col.transform.position;
			Vector2 impulse = pos - transform.position;
			
			impulse /= impulse.magnitude;
			impulse *= blastForce;
			
			float percent = ( blastRadius - Vector3.Distance( pos, transform.position ) ) / blastRadius;
			
			if( col.tag == "Ship" )
			{
				ShipScript ship = col.GetComponent<ShipScript>();
				ship.TakeHit( impulse * percent, pos );
				ship.ApplyDamage( 20.0f * mass * percent, 0.0f );
			}
			else if( col.tag == "Asteroid" )
			{
				//Rigidbody2D roidRb = col.GetComponent<Rigidbody2D>();
				//roidRb.AddForce( impulse * percent, ForceMode2D.Impulse );
				Satellite sat = col.GetComponent<Satellite>();
				sat.ApplyDamage( 10.0f * percent, Vector2.zero, pos, false );
			}
		}
        
	}
    private void HandleAsteroidSplitExplosion()
    {
        float blastRadius = mass * 2f;
        float blastForce = 40f * mass;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);

        foreach (Collider2D col in hitColliders)
        {
            Vector3 pos = col.transform.position;
            Vector2 impulse = pos - transform.position;

            impulse /= impulse.magnitude;
            impulse *= blastForce;

            float percent = (blastRadius - Vector3.Distance(pos, transform.position)) / blastRadius;

            if (col.tag == "Ship")
            {
                ShipScript ship = col.GetComponent<ShipScript>();
                ship.TakeHit(impulse * percent, pos);
                ship.ApplyDamage(5.0f * mass * percent, 0.0f);
            }
            else if (col.tag == "Satellite")
            {
                //Rigidbody2D roidRb = col.GetComponent<Rigidbody2D>();
                //roidRb.AddForce( impulse * percent, ForceMode2D.Impulse );
                Satellite sat = col.GetComponent<Satellite>();
                sat.ApplyDamage(10.0f * percent, Vector2.zero, pos, false);
            }
        }

    }
    public IEnumerator InvincibleTimer()
    {
        yield return new WaitForSeconds(1f);
        invincible = false;
    }
}
