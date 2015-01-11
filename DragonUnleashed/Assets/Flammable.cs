using UnityEngine;
using System.Collections;

public class Flammable : MonoBehaviour {

	public float StartingIntegrity = 100.0f;

	[HideInInspector]
    public float Integrity;
    private float BurninationLevel;

	// Use this for initialization
	void Start () {
		Integrity = StartingIntegrity;

	}
	
	// Update is called once per frame
	void Update () {
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
        Integrity -= (BurninationLevel * Time.deltaTime);
        print("Integrity: " + Integrity);
        if (Integrity <= 0)
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
