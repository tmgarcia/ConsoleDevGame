using UnityEngine;
using System.Collections;

public class Flammable : Damageable
{
	private float BurninationLevel;
    public ParticleSystem fire;
    private ParticleSystem personalFire;
    private float ignitionThreshold = 0.15f;
    

	void Start()
	{
		CurrentIntegrity = StartingIntegrity;

        Vector3 firePosition = transform.position;
        float longSideLength = 1;

        if (gameObject.GetComponent<Collider>() != null)
        {
            firePosition = gameObject.collider.bounds.center + new Vector3(0, gameObject.collider.bounds.size.y / 2, 0);      // Places fire on top of object
            //firePosition = gameObject.renderer.bounds.center;                                                                   // Places fire in center of object
            longSideLength = Mathf.Max(gameObject.collider.bounds.size.x, gameObject.collider.bounds.size.z);

        }
       
        //reposition fire
        personalFire = Instantiate(fire, firePosition, Quaternion.identity) as ParticleSystem;
        personalFire.transform.Rotate(new Vector3(1, 0, 0), -90);
        personalFire.transform.parent = transform;

        //resize fire 
       
        personalFire.transform.localScale *= longSideLength;

        //personalFire.emissionRate = 20 * longSideLength;

        







	}

	void Update()
	{
        //AdjustFlameParticles();
        if (BurninationLevel > ignitionThreshold)
        {
            AdjustFlameParticles();
            ReduceIntegrity();
            ReduceBurnination();
        }
        else if(personalFire.enableEmission)
        {
            DisableFlameParticles();
        }

		if (damageRole == DamageRole.Villager)
		{
			base.Update();
		}
	}

    private void AdjustFlameParticles()
    {
        if (!personalFire.enableEmission)
        {
            EnableFlameParticles();
        }
        personalFire.emissionRate = BurninationLevel ; // *multiplier?
        print(personalFire.emissionRate);
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
		CurrentIntegrity -= (BurninationLevel * Time.deltaTime);
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
			Destroy(this.gameObject);
		}
        Destroy(personalFire, 2.0f);
        BurninationLevel = 0;

	}
}
