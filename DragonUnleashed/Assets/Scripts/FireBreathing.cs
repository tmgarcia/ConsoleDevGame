using UnityEngine;
using System.Collections;

public class FireBreathing : MonoBehaviour {
    public ParticleEmitter FireInside;
    public ParticleEmitter FireOutside;
	private NetworkAgent na;
	// Use this for initialization
	void Start() {
        DisableFire();
		na = gameObject.GetComponentInParent<NetworkAgent>();
	}
	
	// Update is called once per frame
	void Update()
	{
		if (na.IsLocalCharacter())
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				EnableFire();
			}
			else if (Input.GetKeyUp(KeyCode.Space))
			{
				DisableFire();
			}
		}
	}

	[RPC]
    private void DisableFire()
    {
        FireInside.emit = false;
        FireOutside.emit = false;
    }

	[RPC]
    private void EnableFire()
    {
        FireInside.emit = true;
        FireOutside.emit = true;
    }
}
