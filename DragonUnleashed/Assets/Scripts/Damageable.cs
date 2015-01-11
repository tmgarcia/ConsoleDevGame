using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour
{
	public float StartingIntegrity = 100.0f;
	public float CurrentIntegrity { get; set; }

	void Start()
	{
		CurrentIntegrity = StartingIntegrity;
	}
}
