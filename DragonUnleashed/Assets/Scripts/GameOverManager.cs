using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameOverManager : MonoBehaviour
{
	public static GameOverManager instance;
	private GameObject VillagerWin;
	private GameObject DragonWin;
	private GameObject LeavingPlayArea;

	void Start()
	{
		if (instance == null)
		{
			instance = this;
			VillagerWin = GameObject.Find("VillagerWin");
			DragonWin = GameObject.Find("DragonWin");
			HideGameOver();
			GameObject.Find("TimerText").GetComponent<GameTimer>().TimerFinishEvent += ShowVillagerWin;
			LeavingPlayArea = GameObject.Find("LeavingPlayArea");
			HideLeavingMessage();
		}
	}

	public void ShowDragonWin()
	{
		DragonWin.SetActive(true);
	}

	public void ShowVillagerWin()
	{
		VillagerWin.SetActive(true);
	}

	public void HideGameOver()
	{
		VillagerWin.SetActive(false);
		DragonWin.SetActive(false);
	}

	public void ShowLeavingMessage(int displayTime)
	{
		LeavingPlayArea.SetActive(true);
		var parts = LeavingPlayArea.GetComponentInChildren<Text>().text.Split(':');
		LeavingPlayArea.GetComponentInChildren<Text>().text = parts[0] + ": " + displayTime;


	}

	public void HideLeavingMessage()
	{
		LeavingPlayArea.SetActive(false);
	}
}
