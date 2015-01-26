using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
	public float spawnRadius = 30.0f;
	public int maxSpawned = 10;
	public int currentSpawned = 0;
	public float spawnInterval = 1.0f;
	private float currentSpawnInterval = 0.0f;
	private GameObject dingo;

	void Start()
	{
		dingo = GameObject.Find("jcDingo");
		for (int i = 0; i < maxSpawned; i++)
		{
			Instantiate(dingo, new Vector3(gameObject.transform.position.x + (Random.insideUnitCircle.x * Random.Range(0, spawnRadius)), gameObject.transform.position.y, gameObject.transform.position.z + (Random.insideUnitCircle.y * Random.Range(0, spawnRadius))), Quaternion.identity);
			currentSpawned++;
		}
		dingo.SetActive(false);
	}

	void Update()
	{
		if (currentSpawned < maxSpawned)
		{
			currentSpawnInterval += Time.deltaTime;

			if (currentSpawnInterval >= spawnInterval)
			{
				currentSpawnInterval = 0.0f;
				dingo.SetActive(true);

				Instantiate(dingo, new Vector3(gameObject.transform.position.x + (Random.insideUnitCircle.x * Random.Range(0, 30)), gameObject.transform.position.y, gameObject.transform.position.z + (Random.insideUnitCircle.y * Random.Range(0, 30))), Quaternion.identity);

				currentSpawned++;

				dingo.SetActive(false);
			}
		}
	}
}
