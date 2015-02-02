using UnityEngine;
using System.Collections;

public class StickToCamera : MonoBehaviour 
{
    private GameObject OVRCamera;
    private GameObject TransformOVR;

	// Use this for initialization
	void Start () 
    {
        TransformOVR = GameObject.Find("DragonOVR_Rotation");
        GameObject OVR = GameObject.Find("OVRCameraRig");
        OVRCamera = OVR.transform.GetChild(1).gameObject;
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.position = OVRCamera.transform.position + ((OVRCamera.transform.forward * 0.55f) + (OVRCamera.transform.right * 0.15f) + (-OVRCamera.transform.up * 0.1f));
        transform.rotation = TransformOVR.transform.rotation;
	}
}
