using UnityEngine;
using System.Collections.Generic;

public class GameOverManager : MonoBehaviour
{
	public static GameOverManager instance;

	void Start ()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	public void ShowDragonWin()
	{

	}

	public void ShowVillagerWin()
	{

	}

	public void HideGameOver()
	{

	}
}
