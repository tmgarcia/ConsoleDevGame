using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Timer : MonoBehaviour
{
	public string VillagerTimerText = "Survive the Dragon: ";
	public string DragonTimerText = "Survive the Dragon: ";
	public int StartingMinutes = 5;
	public int StartingSeconds = 0;
	private float currentSeconds;
	private bool isRunning = false;
	private Text timerText;
	private int currentDisplayMinutes;
	private int currentDisplaySeconds;

	public delegate void TimerDelegate();
	public event TimerDelegate TimerFinishEvent;

	public bool StartOnStart = true;

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
		isRunning = true;
		timerText.enabled = true;
	}

	public void StopTimer()
	{
		isRunning = false;
		timerText.enabled = false;
	}

	public void ResetTimer()
	{
		currentSeconds = StartingMinutes * 60 + StartingSeconds;
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
			timerText.text = VillagerTimerText + newDisplayMinutes + ":";
			timerText.text += (newDisplaySecounds > 9) ? newDisplaySecounds.ToString() : ("0" + newDisplaySecounds.ToString());
			currentDisplayMinutes = newDisplayMinutes;
			currentDisplaySeconds = newDisplaySecounds;
		}
	}

	public void OnTimerFinishEvent()
	{
		if (TimerFinishEvent != null)
		{
			TimerFinishEvent();
			print("Ding");
		}
	}
}
