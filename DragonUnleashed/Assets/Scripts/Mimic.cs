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
	void Update () 
    {
        if (GetComponent<Player>().IsLocal)
        {
            if (Input.GetKeyDown(KeyCode.E) && !Input.GetKey(KeyCode.Mouse1)) MimicProp();
            if (currentDisguise != "Villager" && Input.GetKeyDown(KeyCode.Mouse1))
            {
                gameObject.GetComponent<PhotonView>().RPC("SwitchModels", PhotonTargets.All);
                gameObject.GetComponent<PhotonView>().RPC("CopyObject", PhotonTargets.All, "Villager");
            }
            if (currentDisguise != "Villager" && Input.GetKeyUp(KeyCode.Mouse1))
            {
                gameObject.GetComponent<PhotonView>().RPC("SwitchModels", PhotonTargets.All);
                gameObject.GetComponent<PhotonView>().RPC("CopyObject", PhotonTargets.All, currentDisguise);
            }
        }
	}

	public void ResetAfterDeath()
	{
		SetCurrentDisguise("Villager");
		gameObject.GetComponent<PhotonView>().RPC("SwitchModels", PhotonTargets.All);
		gameObject.GetComponent<PhotonView>().RPC("CopyObject", PhotonTargets.All, currentDisguise);
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
                string newDigs = hit.collider.gameObject.name;
                if(newDigs.Contains("(Clone)")) newDigs = newDigs.Remove(newDigs.Length-7);
                currentDisguise = newDigs;
                gameObject.GetComponent<PhotonView>().RPC("CopyObject", PhotonTargets.All, newDigs);
            }
        }
    }

    [RPC]
    public void CopyObject(string otherName)
    {
        gameObject.GetComponent<MeshFilter>().mesh = PropMaster.instance.GetMeshByName(otherName);

        gameObject.GetComponent<MeshRenderer>().material = PropMaster.instance.GetMaterialByName(otherName);

        gameObject.GetComponent<MeshCollider>().sharedMesh = PropMaster.instance.GetColliderMeshByName(otherName);
    }
}
