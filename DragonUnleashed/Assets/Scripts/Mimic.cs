using UnityEngine;
using System.Collections;

public class Mimic : MonoBehaviour {

    public GameObject[] disguises;

    private string currentDisguise = "Villager";

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.E)&&!Input.GetKey(KeyCode.Mouse1)) MimicProp();
        if (currentDisguise == "Villager" && Input.GetKeyDown(KeyCode.Mouse1))
        {
            gameObject.GetComponent<PhotonView>().RPC("SwitchModels", PhotonTargets.All);
            gameObject.GetComponent<PhotonView>().RPC("CopyObject", PhotonTargets.All, "Villager");
        }
        if (currentDisguise == "Villager" && Input.GetKeyUp(KeyCode.Mouse1))
        {
            gameObject.GetComponent<PhotonView>().RPC("SwitchModels", PhotonTargets.All);
            gameObject.GetComponent<PhotonView>().RPC("CopyObject", PhotonTargets.All,currentDisguise);
        }
	}

    [RPC]
    public void SwitchModels()
    {
        transform.FindChild("Staff").gameObject.SetActive(!transform.FindChild("Staff").gameObject.GetActive());
        transform.FindChild("QuiverRig").gameObject.SetActive(!transform.FindChild("QuiverRig").gameObject.GetActive());
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
                if(currentDisguise == "Villager")gameObject.GetComponent<PhotonView>().RPC("SwitchModels", PhotonTargets.All);
                gameObject.GetComponent<PhotonView>().RPC("CopyObject", PhotonTargets.All, hit.collider.gameObject.name);
            }
        }
    }

    [RPC]
    public void CopyObject(string otherName)
    {
        GameObject other = (GameObject)Instantiate(GameObject.Find(otherName), new Vector3(0, -50, 0), new Quaternion());

        gameObject.GetComponent<MeshFilter>().mesh = other.GetComponent<MeshFilter>().mesh;

        gameObject.renderer.material = other.renderer.material;
        
        DestroyImmediate(gameObject.collider);

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
