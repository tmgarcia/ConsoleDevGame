using UnityEngine;
using System.Collections;

public class DragonSegment : MonoBehaviour {

    private PhotonView photonView;
    public Transform parent;
    private DragonMovement movement;

	// Use this for initialization
	void Start () {
        photonView = gameObject.GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
    void Update()
    {
        if (movement == null) movement = GameObject.Find("DragonHead").GetComponent<DragonMovement>();
        if (movement.isLocal)
        {
            if (Vector3.Distance(transform.position, parent.position) > 3.5f)
            {
                transform.position = Vector3.Lerp(transform.position, parent.position, Time.deltaTime * movement.speed);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, GetComponent<NetworkAgent>().GetNetworkPosition(), Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, GetComponent<NetworkAgent>().GetNetworkRotation(), Time.deltaTime * 5);
        }
    }
}
