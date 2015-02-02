using UnityEngine;
using System.Collections;

public class FireBreathing : Photon.MonoBehaviour
{
    public ParticleSystem Fire;
    //public ParticleEmitter FireInside;
    //public ParticleEmitter FireOutside;
    private static PhotonView photonView;
	private NetworkAgent na;
	// Use this for initialization
	void Start() {
        RPCDisableFire();
		na = gameObject.GetComponent<NetworkAgent>();
        photonView = gameObject.GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update()
	{
		if (na.IsLocalCharacter())
		{
            if (OVRManagerHelper.instance.IsLocalPlayerUsingOVR)
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
            else
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    EnableFire();
                }
                else if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
                {
                    DisableFire();
                }
            }
		}
	}

    public static void EnableFire()
    {
        photonView.RPC("RPCEnableFire", PhotonTargets.All);
         //.GetComponent<PhotonView>().RPC("RPCEnableFire", PhotonTargets.All);
    }
    public static void DisableFire()
    {
        photonView.RPC("RPCDisableFire", PhotonTargets.All);
    }

	[RPC]
    private void RPCDisableFire()
    {
        Fire.enableEmission = false;
        //FireInside.emit = false;
        //FireOutside.emit = false;
    }

	[RPC]
    private void RPCEnableFire()
    {
        Fire.enableEmission = true;
        //FireInside.emit = true;
        //FireOutside.emit = true;
    }
}
