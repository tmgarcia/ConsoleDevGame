using UnityEngine;
using System.Collections;

public class DragonSegment : MonoBehaviour {

    private PhotonView photonView;
    public Transform parent;
    public Transform parentRotationOVR;
    private DragonMovement movement;

	// Use this for initialization
	void Start () {
        photonView = gameObject.GetComponent<PhotonView>();
        parentRotationOVR = GameObject.Find("DragonOVR_Rotation").transform;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (movement == null) movement = GameObject.Find("DragonHead").GetComponent<DragonMovement>();
        if (movement.isLocal)
        {
            transform.position = Vector3.Lerp(transform.position, parent.position + parent.forward * -2, Time.deltaTime * movement.glideSpeed);

            if (OVRManagerHelper.instance.IsLocalPlayerUsingOVR)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(parentRotationOVR.forward), Time.deltaTime * 5);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, parent.rotation, Time.deltaTime * 5);
            }

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, GetComponent<NetworkAgent>().GetNetworkPosition(), Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, GetComponent<NetworkAgent>().GetNetworkRotation(), Time.deltaTime * 5);
        }
    }
}
