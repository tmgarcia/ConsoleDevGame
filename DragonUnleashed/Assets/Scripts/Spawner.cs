using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
	public bool SpawnAtStart = false;
	public bool RespawnAfterStart = true;
	public float spawnRadius = 30.0f;
	public int maxSpawned = 10;
	public int currentSpawned = 0;
	public float spawnInterval = 1.0f;
	private float currentSpawnInterval = 0.0f;
	public GameObject NPCSpawnInstance;
	public float BurnChance = 0.05f;

	void Start()
	{
		if (NPCSpawnInstance != null || NPCSpawnInstance.GetComponent<MeshCollider>() != null)
		{
			if (SpawnAtStart)
			{
				for (int i = 0; i < maxSpawned; i++)
				{
					var spawnedAnimal = (GameObject)Instantiate(NPCSpawnInstance, new Vector3(gameObject.transform.position.x + (Random.insideUnitCircle.x * Random.Range(0, spawnRadius)), gameObject.transform.position.y, gameObject.transform.position.z + (Random.insideUnitCircle.y * Random.Range(0, spawnRadius))), Quaternion.identity);
					spawnedAnimal.SetActive(true);
					spawnedAnimal.GetComponentInChildren<MonsterMovement>().anchor = gameObject;
					spawnedAnimal.GetComponentInChildren<MonsterMovement>().anchorPosition = transform.position;
					if (Random.value <= BurnChance)
					{
						spawnedAnimal.GetComponentInChildren<Flammable>().BurninationLevel = 300.0f;
					}
					currentSpawned++;
				}
			}
		}
		else if(NPCSpawnInstance == null)
		{
			Debug.LogException(new UnityException("NPCSpawnInstance is null."));
		}
		else if(NPCSpawnInstance.GetComponent<MeshCollider>() == null)
		{
			Debug.LogException(new UnityException("NPCSpawnInstance.GetComponent<MeshCollider> is null."));
		}
		else
		{
			Debug.LogException(new UnityException("An unknown error has occurred."));
		}
	}

	void Update()
	{
		if (currentSpawned < maxSpawned && RespawnAfterStart)
		{
			currentSpawnInterval += Time.deltaTime;

			if (currentSpawnInterval >= spawnInterval)
			{
				currentSpawnInterval = 0.0f;
				NPCSpawnInstance.SetActive(true);

				var spawnedAnimal = (GameObject)Instantiate(NPCSpawnInstance, new Vector3(gameObject.transform.position.x + (Random.insideUnitCircle.x * Random.Range(0, 30)), gameObject.transform.position.y, gameObject.transform.position.z + (Random.insideUnitCircle.y * Random.Range(0, 30))), Quaternion.identity);
				if (Random.value <= BurnChance)
				{
					spawnedAnimal.GetComponentInChildren<Flammable>().BurninationLevel = 300.0f;
				}
				spawnedAnimal.GetComponentInChildren<MonsterMovement>().anchor = gameObject;
				spawnedAnimal.GetComponentInChildren<MonsterMovement>().anchorPosition = transform.position;
				currentSpawned++;

				NPCSpawnInstance.SetActive(false);
			}
		}
	}

	float lastSpawnerRadius;

#if UNITY_EDITOR
	void OnValidate()
	{
		if (GetComponent<Spawner>().spawnRadius != lastSpawnerRadius)
		{
			GetComponent<SphereCollider>().radius = GetComponent<Spawner>().spawnRadius;

		}

		lastSpawnerRadius = GetComponent<Spawner>().spawnRadius;
	}
#endif
}
