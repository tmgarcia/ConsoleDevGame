using UnityEngine;
using System.Collections;

public class FireParticleCollision : MonoBehaviour {
    public float FlameIntensity;
	// Use this for initialization
	void Start () {
        FlameIntensity = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnParticleCollision(GameObject other)
    {
        //print("Collision with " + other.name);
        

        if (other.gameObject.GetComponents<Flammable>().Length != 0)
        {
            (other.gameObject.GetComponent("Flammable") as Flammable).Burn(FlameIntensity);
        }

    }
}
