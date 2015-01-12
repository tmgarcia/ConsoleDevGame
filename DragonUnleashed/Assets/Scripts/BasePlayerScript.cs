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
            playerCharacter = (GameObject)Instantiate(villagerPrefab);
            playerCharacter.transform.position = new Vector3(Random.Range(10, 20), 1, Random.Range(10, 20));
        }
        else if (Role == CharacterRole.Seeker)
        {
            playerCharacter = (GameObject)Instantiate(dragonPrefab);
            playerCharacter.transform.position = new Vector3(Random.Range(10, 20), 10, Random.Range(10, 20));
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
