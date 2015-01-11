using UnityEngine;
using System.Collections.Generic;

public class RespawnManager : MonoBehaviour
{
	public static RespawnManager instance;
	public List<GameObject> RespawnPoints;

	void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	void Update()
	{
		Respawn(gameObject);
	}

	public void Respawn(GameObject character)
	{
		if (RespawnPoints.Count < 1)
		{
			Debug.LogException(new UnityException("Attempting to Respawn without any RespawnPoints!"));
		}
		else
		{
			character.transform.position = RespawnPoints[Random.Range(0, RespawnPoints.Count)].transform.position;
		}
	}
}
