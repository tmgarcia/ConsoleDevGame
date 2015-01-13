﻿using UnityEngine;
using System.Collections;

public class Archery : Photon.MonoBehaviour
{
	private bool aiming;
	public GameObject arrow;
	private NetworkAgent na;
    private static PhotonView photonView;

	// Use this for initialization
	void Start()
	{
		aiming = false;
		na = GetComponent<NetworkAgent>();
        photonView = GetComponent<PhotonView>();
    }

	// Update is called once per frame
	void Update()
	{
		if (na.IsLocalCharacter())
		{
			if (Input.GetMouseButtonDown(1)) //Right mouse down
			{
				//Enter aim mode (Disable motion?)
				aiming = true;
				//print("Aiming!");
			}
			if (Input.GetMouseButtonUp(1)) //Right mouse up
			{
				//Exit aim mode
				aiming = false;
				//print("Cease Aiming!");
			}
			if (aiming && Input.GetMouseButtonDown(0)) //Left mouse down
			{
                Vector3 arrowForce = Camera.main.transform.forward * 1000;
                Vector3 launchLocation = gameObject.transform.position + (0.75f * (gameObject.transform.up + gameObject.transform.right)); 
                FireArrow(launchLocation, transform.rotation, arrowForce);
			}
		}
	}

    public static void FireArrow(Vector3 position, Quaternion rotation, Vector3 force)
    {
        photonView.RPC("RPCFireArrow", PhotonTargets.All, new object[] { position, rotation, force });
    }

	[RPC]
	private void RPCFireArrow(Vector3 position, Quaternion rotation, Vector3 force)
	{
        GameObject launchedArrow = Instantiate(arrow, position , rotation) as GameObject;
        launchedArrow.rigidbody.AddForce(force);
	}

	public bool GetAiming()
	{
		return aiming;
	}
}
