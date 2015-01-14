using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameSetupManager : Photon.MonoBehaviour
{
    public GameObject HidersPanel;
    public GameObject SeekersPanel;
    public GameObject UnassignedPanel;
    public GameObject StartGameButton;

    private static PhotonView ScenePhotonView;
    private static PlayersManager playersManager;

	//private bool isOculusConnected;

	//private void CheckOculusConnection()
	//{
	//	isOculusConnected = Ovr.Hmd.Detect() > 0;
	//}

	private bool IsOculusConnected()
	{
		return Ovr.Hmd.Detect() > 0;
	}

	void Awake()
	{
		print("Oculus Connection: " + IsOculusConnected());
	}

	void Start () 
    {
		//CheckOculusConnection();
		//print("connection: " + (Ovr.Hmd.Detect() > 0));
		//OVRManager.HMDAcquired += CheckOculusConnection;
		//OVRManager.HMDLost += CheckOculusConnection;

		print("Oculus Connection: " + IsOculusConnected());

        ScenePhotonView = this.GetComponent<PhotonView>();
        playersManager = this.GetComponent<PlayersManager>();
	}
	
	void Update () 
    {
        if (PhotonNetwork.inRoom && GameObject.Find("GameSetupUI")!=null)
        {
            GameObject.Find("SeekersLabel").GetComponent<Text>().text = "Seekers " + playersManager.currentNumSeekers + "/" + playersManager.maxNumSeekers;
            GameObject.Find("HidersLabel").GetComponent<Text>().text = "Hiders " + playersManager.currentNumHiders + "/" + playersManager.maxNumHiders;
            GameObject.Find("UnassignedLabel").GetComponent<Text>().text = "Unassigned " + playersManager.currentNumUnassigned;
        }
        if (PhotonNetwork.isMasterClient)
        {
            StartGameButton.SetActive(playersManager.AllPlayersAreReady());
        }
	}
    public void SetSelfToReady()
    {
        SetToReady(GetComponent<PlayersManager>().localPlayerID);
        SetPlayerToReady(GetComponent<PlayersManager>().localPlayerID);
    }
    public void SetSelfToNotReady()
    {
        SetToNotReady(GetComponent<PlayersManager>().localPlayerID);
        SetPlayerToNotReady(GetComponent<PlayersManager>().localPlayerID);
    }
    public void MoveSelfToHiders()
    {
        SetToNotReady(playersManager.localPlayerID);
        MoveToRole(playersManager.localPlayerID, PlayerRole.Villager);
        MovePlayerToHiders(playersManager.localPlayerID);
    }
    public void MoveSelfToSeekers()
    {
        SetToNotReady(playersManager.localPlayerID);
        MoveToRole(playersManager.localPlayerID, PlayerRole.Dragon);
        MovePlayerToSeekers(playersManager.localPlayerID);
    }
    public void MoveSelfToUnassigned()
    {
        SetToNotReady(playersManager.localPlayerID);
        MoveToRole(playersManager.localPlayerID, PlayerRole.Unassigned);
        MovePlayerToUnassigned(playersManager.localPlayerID);
    }
    public void MoveToRole(int playerID, PlayerRole role)
    {
        switch (role)
        {
            case PlayerRole.Villager:
                MoveToHiders(playerID);
                break;
            case PlayerRole.Dragon:
                MoveToSeekers(playerID);
                break;
            case PlayerRole.Unassigned:
                MoveToUnassigned(playerID);
                break;
        }
    }
    [RPC]
    public void MoveToHiders(int playerID)
    {
        BasePlayerScript playerToMove = playersManager.GetPlayer(playerID).GetComponent<BasePlayerScript>();
        if (HidersPanel.transform.FindChild(playerToMove.setupPlayerLabel.name) != null)
        {
            if (playerToMove.isLocalPlayer)
            {
                GameObject.Find("OtherDebugText").GetComponent<Text>().text = "You are already a Hider!";
            }
        }
        else
        {
            if (playersManager.currentNumHiders < playersManager.maxNumHiders)
            {
                playersManager.AdjustRoleNumbersForMovedPlayer(playerToMove.Role, PlayerRole.Villager);
                playerToMove.Role = PlayerRole.Villager;
                playerToMove.setupPlayerLabel.transform.SetParent(HidersPanel.transform, false);
				playerToMove.playerReadyButton.SetActive(true);
            }
            else
            {
                if (playerToMove.isLocalPlayer)
                {
                    GameObject.Find("OtherDebugText").GetComponent<Text>().text = "Hiders Full!";
                }
            }
        }
    }
    [RPC]
    public void MoveToSeekers(int playerID)
    {
        BasePlayerScript playerToMove = playersManager.GetPlayer(playerID).GetComponent<BasePlayerScript>();
        if (SeekersPanel.transform.FindChild(playerToMove.setupPlayerLabel.name) != null)
        {
            if (playerToMove.isLocalPlayer)
            {
                GameObject.Find("OtherDebugText").GetComponent<Text>().text = "You are already a Seeker!";
            }
        }
        else
        {
            if (playersManager.currentNumSeekers < playersManager.maxNumSeekers )
            {
                playersManager.AdjustRoleNumbersForMovedPlayer(playerToMove.Role, PlayerRole.Dragon);
                playerToMove.Role = PlayerRole.Dragon;
                playerToMove.setupPlayerLabel.transform.SetParent(SeekersPanel.transform, false);
				playerToMove.playerReadyButton.SetActive(true);
            }
            else
            {
                if (playerToMove.isLocalPlayer)
                {
                    GameObject.Find("OtherDebugText").GetComponent<Text>().text = "Seekers Full!";
                }
            }
        }
    }
    [RPC]
    public void MoveToUnassigned(int playerID)
    {
        BasePlayerScript playerToMove = playersManager.GetPlayer(playerID).GetComponent<BasePlayerScript>();
        if (UnassignedPanel.transform.FindChild(playerToMove.setupPlayerLabel.name) != null)
        {
            if (playerToMove.isLocalPlayer)
            {
                GameObject.Find("OtherDebugText").GetComponent<Text>().text = "You are already Unassigned!";
            }
        }
        else
        {
            playersManager.AdjustRoleNumbersForMovedPlayer(playerToMove.Role, PlayerRole.Unassigned);
            playerToMove.Role = PlayerRole.Unassigned;
            playerToMove.setupPlayerLabel.transform.SetParent(UnassignedPanel.transform, false);
			playerToMove.playerReadyButton.SetActive(false);
        }
    }
    [RPC]
    public void SetToReady(int playerID)
    {
        BasePlayerScript playerToMove = playersManager.GetPlayer(playerID).GetComponent<BasePlayerScript>();
        if (playerToMove.isLocalPlayer)
        {
            playerToMove.setupPlayerLabel.transform.FindChild("ReadyButton").FindChild("ReadyButtonText").GetComponent<Text>().text = "Ready.";
            playerToMove.setupPlayerLabel.transform.FindChild("ReadyButton").GetComponent<Button>().enabled = false;
        }
        else
        {
            playerToMove.setupPlayerLabel.transform.FindChild("ReadyText").GetComponent<Text>().text = "Ready.";
        }
        playerToMove.ready = true;
    }
    [RPC]
    public void SetToNotReady(int playerID)
    {
        BasePlayerScript playerToMove = GetComponent<PlayersManager>().GetPlayer(playerID).GetComponent<BasePlayerScript>();
        if (playerToMove.isLocalPlayer)
        {
            playerToMove.setupPlayerLabel.transform.FindChild("ReadyButton").FindChild("ReadyButtonText").GetComponent<Text>().text = "Ready?";
            playerToMove.setupPlayerLabel.transform.FindChild("ReadyButton").GetComponent<Button>().enabled = true;
        }
        else
        {
            playerToMove.setupPlayerLabel.transform.FindChild("ReadyText").GetComponent<Text>().text = "Waiting";
        }
        playerToMove.ready = false;
    }

    public static void MovePlayerToHiders(int playerID)
    {
        ScenePhotonView.RPC("MoveToHiders", PhotonTargets.OthersBuffered, playerID);
    }
    public static void MovePlayerToSeekers(int playerID)
    {
        ScenePhotonView.RPC("MoveToSeekers", PhotonTargets.OthersBuffered, playerID);
    }
    public static void MovePlayerToUnassigned(int playerID)
    {
        ScenePhotonView.RPC("MoveToUnassigned", PhotonTargets.OthersBuffered, playerID);
    }
    public static void SetPlayerToReady(int playerID)
    {
        ScenePhotonView.RPC("SetToReady", PhotonTargets.OthersBuffered, playerID);
    }
    public static void SetPlayerToNotReady(int playerID)
    {
        ScenePhotonView.RPC("SetToNotReady", PhotonTargets.OthersBuffered, playerID);
    }
    public void StartGame()
    {
        ScenePhotonView.RPC("InitializePlayers", PhotonTargets.All);
    }
}
