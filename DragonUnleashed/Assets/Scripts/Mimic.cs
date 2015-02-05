using UnityEngine;
using System.Collections;

public class Mimic : MonoBehaviour {

    private string currentDisguise = "Villager";

    public void SetCurrentDisguise(string newname)
    {
        currentDisguise = newname;
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.E)&&!Input.GetKey(KeyCode.Mouse1)) MimicProp();
        if (currentDisguise != "Villager" && Input.GetKeyDown(KeyCode.Mouse1))
        {
            gameObject.GetComponent<PhotonView>().RPC("SwitchModels", PhotonTargets.All);
            gameObject.GetComponent<PhotonView>().RPC("CopyObject", PhotonTargets.All, "Villager");
        }
        if (currentDisguise != "Villager" && Input.GetKeyUp(KeyCode.Mouse1))
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
                currentDisguise = hit.collider.gameObject.name;
                gameObject.GetComponent<PhotonView>().RPC("CopyObject", PhotonTargets.All, hit.collider.gameObject.name);
            }
        }
    }

    [RPC]
    public void CopyObject(string otherName)
    {
        gameObject.GetComponent<MeshFilter>().mesh = PropMaster.instance.GetMeshByName(otherName);

        gameObject.renderer.material = PropMaster.instance.GetMaterialByName(otherName);

        gameObject.GetComponent<MeshCollider>().sharedMesh = PropMaster.instance.GetColliderMeshByName(otherName);
    }
}
