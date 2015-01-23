using UnityEngine;
using System.Collections;

public class CajunTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        //renderer.material.color -= new Color(0.001f, 0.001f, 0.001f);

        transform.position += new Vector3(0.01f, 0, 0);
	}
}
