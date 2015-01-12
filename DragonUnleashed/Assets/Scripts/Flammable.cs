using UnityEngine;
using System.Collections;

public class Flammable : Damageable
{
	private float BurninationLevel;

	void Start()
	{
		CurrentIntegrity = StartingIntegrity;
	}

	void Update()
	{
		if (BurninationLevel > 0)
		{
			ReduceIntegrity();
			ReduceBurnination();
		}
	}

	private void ReduceBurnination()
	{
		BurninationLevel -= (BurninationLevel * Time.deltaTime);
	}
	private void ReduceIntegrity()
	{
		CurrentIntegrity -= (BurninationLevel * Time.deltaTime);
		print("CurrentIntegrity: " + CurrentIntegrity);
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
		Destroy(this.gameObject);
	}
}
