using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayersManager : MonoBehaviour 
{
    public int maxNumHiders = 4;
    public int maxNumSeekers = 1;

    private static int currentNumHiders = 0;
    private static int currentNumSeekers = 0;
    private static int currentNumUnassigned = 0;

    public List<GameObject> playerObjects;

    public GameObject PlayerCharacterPrefab;

	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public GameObject GetPlayer(int playerID)
    {
        GameObject player = playerObjects.FirstOrDefault(p => p.GetComponent<BasePlayerScript>().playerID == playerID);
        return player;
    }
    public BasePlayerScript.CharacterRole GetPlayerRole(int playerID)
    {
        GameObject player = playerObjects.FirstOrDefault(p => p.GetComponent<BasePlayerScript>().playerID == playerID);
        return player.GetComponent<BasePlayerScript>().Role;
    }
    public void SetPlayerRole(int playerID, BasePlayerScript.CharacterRole role)
    {
        GameObject player = playerObjects.FirstOrDefault(p => p.GetComponent<BasePlayerScript>().playerID == playerID);
        player.GetComponent<BasePlayerScript>().Role = role;
    }
    public void AddNewPlayer(int playerID, bool isLocalPlayer, bool ready)
    {
        var newPlayer = (GameObject)Instantiate(PlayerCharacterPrefab);
        BasePlayerScript newPlayerBase = newPlayer.GetComponent<BasePlayerScript>();
        newPlayerBase.playerID = playerID;
        newPlayerBase.isLocalPlayer = isLocalPlayer;
        newPlayerBase.ready = ready;
        newPlayerBase.Role = BasePlayerScript.CharacterRole.Unassigned;
        newPlayerBase.InitialSetup();
        currentNumUnassigned += 1;
    }
    public bool AllPlayersAreReady()
    {
        bool allReady = true;
        foreach (GameObject player in playerObjects)
        {
            if (!player.GetComponent<BasePlayerScript>().ready)
            {
                allReady = false;
            }
        }
        return allReady;
    }
}
