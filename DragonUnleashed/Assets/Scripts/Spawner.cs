using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
	public bool RespawnAfterStart = true;
	public float spawnRadius = 30.0f;
	public int maxSpawned = 10;
	public int currentSpawned = 0;
	public float spawnInterval = 1.0f;
	private float currentSpawnInterval = 0.0f;
	public GameObject NPCSpawnInstance;

	void Start()
	{
		if (NPCSpawnInstance == null || NPCSpawnInstance.GetComponent<MeshCollider>() == null)
		{
			for (int i = 0; i < maxSpawned; i++)
			{
				Instantiate(NPCSpawnInstance, new Vector3(gameObject.transform.position.x + (Random.insideUnitCircle.x * Random.Range(0, spawnRadius)), gameObject.transform.position.y, gameObject.transform.position.z + (Random.insideUnitCircle.y * Random.Range(0, spawnRadius))), Quaternion.identity);
				currentSpawned++;
			}
			NPCSpawnInstance.SetActive(false);
		}
		else
		{
			Debug.LogException(new UnityException("NPCSpawnInstance is null or does not contain Actor script. Fuck you."));
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

				Instantiate(NPCSpawnInstance, new Vector3(gameObject.transform.position.x + (Random.insideUnitCircle.x * Random.Range(0, 30)), gameObject.transform.position.y, gameObject.transform.position.z + (Random.insideUnitCircle.y * Random.Range(0, 30))), Quaternion.identity);

				currentSpawned++;

				NPCSpawnInstance.SetActive(false);
			}
		}
	}

	float lastSpawnerRadius;
	//float lastColliderRadius;

#if UNITY_EDITOR
	void OnValidate()
	{

		print("RING RING");

		if (GetComponent<Spawner>().spawnRadius != lastSpawnerRadius)
		{
			GetComponent<SphereCollider>().radius = GetComponent<Spawner>().spawnRadius;

		}
		//else if (GetComponent<SphereCollider>().radius != lastColliderRadius)
		//{
		//	GetComponent<Spawner>().spawnRadius = GetComponent<SphereCollider>().radius;
		//}

		lastSpawnerRadius = GetComponent<Spawner>().spawnRadius;
		//lastColliderRadius = lastSpawnerRadius;
	}
#endif
}
