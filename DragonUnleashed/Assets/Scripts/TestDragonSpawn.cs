using UnityEngine;
using System.Collections;

public class TestDragonSpawn : MonoBehaviour 
{
    float time = 0.0f;
    float TIME_UP = 10.0f;
    bool spawned;
    bool timeUp;
    public GameObject toSpawn;

	void Start () 
    {

	}
	
	void Update () 
    {
        if (!spawned)
        {
            time += Time.deltaTime;
            if (OVRManagerHelper.instance.IsLocalPlayerUsingOVR && time >= TIME_UP)
            {
                GameObject playerCharacter = (GameObject)Instantiate(toSpawn, Vector3.zero, Quaternion.identity);
                spawned = true;
            }
        }

	}
}
