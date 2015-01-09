using UnityEngine;
using System.Collections;

public class DragonMovement : MonoBehaviour {

    private PhotonView photonView;
    private GameObject cam;

	// Use this for initialization
	void Start () {
        photonView = gameObject.GetComponent<PhotonView>();
        PhotonNetwork.ConnectUsingSettings("0.1");
        cam = transform.FindChild("DragonCamera").gameObject;
	}

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    void OnJoinedLobby()
    {
        Debug.Log("JoinRandom");
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }

    void OnJoinedRoom()
    {
        if (photonView.isMine)
        {
            cam.GetComponent<Camera>().enabled = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
