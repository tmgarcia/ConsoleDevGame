using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour
{
	public float StartingIntegrity = 100.0f;
	public float CurrentIntegrity { get; set; }
	public DamageRole damageRole = DamageRole.Villager;

	void Start()
	{
		CurrentIntegrity = StartingIntegrity;
	}
}
