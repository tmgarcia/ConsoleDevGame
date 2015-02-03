using UnityEngine;
using System.Collections;
using System.Linq;

public class Player : MonoBehaviour
{
	public PlayerRole Role = PlayerRole.Villager;
	private Damageable damageable;
	public bool IsAlive { get; set; }
	public bool IsLocal = false;
	public bool InitialSetup = false;

	void Start()
	{
		IsAlive = true;
		damageable = GetComponent<Damageable>();

		if (damageable == null)
		{
			Debug.LogException(new UnityException("Player GameObject must contain Damageable script."));
			Destroy(this);
		}

		GetComponent<AudioListener>().enabled = false;
	}

	void Update()
	{
		if (damageable.CurrentIntegrity <= float.Epsilon && IsAlive)
		{
			OnDeath();
		}

		if (!InitialSetup && IsLocal)
		{
			GetComponent<AudioListener>().enabled = true;
			InitialSetup = true;
		}
	}

	public void OnDeath()
	{
		if (Role == PlayerRole.Villager)
		{
			RespawnManager.instance.Respawn(gameObject);
			damageable.CurrentIntegrity = damageable.StartingIntegrity;
		}
		else if (Role == PlayerRole.Dragon)
		{
			//print("You killed the dragon yay.");
			IsAlive = false;
			GameOverManager.instance.ShowVillagerWin();
		}
	}
}
