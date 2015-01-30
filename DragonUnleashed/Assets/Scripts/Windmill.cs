using UnityEngine;
using System.Collections;

public class Windmill : MonoBehaviour {
    private Vector3 axis = new Vector3(0, 0, 1);
    public float speed = 2.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(axis, -speed);
	}
}
