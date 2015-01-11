using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameSetupManager : Photon.MonoBehaviour
{
    public int maxNumHiders = 4;
    public int maxNumSeekers = 1;

    private static int currentNumHiders = 0;
    private static int currentNumSeekers = 0;

    public GameObject PlayerSelfLabelPrefab;
    public GameObject PlayerOtherLabelPrefab;

    private static PhotonView ScenePhotonView;

	// Use this for initialization
	void Start () 
    {
        ScenePhotonView = this.GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (PhotonNetwork.inRoom && GameObject.Find("GameSetupUI")!=null)
        {
            GameObject.Find("SeekersLabel").GetComponent<Text>().text = "Seekers " + currentNumSeekers + "/" + maxNumSeekers;
            GameObject.Find("HidersLabel").GetComponent<Text>().text = "Hiders " + currentNumHiders + "/" + maxNumHiders;
        }
	}


    public void MoveSelfToHiders()
    {
        //Already a hider
        if(GameObject.Find("HiderPlayersPanel").transform.FindChild("PlayerSelf")!=null)
        {
            GameObject.Find("OtherDebugText").GetComponent<Text>().text = "You are already a Hider!";
        }
        //Not already a Hider
        else
        {
            //Not at max num hiders
            if (currentNumHiders < maxNumHiders)
            {
                //Already assigned as seeker
                if (GameObject.Find("SeekerPlayersPanel").transform.FindChild("PlayerSelf") != null)
                {
                    var selfLabel = GameObject.Find("SeekerPlayersPanel").transform.FindChild("PlayerSelf").gameObject;
                    selfLabel.transform.SetParent(GameObject.Find("HiderPlayersPanel").transform, false);
                    selfLabel.transform.FindChild("ReadyButton").FindChild("ReadyButtonText").GetComponent<Text>().text = "Ready?";
                    selfLabel.transform.FindChild("ReadyButton").GetComponent<Button>().enabled = true;

                    currentNumSeekers -= 1;
                }
                //not yet assigned
                else
                {
                    var selfLabel = (GameObject)Instantiate(PlayerSelfLabelPrefab);
                    selfLabel.name = "PlayerSelf";
                    selfLabel.transform.SetParent(GameObject.Find("HiderPlayersPanel").transform, false);
                    selfLabel.transform.FindChild("ReadyButton").FindChild("ReadyButtonText").GetComponent<Text>().text = "Ready?";
                    selfLabel.transform.FindChild("ReadyButton").GetComponent<Button>().enabled = true;
                    selfLabel.transform.FindChild("PlayerID").GetComponent<Text>().text = "ID: " + PhotonNetwork.player.ID;
                }
                currentNumHiders += 1;
                MovePlayerToHiders(PhotonNetwork.player.ID);
            }
            //At max num hiders
            else
            {
                GameObject.Find("OtherDebugText").GetComponent<Text>().text = "Too many hiders! Wait for someone to switch or join as seeker.";
                if (currentNumSeekers >= maxNumSeekers)
                {
                    GameObject.Find("OtherDebugText").GetComponent<Text>().text = "Too many hiders!\nToo many seekers! Game full, sorry?";
                }
            }
        }
    }
    public void MoveSelfToSeekers()
    {
        //Already a seeker
        if (GameObject.Find("SeekerPlayersPanel").transform.FindChild("PlayerSelf") != null)
        {
            GameObject.Find("OtherDebugText").GetComponent<Text>().text = "You are already a Seeker!";
        }
        //Not already a seeker
        else
        {
            //Not at max num seekers
            if (currentNumSeekers < maxNumSeekers)
            {
                //Currently assigned as hider
                if (GameObject.Find("HiderPlayersPanel").transform.FindChild("PlayerSelf") != null)
                {
                    var selfLabel = GameObject.Find("HiderPlayersPanel").transform.FindChild("PlayerSelf").gameObject;
                    selfLabel.transform.SetParent(GameObject.Find("SeekerPlayersPanel").transform, false);
                    selfLabel.transform.FindChild("ReadyButton").FindChild("ReadyButtonText").GetComponent<Text>().text = "Ready?";
                    selfLabel.transform.FindChild("ReadyButton").GetComponent<Button>().enabled = true;

                    currentNumHiders -= 1;
                }
                 //not yet assigned to role
                else
                {
                    var selfLabel = (GameObject)Instantiate(PlayerSelfLabelPrefab);
                    selfLabel.name = "PlayerSelf";
                    selfLabel.transform.SetParent(GameObject.Find("SeekerPlayersPanel").transform, false);
                    selfLabel.transform.FindChild("ReadyButton").FindChild("ReadyButtonText").GetComponent<Text>().text = "Ready?";
                    selfLabel.transform.FindChild("ReadyButton").GetComponent<Button>().enabled = true;
                    selfLabel.transform.FindChild("PlayerID").GetComponent<Text>().text = "ID: " + PhotonNetwork.player.ID;
                }
                currentNumSeekers += 1;
                MovePlayerToSeekers(PhotonNetwork.player.ID);
            }
            //Too many seekers
            else
            {
                GameObject.Find("OtherDebugText").GetComponent<Text>().text = "Too many seekers! Wait for someone to switch or join as hider.";
                //Too many hiders AND seekers (shouldn't hit this, adjust max num hiders or seekers for bigger room
                if (currentNumHiders >= maxNumHiders)
                {
                    GameObject.Find("OtherDebugText").GetComponent<Text>().text = "Too many seekers!\nToo many Hiders!\n Game full, sorry?";
                }
            }
        }
    }

    public static void MovePlayerToHiders(int playerID)
    {
        ScenePhotonView.RPC("MovePlayerLabelToHiders", PhotonTargets.OthersBuffered, playerID);
    }
    public static void MovePlayerToSeekers(int playerID)
    {
        ScenePhotonView.RPC("MovePlayerLabelToSeekers", PhotonTargets.OthersBuffered, playerID);
    }

    [RPC]
    void MovePlayerLabelToHiders(int playerID)
    {
        if (playerID != PhotonNetwork.player.ID)
        {
            string playerOtherName = "PlayerOther" + playerID;
            if (GameObject.Find("HiderPlayersPanel").transform.FindChild(playerOtherName) != null)
            {
                //do nothing
            }
            //They were a seeker
            else if (GameObject.Find("SeekerPlayersPanel").transform.FindChild(playerOtherName) != null)
            {
                var playerLabel = GameObject.Find("SeekerPlayersPanel").transform.FindChild(playerOtherName).gameObject;
                playerLabel.transform.SetParent(GameObject.Find("HiderPlayersPanel").transform, false);
                playerLabel.transform.FindChild("ReadyText").GetComponent<Text>().text = "Waiting";
                currentNumHiders += 1;
                currentNumSeekers -= 1;
            }
            //They were not assigned
            else
            {
                var playerLabel = (GameObject)Instantiate(PlayerOtherLabelPrefab);
                playerLabel.name = playerOtherName;
                playerLabel.transform.SetParent(GameObject.Find("HiderPlayersPanel").transform, false);
                playerLabel.transform.FindChild("ReadyText").GetComponent<Text>().text = "Waiting";
                playerLabel.transform.FindChild("PlayerID").GetComponent<Text>().text = "ID: " + playerID;
                currentNumHiders += 1;
            }
        }
    }
    [RPC]
    void MovePlayerLabelToSeekers(int playerID)
    {
        if (playerID != PhotonNetwork.player.ID)
        {
            string playerOtherName = "PlayerOther" + playerID;
            if (GameObject.Find("SeekerPlayersPanel").transform.FindChild(playerOtherName) != null)
            {
                //do nothing
            }
            else if (GameObject.Find("HiderPlayersPanel").transform.FindChild(playerOtherName) != null)
            {
                var playerLabel = GameObject.Find("HiderPlayersPanel").transform.FindChild(playerOtherName).gameObject;
                playerLabel.transform.SetParent(GameObject.Find("SeekerPlayersPanel").transform, false);
                playerLabel.transform.FindChild("ReadyText").GetComponent<Text>().text = "Waiting";
                currentNumSeekers += 1;
                currentNumHiders -= 1;
            }
            else
            {
                var playerLabel = (GameObject)Instantiate(PlayerOtherLabelPrefab);
                playerLabel.name = playerOtherName;
                playerLabel.transform.SetParent(GameObject.Find("SeekerPlayersPanel").transform, false);
                playerLabel.transform.FindChild("ReadyText").GetComponent<Text>().text = "Waiting";
                playerLabel.transform.FindChild("PlayerID").GetComponent<Text>().text = "ID: " + playerID;
                currentNumSeekers += 1;
            }
        }
    }
}
