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
                setupPlayerLabel.transform.FindChild("ReadyButton").GetComponent<Button>().onClick.AddListener(() => { GameObject.Find("GameManager").GetComponent<GameSetupManager>().SetSelfToReady(); });
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
