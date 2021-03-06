﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameTimer : Timer
{
	public string VillagerTimerText = "Survive the Dragon: ";
	public string DragonTimerText = "Kill Them All: ";
	private string localTimerText = "";
	private Text timerText;

	public bool StartOnStart = true;

	private int localPlayerID = -1;
	private PlayerRole localPlayerRole;

	void Start()
	{
		timerText = GetComponent<Text>();
		StopTimer();
		ResetTimer();

		if (StartOnStart)
		{
			StartTimer();
		}
	}

	void Update()
	{
		if (isRunning)
		{
			currentSeconds -= Time.deltaTime;
			if (currentSeconds <= float.Epsilon)
			{
				currentSeconds = float.Epsilon;
				StopTimer();
				OnTimerFinishEvent();
			}
			UpdateDisplay();
		}
	}

	public void StartTimer()
	{
		base.StartTimer();
		timerText.enabled = true;
	}

	public void StopTimer()
	{
		base.StopTimer();
		timerText.enabled = false;
	}

	private void UpdateDisplay()
	{
		int newDisplayMinutes = (int)(currentSeconds / 60);
		float newSeconds = currentSeconds - (newDisplayMinutes * 60);
		int newDisplaySecounds = (int)Mathf.Round(newSeconds);

		if (newDisplaySecounds == 60)
		{
			newDisplayMinutes++;
			newDisplaySecounds = 0;
		}

		if (newDisplayMinutes != currentDisplayMinutes || newDisplaySecounds != currentDisplaySeconds)
		{
			SetLocalText();
			timerText.text = localTimerText + newDisplayMinutes + ":";
			timerText.text += (newDisplaySecounds > 9) ? newDisplaySecounds.ToString() : ("0" + newDisplaySecounds.ToString());
			currentDisplayMinutes = newDisplayMinutes;
			currentDisplaySeconds = newDisplaySecounds;
		}
	}

	private void SetLocalText()
	{
		localPlayerID = PlayersManager.instance.localPlayerID;
		localPlayerRole = PlayersManager.instance.GetPlayerRole(localPlayerID);

		switch (localPlayerRole)
		{
			case PlayerRole.Villager:
				localTimerText = VillagerTimerText;
				break;
			case PlayerRole.Dragon:
				localTimerText = DragonTimerText;
				break;
			default:
				localTimerText = "Unknown Player Role: ";
				Debug.LogException(new UnityException("Timer's local player role set to unhandled type."));
				break;
		}
	}
}
