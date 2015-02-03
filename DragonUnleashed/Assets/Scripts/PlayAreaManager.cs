using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayAreaManager : MonoBehaviour
{
	bool isOutsidePlayArea = false;
	Timer graceTimer;
	Text graceText;
	List<Damageable> removalQueue;

	void OnEnable()
	{
		graceTimer = GetComponent<Timer>();
		graceText = GameObject.Find("LeavingPlayArea").GetComponentInChildren<Text>();
		graceTimer.TimerFinishEvent += HandlePlayerRemoval;
		removalQueue = new List<Damageable>();
		
	}

	void OnDisable()
	{
		graceTimer.TimerFinishEvent -= HandlePlayerRemoval;
	}

	void Update ()
	{
		renderer.material.mainTextureOffset += new Vector2(Time.deltaTime / 5, Time.deltaTime / 5);

		if (isOutsidePlayArea)
		{
			GameOverManager.instance.ShowLeavingMessage(graceTimer.currentDisplaySeconds);
		}
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<Damageable>() != null && c.GetComponent<Damageable>().damageRole != DamageRole.Scenery)
		{
			if (c.GetComponent<Damageable>().damageRole == DamageRole.Dragon && c.GetComponentInParent<NetworkAgent>().playerID == PlayersManager.instance.localPlayerID || c.GetComponent<Damageable>().damageRole == DamageRole.Villager && c.GetComponent<NetworkAgent>().playerID == PlayersManager.instance.localPlayerID)
			{

				isOutsidePlayArea = false; ;
				GameOverManager.instance.HideLeavingMessage();
			}
			removalQueue.Remove(c.GetComponent<Damageable>());
		}

	}

	void OnTriggerExit(Collider c)
	{
		if (c.GetComponent<Damageable>() != null && c.GetComponent<Damageable>().damageRole != DamageRole.Scenery)
		{
			if (c.GetComponent<Damageable>().damageRole == DamageRole.Dragon && c.GetComponentInParent<NetworkAgent>().playerID == PlayersManager.instance.localPlayerID || c.GetComponent<Damageable>().damageRole == DamageRole.Villager && c.GetComponent<NetworkAgent>().playerID == PlayersManager.instance.localPlayerID)
			{
							isOutsidePlayArea = true;
			}

			graceTimer.ResetTimer();
			graceTimer.StartTimer();
			removalQueue.Add(c.GetComponent<Damageable>());
		}
	}

	public void HandlePlayerRemoval()
	{
		foreach(Damageable player in removalQueue)
		{
			player.CurrentLocalIntegrity = 0.0f;
		}

		removalQueue.Clear();
	}
}
