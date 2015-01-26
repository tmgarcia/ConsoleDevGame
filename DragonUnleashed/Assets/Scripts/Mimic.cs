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
        
        DestroyImmediate(gameObject.collider);

        print("Other prop collider:");
        print("\tType: " + other.collider.GetType().ToString());

        if(other.collider.GetType()==typeof(BoxCollider))
        {
            print("\tCenter: " + other.GetComponent<BoxCollider>().center.ToString());
            print("\tSize: " + other.GetComponent<BoxCollider>().size.ToString());
            gameObject.AddComponent<BoxCollider>();
            gameObject.GetComponent<BoxCollider>().center = other.GetComponent<BoxCollider>().center;
            gameObject.GetComponent<BoxCollider>().size = other.GetComponent<BoxCollider>().size;

            print("This prop collider:");
            print("\tType: " + gameObject.collider.GetType().ToString());
            print("\tCenter: " + gameObject.GetComponent<BoxCollider>().center.ToString());
            print("\tSize: " + gameObject.GetComponent<BoxCollider>().size.ToString());
        }
        else if(other.collider.GetType()==typeof(CapsuleCollider))
        {
            print("\tCenter: " + other.GetComponent<CapsuleCollider>().center.ToString());
            print("\tRadius: " + other.GetComponent<CapsuleCollider>().radius.ToString());
            print("\tHeight: " + other.GetComponent<CapsuleCollider>().height.ToString());
            print("\tDirection: " + other.GetComponent<CapsuleCollider>().direction.ToString());
            gameObject.AddComponent<CapsuleCollider>();
            gameObject.GetComponent<CapsuleCollider>().center = other.GetComponent<CapsuleCollider>().center;
            gameObject.GetComponent<CapsuleCollider>().radius = other.GetComponent<CapsuleCollider>().radius;
            gameObject.GetComponent<CapsuleCollider>().height = other.GetComponent<CapsuleCollider>().height;
            gameObject.GetComponent<CapsuleCollider>().direction = other.GetComponent<CapsuleCollider>().direction;
            print("This prop collider:");
            print("\tType: " + gameObject.collider.GetType().ToString());
            print("\tCenter: " + gameObject.GetComponent<CapsuleCollider>().center.ToString());
            print("\tRadius: " + gameObject.GetComponent<CapsuleCollider>().radius.ToString());
            print("\tHeight: " + gameObject.GetComponent<CapsuleCollider>().height.ToString());
            print("\tDirection: " + gameObject.GetComponent<CapsuleCollider>().direction.ToString());
        }
        else if (other.collider.GetType() == typeof(SphereCollider))
        {
            print("\tCenter: " + other.GetComponent<SphereCollider>().center.ToString());
            print("\tRadius: " + other.GetComponent<SphereCollider>().radius.ToString());
            gameObject.AddComponent<SphereCollider>();
            gameObject.GetComponent<SphereCollider>().center = other.GetComponent<SphereCollider>().center;
            gameObject.GetComponent<SphereCollider>().radius = other.GetComponent<SphereCollider>().radius;
            print("This prop collider:");
            print("\tType: " + gameObject.collider.GetType().ToString());
            print("\tCenter: " + gameObject.GetComponent<SphereCollider>().center.ToString());
            print("\tRadius: " + gameObject.GetComponent<SphereCollider>().radius.ToString());
        }
        
        Destroy(other);
    }
}
