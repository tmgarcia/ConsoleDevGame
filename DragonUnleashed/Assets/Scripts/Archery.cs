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
                FireArrow();
			}
		}
	}

    public static void FireArrow()
    {
        photonView.RPC("RPCFireArrow", PhotonTargets.All);
    }

	[RPC]
	private void RPCFireArrow()
	{
		GameObject launchedArrow = Instantiate(arrow, gameObject.transform.position + new Vector3(0.75f, 0.75f, 0), transform.rotation) as GameObject;
		launchedArrow.rigidbody.AddForce(Camera.main.transform.forward * 1000);

	}

	public bool GetAiming()
	{
		return aiming;
	}
}
