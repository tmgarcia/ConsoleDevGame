using UnityEngine;
using System.Collections;

public class Mimic : MonoBehaviour {

    public Mesh[] disguises;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E)) BecomeAnotherPerson();
	}

    void BecomeAnotherPerson()
    {
        if(disguises.Length>0)
        gameObject.GetComponent<MeshFilter>().mesh = (Mesh)disguises[Random.Range(0, disguises.Length)];
    }
}
