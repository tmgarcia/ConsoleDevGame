using UnityEngine;
using System.Collections;

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
