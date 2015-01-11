﻿using UnityEngine;
using System.Collections;

public class DragonSegment : MonoBehaviour {

    private PhotonView photonView;
    public Transform parent;
    private DragonMovement movement;

	// Use this for initialization
	void Start () {
        photonView = gameObject.GetComponent<PhotonView>();
        movement = GameObject.Find("DragonHead").GetComponent<DragonMovement>();
	}
	
	// Update is called once per frame
    void Update()
    {
        
            if (Vector3.Distance(transform.position, parent.position) > 1)
            {
                transform.position = Vector3.Lerp(transform.position, parent.position, Time.deltaTime * movement.speed);
            }
        
    }
}
