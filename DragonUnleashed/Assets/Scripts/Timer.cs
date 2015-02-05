using UnityEngine;
using System.Collections.Generic;

public class Timer : MonoBehaviour
{
	public delegate void TimerDelegate();
	public event TimerDelegate TimerFinishEvent;

	public int StartingMinutes = 5;
	public int StartingSeconds = 0;
	protected float currentSeconds;
	public bool isRunning = false;
	public int currentDisplayMinutes;
	public int currentDisplaySeconds;

	void Start()
	{
		StopTimer();
		ResetTimer();
	}

	void Update()
	{
		if (isRunning)
		{
			currentSeconds -= Time.deltaTime;
			UpdateDisplay();
			if (currentSeconds <= float.Epsilon)
			{
				currentSeconds = float.Epsilon;
				StopTimer();
				OnTimerFinishEvent();
			}
		}
	}

	public void StartTimer()
	{
		isRunning = true;
	}

	public void StopTimer()
	{
		isRunning = false;
	}

	public void ResetTimer()
	{
		currentSeconds = StartingMinutes * 60 + StartingSeconds;
	}

	public void OnTimerFinishEvent()
	{
		if (TimerFinishEvent != null)
		{
			TimerFinishEvent();
		}
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
			currentDisplayMinutes = newDisplayMinutes;
			currentDisplaySeconds = newDisplaySecounds;
		}
	}

}
