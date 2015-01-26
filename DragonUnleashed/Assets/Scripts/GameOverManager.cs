using UnityEngine;
using System.Collections.Generic;

public class GameOverManager : MonoBehaviour
{
	public static GameOverManager instance;
	private GameObject VillagerWin;
	private GameObject DragonWin;

	void Start()
	{
		if (instance == null)
		{
			instance = this;
			VillagerWin = GameObject.Find("VillagerWin");
			DragonWin = GameObject.Find("DragonWin");
			HideGameOver();
			GameObject.Find("TimerText").GetComponent<GameTimer>().TimerFinishEvent += ShowVillagerWin;
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
}
