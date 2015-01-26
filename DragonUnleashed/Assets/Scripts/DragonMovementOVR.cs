using UnityEngine;
using System.Collections;

public class DragonMovementOVR : MonoBehaviour 
{
    private PhotonView photonView;
    public bool isLocal = false;

	void Start () 
    {
        photonView = transform.parent.gameObject.GetComponent<PhotonView>();
        isLocal = transform.parent.GetComponent<NetworkAgent>().IsLocalCharacter();
	}
	
	void Update () 
    {

	}
}
