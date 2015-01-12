using UnityEngine;
using System.Collections;
using System.Linq;

public class Player : MonoBehaviour
{
	public PlayerRole Role = PlayerRole.Villager;
	private Damageable damageable;
	public bool IsAlive { get; set; }
	public int StartingLivePool = 15;
	public static int? LivePool = null;

	void Start()
	{
		IsAlive = true;
		damageable = GetComponent<Damageable>();

		if (damageable == null)
		{
			Debug.LogException(new UnityException("Player GameObject must contain Damageable script."));
			Destroy(this);
		}

		if (LivePool == null)
		{
			LivePool = StartingLivePool;
		}

	}

	void Update()
	{
		if (damageable.CurrentIntegrity <= float.Epsilon && IsAlive)
		{
			OnDeath();
		}
	}

	public void OnDeath()
	{
		if (Role == PlayerRole.Villager)
		{
			RespawnManager.instance.Respawn(gameObject);
			damageable.CurrentIntegrity = damageable.StartingIntegrity;
			if (--LivePool == 0)
			{
				GameOverManager.instance.ShowDragonWin();
			}
			print("Current Live Pool: " + LivePool);
		}
		else if (Role == PlayerRole.Dragon)
		{
			print("You killed the dragon yay.");
			IsAlive = false;
			GameOverManager.instance.ShowVillagerWin();
		}
	}
}
