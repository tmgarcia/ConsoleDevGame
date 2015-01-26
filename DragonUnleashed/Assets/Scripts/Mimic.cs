using UnityEngine;
using System.Collections;

public class Mimic : MonoBehaviour {

    public GameObject[] disguises;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E)) MimicProp();
	}

    void MimicProp()
    {
        GameObject cam = transform.FindChild("CamTetherPoint").FindChild("VillagerCamera").gameObject;
        Debug.DrawLine(cam.transform.position, cam.transform.position+cam.transform.forward*20, Color.yellow, 1.0f);
        RaycastHit hit = new RaycastHit();
        int layermask = 1<<8;
        layermask = ~layermask;
        if (Physics.Linecast(cam.transform.position, cam.transform.position + cam.transform.forward * 20, out hit,layermask))
        {
            if (hit.collider.tag == "Prop")
            {
                //GameObject temp = (GameObject)Instantiate(hit.collider.gameObject, new Vector3(0, -50, 0), new Quaternion());
                gameObject.GetComponent<PhotonView>().RPC("CopyObject", PhotonTargets.All, hit.collider.gameObject.name);
                //Destroy(temp);
            }
        }
    }

    void BecomeAnotherPerson()
    {
        if (disguises.Length > 0)
        {
            int i = Random.Range(0, disguises.Length);
            GameObject temp = (GameObject)Instantiate(disguises[i],new Vector3(0,-50,0), new Quaternion());
            gameObject.GetComponent<PhotonView>().RPC("CopyObject",PhotonTargets.All,temp.transform.GetChild(0).gameObject);
            Destroy(temp);
        }
    }

    [RPC]
    public void CopyObject(string otherName)
    {
        //print(otherName);
        GameObject other = (GameObject)Instantiate(GameObject.Find(otherName), new Vector3(0, -50, 0), new Quaternion());
        gameObject.GetComponent<MeshFilter>().mesh = other.GetComponent<MeshFilter>().mesh;

        gameObject.renderer.material = other.renderer.material;
        
        Destroy(gameObject.collider);
        if(other.collider.GetType()==typeof(BoxCollider))
        {
            gameObject.AddComponent<BoxCollider>();
            gameObject.GetComponent<BoxCollider>().center = other.GetComponent<BoxCollider>().center;
            gameObject.GetComponent<BoxCollider>().size = other.GetComponent<BoxCollider>().size;
        }
        else if(other.collider.GetType()==typeof(CapsuleCollider))
        {
            gameObject.AddComponent<CapsuleCollider>();
            gameObject.GetComponent<CapsuleCollider>().center = other.GetComponent<CapsuleCollider>().center;
            gameObject.GetComponent<CapsuleCollider>().radius = other.GetComponent<CapsuleCollider>().radius;
            gameObject.GetComponent<CapsuleCollider>().height = other.GetComponent<CapsuleCollider>().height;
            gameObject.GetComponent<CapsuleCollider>().direction = other.GetComponent<CapsuleCollider>().direction;
        }
        else if (other.collider.GetType() == typeof(SphereCollider))
        {
            gameObject.AddComponent<SphereCollider>();
            gameObject.GetComponent<SphereCollider>().center = other.GetComponent<SphereCollider>().center;
            gameObject.GetComponent<SphereCollider>().radius = other.GetComponent<SphereCollider>().radius;
        }
        Destroy(other);
    }
}
