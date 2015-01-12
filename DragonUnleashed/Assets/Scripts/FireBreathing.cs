using UnityEngine;
using System.Collections;

public class FireBreathing : MonoBehaviour {
    public ParticleEmitter FireInside;
    public ParticleEmitter FireOutside;
	private NetworkAgent na;
	// Use this for initialization
	void Start() {
        StopFire();
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

    public static void EnableFire()
    {
        GameObject.Find("GameManager").GetComponent<PhotonView>().RPC("RPCEnableFire", PhotonTargets.All);
    }
    public static void DisableFire()
    {
        GameObject.Find("GameManager").GetComponent<PhotonView>().RPC("RPCDisableFire", PhotonTargets.All);
    }

	[RPC]
    private void RPCDisableFire()
    {
        FireInside.emit = false;
        FireOutside.emit = false;
    }

	[RPC]
    private void RPCEnableFire()
    {
        FireInside.emit = true;
        FireOutside.emit = true;
    }
}
