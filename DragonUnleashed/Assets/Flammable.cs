using UnityEngine;
using System.Collections;

public class Flammable : MonoBehaviour {

    private float Integrity;
    private float BurninationLevel;

	// Use this for initialization
	void Start () {
        Integrity = 100.0f;

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
