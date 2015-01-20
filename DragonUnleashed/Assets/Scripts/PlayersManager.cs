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
	public static PlayersManager instance;

	public GameObject HUDManager;

	public GameObject dragonPlayer;

	void Start()
	{
		if (instance == null)
		{
			instance = this;
		}

		ScenePhotonView = this.GetComponent<PhotonView>();
		HUDManager = GameObject.Find("HUDManager");
		HUDManager.SetActive(false);
	}

	public void RemoveExistingPlayer(int playerID)
	{
		GameObject playerToRemove = GetPlayer(playerID);
		switch (playerToRemove.GetComponent<BasePlayerScript>().Role)
		{
			case PlayerRole.Villager:
				currentNumHiders -= 1;
				break;
			case PlayerRole.Dragon:
				currentNumSeekers -= 1;
				break;
			case PlayerRole.Unassigned:
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

	public PlayerRole GetPlayerRole(int playerID)
	{
		GameObject player = playerObjects.FirstOrDefault(p => p.GetComponent<BasePlayerScript>().playerID == playerID);
		return player.GetComponent<BasePlayerScript>().Role;
	}

	public void SetPlayerRole(int playerID, PlayerRole role)
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
		newPlayerBase.Role = PlayerRole.Unassigned;
		newPlayerBase.InitialSetup();
		playerObjects.Add(newPlayer);
		GetComponent<GameSetupManager>().MoveToRole(playerID, PlayerRole.Unassigned);
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

	public void AdjustRoleNumbersForMovedPlayer(PlayerRole oldRole, PlayerRole newRole)
	{
		switch (oldRole)
		{
			case PlayerRole.Villager:
				currentNumHiders -= 1;
				break;
			case PlayerRole.Dragon:
				currentNumSeekers -= 1;
				break;
			case PlayerRole.Unassigned:
				currentNumUnassigned -= 1;
				break;
		}
		switch (newRole)
		{
			case PlayerRole.Villager:
				currentNumHiders += 1;
				break;
			case PlayerRole.Dragon:
				currentNumSeekers += 1;
				break;
			case PlayerRole.Unassigned:
				currentNumUnassigned += 1;
				break;
		}
	}

	[RPC]
	public void InitializePlayers()
	{
		HUDManager.SetActive(true);
		GetPlayer(localPlayerID).GetComponent<BasePlayerScript>().InitializeGame();
		GameObject.Find("DebugHud").GetComponent<Canvas>().enabled = false;
		GameObject.Find("GameSetupUI").GetComponent<Canvas>().enabled = false;
		GameObject.Find("GameSetupUI").SetActive(false);
		GameObject.Find("TimerText").GetComponent<Timer>().ResetTimer();
		GameObject.Find("TimerText").GetComponent<Timer>().StartTimer();
		RespawnManager.instance.numVillagerAlive = currentNumSeekers;

		print("Dragon Player: " + dragonPlayer);
		print("Am I a dragon? " + (GetPlayerRole(localPlayerID) == PlayerRole.Dragon));

		if (dragonPlayer == null || GetPlayerRole(localPlayerID) == PlayerRole.Dragon)
		{
			GameObject.Find("DragonHealthContainer").SetActive(false);
		}

		RespawnManager.instance.FindVillagerText();
	}
}
