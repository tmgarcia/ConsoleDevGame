using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayersManager : MonoBehaviour 
{
    public int maxNumHiders = 4;
    public int maxNumSeekers = 1;

    public int localPlayerID;

    public int currentNumHiders = 0;
    public int currentNumSeekers = 0;
    public int currentNumUnassigned = 0;

    public List<GameObject> playerObjects;

    public GameObject PlayerCharacterPrefab;

    private static PhotonView ScenePhotonView;

    void Start()
    {
        ScenePhotonView = this.GetComponent<PhotonView>();
    }
	
	// Update is called once per frame
	void Update () 
    {
	    
	}
    public void RemoveExistingPlayer(int playerID)
    {
        GameObject playerToRemove = GetPlayer(playerID);
        switch (playerToRemove.GetComponent<BasePlayerScript>().Role)
        {
            case BasePlayerScript.CharacterRole.Hider:
                currentNumHiders -= 1;
                break;
            case BasePlayerScript.CharacterRole.Seeker:
                currentNumSeekers -= 1;
                break;
            case BasePlayerScript.CharacterRole.Unassigned:
                currentNumUnassigned -= 1;
                break;
        }
        playerToRemove.GetComponent<BasePlayerScript>().setupPlayerLabel.SetActive(false);
        playerObjects.Remove(playerToRemove);
        Destroy(playerToRemove);
    }
    public static void NewPlayer(int playerID)
    {
        ScenePhotonView.RPC("AddNewPlayer", PhotonTargets.AllBuffered, playerID);
        print("NewPlayer");
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
    [RPC]
    public void AddNewPlayer(int playerID)
    {
        print("AddNewPlayer");
        var newPlayer = (GameObject)Instantiate(PlayerCharacterPrefab);
        BasePlayerScript newPlayerBase = newPlayer.GetComponent<BasePlayerScript>();
        newPlayerBase.playerID = playerID;
        newPlayerBase.isLocalPlayer = (playerID == localPlayerID);
        newPlayerBase.ready = false;
        newPlayerBase.Role = BasePlayerScript.CharacterRole.Unassigned;
        newPlayerBase.InitialSetup();
        playerObjects.Add(newPlayer);
        GetComponent<GameSetupManager>().MoveToRole(playerID, BasePlayerScript.CharacterRole.Unassigned);
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
    public void AdjustRoleNumbersForMovedPlayer(BasePlayerScript.CharacterRole oldRole, BasePlayerScript.CharacterRole newRole)
    {
        switch (oldRole)
        {
            case BasePlayerScript.CharacterRole.Hider:
                currentNumHiders -= 1;
                break;
            case BasePlayerScript.CharacterRole.Seeker:
                currentNumSeekers -= 1;
                break;
            case BasePlayerScript.CharacterRole.Unassigned:
                currentNumUnassigned -= 1;
                break;
        }
        switch (newRole)
        {
            case BasePlayerScript.CharacterRole.Hider:
                currentNumHiders += 1;
                break;
            case BasePlayerScript.CharacterRole.Seeker:
                currentNumSeekers += 1;
                break;
            case BasePlayerScript.CharacterRole.Unassigned:
                currentNumUnassigned += 1;
                break;
        }
    }
}
