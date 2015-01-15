using UnityEngine;
using System.Collections;

public class Mimic : MonoBehaviour {

    public GameObject[] disguises;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E)) BecomeAnotherPerson();
	}

    void BecomeAnotherPerson()
    {
        if (disguises.Length > 0)
        {
            int i = Random.Range(0, disguises.Length);
            GameObject temp = (GameObject)Instantiate(disguises[i],new Vector3(0,-50,0), new Quaternion());
            CopyObject(temp.transform.GetChild(0).gameObject);
            Destroy(temp);
        }
    }

    private void CopyObject(GameObject other)
    {
        gameObject.GetComponent<MeshFilter>().mesh = other.GetComponent<MeshFilter>().mesh;

        gameObject.renderer.material = other.renderer.material;
        
        Destroy(gameObject.collider);
        if(other.collider.GetType()==typeof(BoxCollider))
        {
            gameObject.AddComponent<BoxCollider>();
        }
        else if(other.collider.GetType()==typeof(CapsuleCollider))
        {
            gameObject.AddComponent<CapsuleCollider>();
        }
        else if (other.collider.GetType() == typeof(SphereCollider))
        {
            gameObject.AddComponent<SphereCollider>();
        }
    }
}
