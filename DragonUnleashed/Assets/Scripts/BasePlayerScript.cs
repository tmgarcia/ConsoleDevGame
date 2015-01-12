using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BasePlayerScript : MonoBehaviour 
{
    public enum CharacterRole { Unassigned, Hider, Seeker}
    public int playerID;
    public bool ready;
    public bool isLocalPlayer;
    public GameObject setupPlayerLabel;

    public GameObject PlayerSelfLabelPrefab;
    public GameObject PlayerOtherLabelPrefab;

    private CharacterRole _role;
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
                setupPlayerLabel.transform.FindChild("PlayerID").GetComponent<Text>().text = "Player " + playerID + " (You)";
            }
            else
            {
                setupPlayerLabel = (GameObject)Instantiate(PlayerOtherLabelPrefab);
                setupPlayerLabel.name = "PlayerOther" + playerID;
                setupPlayerLabel.transform.FindChild("PlayerID").GetComponent<Text>().text = "Player " + playerID;
                
            }
            completedInitialSetup = true;
        }
    }

    public void InitializeGame()
    {
        if(Role == CharacterRole.Hider)
        {
            playerCharacter = (GameObject)PhotonNetwork.Instantiate("Villager", new Vector3(Random.Range(200, 220), 1, Random.Range(160, 180)), Quaternion.identity, 0);
            playerCharacter.GetComponent<NetworkAgent>().playerID = playerID;
        }
        else if (Role == CharacterRole.Seeker)
        {
            playerCharacter = (GameObject)PhotonNetwork.Instantiate("Dragon", new Vector3(Random.Range(200, 220), 10, Random.Range(160, 180)), Quaternion.identity, 0);
            playerCharacter.GetComponent<NetworkAgent>().playerID = playerID;
        }
    }

    public CharacterRole Role
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
