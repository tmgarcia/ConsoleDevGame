using UnityEngine;
using System.Collections;

public class NetworkAgent : Photon.MonoBehaviour
{
    private Vector3 networkPosition;
    private Quaternion networkRotation;

	// Use this for initialization
	void Start () 
    {
	    
	}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.FindChild("DragonHead").transform.position);
            stream.SendNext(transform.FindChild("DragonHead").transform.rotation);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    public Vector3 GetNetworkPosition()
    {
        return networkPosition;
    }

    public Quaternion GetNetworkRotation()
    {
        return networkRotation;
    }

    public bool IsLocalCharacter()
    {
        return photonView.isMine;
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
