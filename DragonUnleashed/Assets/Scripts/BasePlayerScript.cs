using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BasePlayerScript : MonoBehaviour 
{
    public int playerID;
    public bool ready;
    public bool isLocalPlayer;
    public GameObject setupPlayerLabel;
	public GameObject playerReadyButton;

    public GameObject PlayerSelfLabelPrefab;
    public GameObject PlayerOtherLabelPrefab;

    private PlayerRole _role;
    private bool completedInitialSetup = false;

    public GameObject villagerPrefab;
    public GameObject dragonPrefab;

    public GameObject playerCharacter;

	void Start () 
    {
	    
	}
	
	void Update () 
    {
	    
	}

    public void InitialSetup()
    {
        if (!completedInitialSetup)
        {
            if (isLocalPlayer)
            {
                setupPlayerLabel = (GameObject)Instantiate(PlayerSelfLabelPrefab);
                setupPlayerLabel.name = "PlayerSelf";
                setupPlayerLabel.transform.FindChild("ReadyButton").GetComponent<Button>().onClick.AddListener(() => { GameObject.FindObjectOfType<GameSetupManager>().SetSelfToReady(); });
				playerReadyButton = setupPlayerLabel.transform.FindChild("ReadyButton").GetComponent<Button>().gameObject;
                setupPlayerLabel.transform.FindChild("PlayerID").GetComponent<Text>().text = "Player " + playerID + " (You)";
            }
            else
            {
                setupPlayerLabel = (GameObject)Instantiate(PlayerOtherLabelPrefab);
                setupPlayerLabel.name = "PlayerOther" + playerID;
                //playerReadyButton = setupPlayerLabel.transform.FindChild("ReadyButton").GetComponent<Button>().gameObject;
                setupPlayerLabel.transform.FindChild("PlayerID").GetComponent<Text>().text = "Player " + playerID;
                
            }
            completedInitialSetup = true;
        }
    }

    public void InitializeGame()
    {
        if(Role == PlayerRole.Villager)
        {
            playerCharacter = (GameObject)PhotonNetwork.Instantiate("Villager", GameObject.Find("RespawnManager").GetComponent<RespawnManager>().GetRandomSpawn(Role), Quaternion.identity, 0);
            playerCharacter.GetComponent<NetworkAgent>().playerID = playerID;
            Screen.lockCursor = true;
        }
	else if (Role == PlayerRole.Dragon)
        {
            //playerCharacter = (GameObject)PhotonNetwork.Instantiate("Dragon", GameObject.Find("RespawnManager").GetComponent<RespawnManager>().GetRandomSpawn(Role), Quaternion.identity, 0);
            playerCharacter = (GameObject)PhotonNetwork.Instantiate("DragonOVR", GameObject.Find("RespawnManager").GetComponent<RespawnManager>().GetRandomSpawn(Role), Quaternion.identity, 0);
            playerCharacter.GetComponent<NetworkAgent>().playerID = playerID;
            Screen.lockCursor = true;
        }
    }

    public PlayerRole Role
    {
        get
        {
            return _role;
        }
        set
        {
            _role = value;
        }
    }
}
