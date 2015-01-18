using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RespawnManager : MonoBehaviour
{
	public static RespawnManager instance;
    public int maxVillagerLives = 10;
    public int remainingVillagerLives;
    public GameObject villagerLivesNumberDisplay;
	public List<GameObject> VillagerRespawnPoints;
    public List<GameObject> DragonRespawnPoints;
    private static PhotonView ScenePhotonView;

	void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
        remainingVillagerLives = maxVillagerLives;
        ScenePhotonView = this.GetComponent<PhotonView>();
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
                if (remainingVillagerLives > 0)
                {
                    character.transform.position = VillagerRespawnPoints[Random.Range(0, VillagerRespawnPoints.Count)].transform.position;
                }
            }
            else
            {
                character.transform.position = DragonRespawnPoints[Random.Range(0, DragonRespawnPoints.Count)].transform.position;
            }
		}
	}
}
