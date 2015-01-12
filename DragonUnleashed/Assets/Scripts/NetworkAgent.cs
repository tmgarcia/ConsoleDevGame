using UnityEngine;
using System.Collections;

public class NetworkAgent : Photon.MonoBehaviour
{
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    public GameObject agent;
    public int playerID;

	// Use this for initialization
	void Start () 
    {
	    
	}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(agent.transform.position);
            stream.SendNext(agent.transform.rotation);
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
