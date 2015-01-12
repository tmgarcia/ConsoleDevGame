﻿using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

    private bool stuck;

	// Use this for initialization
	void Start () {
        stuck = false;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnCollisionEnter(Collision collision)
    {
        if (!stuck)
        {
            if (collision.gameObject.name != "Arrow(Clone)")
            {
                gameObject.transform.parent = collision.gameObject.transform;
                Destroy(rigidbody);
            }
           
            if (collision.gameObject.name == "Dragon")
            {
                //inflict damage with damageable
            }
            stuck = true;
        }
    }
}
