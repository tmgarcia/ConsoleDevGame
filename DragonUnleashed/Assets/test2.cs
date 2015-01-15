using UnityEngine;
using System.Collections.Generic;

public class test2 : MonoBehaviour
{
	GameObject testObject;
	void OnEnable ()
	{
		//testObject = (GameObject)Instantiate("OVRCameraRig", new Vector3(Random.Range(200, 220), 1, Random.Range(160, 180)), Quaternion.identity, 0);

		//test.camRef.SetActive(true);
	}

	void OnDisable()
	{
		Destroy(testObject);
	}
}
