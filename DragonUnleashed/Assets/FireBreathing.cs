using UnityEngine;
using System.Collections;

public class FireBreathing : MonoBehaviour {
    public ParticleEmitter FireInside;
    public ParticleEmitter FireOutside;
	// Use this for initialization
	void Start () {
        DisableFire();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EnableFire();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            DisableFire();
        }
	}

    private void DisableFire()
    {
        FireInside.emit = false;
        FireOutside.emit = false;
    }

    private void EnableFire()
    {
        FireInside.emit = true;
        FireOutside.emit = true;
    }
}
