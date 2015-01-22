using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoomManager : Photon.MonoBehaviour
{
    bool hasConnected = false;
    Text NetworkStatusText;
    Text NetworkDebugText;
    public byte Version = 1;
    int maxNumPlayers = 5;

	void Start () 
    {
        PhotonNetwork.autoJoinLobby = false;
        Screen.fullScreen = false; 
        GameObject.Find("GameSetupUI").GetComponent<Canvas>().enabled = false;
        NetworkStatusText = GameObject.Find("NetworkStatusText").GetComponent<Text>();
        NetworkDebugText = GameObject.Find("NetworkDebugText").GetComponent<Text>();
        NetworkDebugText.text = "Start()";
    }
	
	void Update () 
    {
	    if (!hasConnected && !PhotonNetwork.connected)
        {
            Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
            
            hasConnected = true;
            PhotonNetwork.ConnectUsingSettings(Version + "."+Application.loadedLevel);
            NetworkDebugText.text = "Initial connection to Photon";
        }
        NetworkStatusText.text = PhotonNetwork.connectionStateDetailed.ToString();
	}

    public virtual void OnConnectedToMaster()
    {
        NetworkDebugText.text = "Connected to Photon, joining random room";
        PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnPhotonRandomJoinFailed()
    {
        NetworkDebugText.text = "No existing random room, creating new room";
        PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = maxNumPlayers }, null);
    }


    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        NetworkDebugText.text = "Failed to connect to photon\nCause: " + cause;
    }

    public void OnJoinedRoom()
    {
        NetworkDebugText.text = "Room successfully joined";
        GameObject.Find("GameSetupUI").GetComponent<Canvas>().enabled = true;
        GameObject.Find("SelfIDText").GetComponent<Text>().text = "Your Player ID: " + PhotonNetwork.player.ID;
        GetComponent<PlayersManager>().localPlayerID = PhotonNetwork.player.ID;
        PlayersManager.NewPlayer(PhotonNetwork.player.ID);
        //gameObject.GetComponent<GameSetupManager>().MoveSelfToHiders();
        if (OVRManagerHelper.instance.IsLocalPlayerUsingOVR)
        {
            OVRManagerHelper.instance.readyToPlay = true;
            print("Room created and instance ready");
        }
    }
    public void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        GetComponent<PlayersManager>().RemoveExistingPlayer(other.ID);
    }
   
}
