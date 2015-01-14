using UnityEngine;
using System.Collections.Generic;

public class RespawnManager : MonoBehaviour
{
	public static RespawnManager instance;
	public List<GameObject> VillagerRespawnPoints;
    public List<GameObject> DragonRespawnPoints;

	void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

    public Vector3 GetRandomSpawn(PlayerRole role)
    {
        if (VillagerRespawnPoints.Count < 1)
        {
            Debug.LogException(new UnityException("Attempting to Respawn without any RespawnPoints!"));
            return new Vector3();
        }
        else
        {
            if (role == PlayerRole.Villager)
            {
                return VillagerRespawnPoints[Random.Range(0, VillagerRespawnPoints.Count)].transform.position;
            }
            else
            {
                return DragonRespawnPoints[Random.Range(0, DragonRespawnPoints.Count)].transform.position;
            }
        }
    }

	public void Respawn(GameObject character)
	{
        if (VillagerRespawnPoints.Count < 1)
		{
			Debug.LogException(new UnityException("Attempting to Respawn without any RespawnPoints!"));
		}
		else
		{
            if (character.GetComponent<VillagerMovement>() != null)
            {
                character.transform.position = VillagerRespawnPoints[Random.Range(0, VillagerRespawnPoints.Count)].transform.position;
            }
            else
            {
                character.transform.position = DragonRespawnPoints[Random.Range(0, DragonRespawnPoints.Count)].transform.position;
            }
		}
	}
}
