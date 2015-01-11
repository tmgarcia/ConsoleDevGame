using UnityEngine;
using System.Collections;
using System.Linq;

public class Player : MonoBehaviour
{
	public PlayerRole Role = PlayerRole.Villager;
	private Flammable flammable;

	[HideInInspector]
	public bool IsAlive;


	// Use this for initialization
	void Start()
	{
		flammable = GetComponent<Flammable>();

		if (flammable == null)
		{
			Debug.LogException(new UnityException("Player GameObject must contain Flammable script."));
			Destroy(this);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (flammable.Integrity <= float.Epsilon && IsAlive)
		{
			OnDeath();
		}
	}

	public void OnDeath()
	{
		if (Role == PlayerRole.Villager)
		{
			RespawnManager.instance.Respawn(gameObject);
			flammable.Integrity = flammable.StartingIntegrity;
		}
		else if (Role == PlayerRole.Dragon)
		{
			print("You killed the dragon yay.");
			IsAlive = false;
		}
	}
}
