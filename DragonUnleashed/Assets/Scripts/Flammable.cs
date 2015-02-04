using UnityEngine;
using System.Collections;

public class Flammable : Damageable
{
	public float BurninationLevel { get; set; }
	public ParticleSystem fire;
	public ParticleSystem dustCloud;
	public ParticleSystem dustPoof;
	private ParticleSystem personalFire;
	private ParticleSystem personalDust;
	private Vector3 firePosition;
	public float ignitionThreshold = 0.15f; // Arbitrary value entered, could possibly be lower. Must not be zero.
	private float volume;
	private float structureMinVolume = 35; // Just guessing. Needs further testing.

	void Start()
	{
		//gameObject.GetComponentInParent<Transform>().gameObject.name == "NewSheep(Clone)"
		//if (gameObject.GetComponentInParent<Transform>() != null)
		//{
		//	print(gameObject.GetComponentInParent<Transform>().gameObject.name);
		//}

		//print(gameObject.name);

		//if (transform.parent.gameObject.name != null && transform.parent.gameObject.name == "NewSheep(Clone)")
		//{
		//	print(transform.parent.gameObject.name);
		//}

		CurrentLocalIntegrity = StartingIntegrity;
		base.SetIntegrity(CurrentLocalIntegrity);
		volume = (collider.bounds.size.x * collider.bounds.size.y * collider.bounds.size.z);

		firePosition = transform.position;
		float longSideLength = 1.0f;

		if (gameObject.GetComponent<Collider>() != null)
		{
			firePosition = gameObject.collider.bounds.center + new Vector3(0, gameObject.collider.bounds.size.y / 2, 0);      // Places fire on top of object
			//firePosition = gameObject.renderer.bounds.center;                                                                   // Places fire in center of object
			longSideLength = Mathf.Max(gameObject.collider.bounds.size.x, gameObject.collider.bounds.size.z, gameObject.collider.bounds.size.y);

		}

		if (transform.childCount == 0 || transform.GetChild(0).name != "CustomFire1(Clone)")
		{
			personalFire = Instantiate(fire, firePosition, Quaternion.identity) as ParticleSystem;
			personalFire.transform.Rotate(new Vector3(1, 0, 0), -90);		//reposition fire
			personalFire.transform.parent = transform;
			personalFire.transform.localScale *= (2.0f * longSideLength);		//resize fire 
			personalFire.startSize = longSideLength;
		}
		else
		{
			personalFire = GetComponentInChildren<ParticleSystem>();
		}

	}

	void Update()
	{
		//AdjustFlameParticles();
		if (BurninationLevel > ignitionThreshold)
		{
			AdjustFlameParticles();
			ReduceIntegrity();
			ReduceBurnination();
			float percentHealth = (CurrentIntegrity / StartingIntegrity);
			// float percentBlackened = (1.0f - percentHealth);
			renderer.material.color = percentHealth * Color.white;
		}
		else if (personalFire.enableEmission)
		{
			DisableFlameParticles();
		}

		if (damageRole != DamageRole.Scenery)
		{
			base.DamageUpdate();
		}
		else
		{
			base.PropDamageUpdate();
		}
	}

	private void AdjustFlameParticles()
	{
		if (!personalFire.enableEmission)
		{
			EnableFlameParticles();
		}
		personalFire.emissionRate = BurninationLevel; // *multiplier?
		//print(personalFire.emissionRate);
	}

	private void EnableFlameParticles()
	{
		personalFire.enableEmission = true;

	}

	private void DisableFlameParticles()
	{
		personalFire.enableEmission = false;
	}

	private void ReduceBurnination()
	{
		BurninationLevel -= (0.5f * (BurninationLevel * Time.deltaTime));//////////////////////////////////////////////////Slow this down?
	}

	private void ReduceIntegrity()
	{
		CurrentLocalIntegrity -= (BurninationLevel * Time.deltaTime);
		//print("CurrentIntegrity: " + CurrentIntegrity);
		if (CurrentIntegrity <= 0)
		{
			Disintegrate();
		}
	}

	public void Burn(float flameIntensity)
	{
		//print("BURNINATING!!!!!!!!");
		BurninationLevel += flameIntensity;
	}

	private void Disintegrate()
	{
		//create ash particle effect
		//destroy object? Possibly replace with ash pile model

		if (damageRole == DamageRole.Scenery)
		{
			if (volume < structureMinVolume) // Check size to determine if building or prop
			{
				poof();
			}
			else
			{
				collapse();
			}
		}

		BurninationLevel = 0;

	}

	private void collapse() // Disposes of large structures by sinking and raising dust
	{
		//raise dust
		personalDust = Instantiate(dustCloud, firePosition, Quaternion.identity) as ParticleSystem;
		personalDust.transform.Rotate(new Vector3(1, 0, 0), -90);

		personalDust.transform.parent = transform;

		//sink
		Destroy(gameObject.collider);
		gameObject.AddComponent<Rigidbody>();
		Destroy(rigidbody, 3.5f);
		Destroy(gameObject, 5.0f);
		Destroy(personalFire, 5.0f);
		Destroy(personalDust, 5.0f);
	}

	private void poof() // Disposes of small objects with a poof of smoke
	{
		//create poof
		personalDust = Instantiate(dustPoof, collider.bounds.center, Quaternion.identity) as ParticleSystem;
		personalDust.transform.parent = transform;
		personalDust.transform.localScale *= (0.5f * volume);
		personalDust.emissionRate = 40.0f * volume;
		renderer.enabled = false;
		Destroy(collider);
		Destroy(gameObject, 10);
		Destroy(personalFire, 10.0f);
		Destroy(personalDust, 10.0f);

	}



}
