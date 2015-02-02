using UnityEngine;
using System.Collections;

public class Mimic : MonoBehaviour {

    public GameObject[] disguises;

    private class Form{
        public Collider collider;
        public Mesh mesh;
        public Material material;
    }

    private Form trueForm;
    private Form currentForm;

	// Use this for initialization
	void Start () {
	    trueForm = new Form();
        currentForm = new Form();
        trueForm.collider = new CapsuleCollider();
        ((CapsuleCollider)trueForm.collider).center    = GetComponent<CapsuleCollider>().center;
        ((CapsuleCollider)trueForm.collider).radius    = GetComponent<CapsuleCollider>().radius;
        ((CapsuleCollider)trueForm.collider).height    = GetComponent<CapsuleCollider>().height;
        ((CapsuleCollider)trueForm.collider).direction = GetComponent<CapsuleCollider>().direction;

        trueForm.mesh = gameObject.GetComponent<MeshFilter>().mesh;

        trueForm.material = gameObject.renderer.material;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E)) MimicProp();
        if (Input.GetKeyDown(KeyCode.Mouse1)) gameObject.GetComponent<PhotonView>().RPC("CopyObject", PhotonTargets.All, trueForm);
        if (Input.GetKeyUp(KeyCode.Mouse1)) gameObject.GetComponent<PhotonView>().RPC("CopyObject", PhotonTargets.All, currentForm);
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
        currentForm.mesh = other.GetComponent<MeshFilter>().mesh;

        gameObject.renderer.material = other.renderer.material;
        currentForm.material = other.renderer.material;
        
        DestroyImmediate(gameObject.collider);

        if(other.collider.GetType()==typeof(BoxCollider))
        {
            gameObject.AddComponent<BoxCollider>();
            currentForm.collider = new BoxCollider();
            gameObject.GetComponent<BoxCollider>().center = other.GetComponent<BoxCollider>().center;
            gameObject.GetComponent<BoxCollider>().size = other.GetComponent<BoxCollider>().size;
            ((BoxCollider)currentForm.collider).center = other.GetComponent<BoxCollider>().center;
            ((BoxCollider)currentForm.collider).size = other.GetComponent<BoxCollider>().size;
        }
        else if(other.collider.GetType()==typeof(CapsuleCollider))
        {
            gameObject.AddComponent<CapsuleCollider>();
            currentForm.collider = new CapsuleCollider();
            gameObject.GetComponent<CapsuleCollider>().center = other.GetComponent<CapsuleCollider>().center;
            gameObject.GetComponent<CapsuleCollider>().radius = other.GetComponent<CapsuleCollider>().radius;
            gameObject.GetComponent<CapsuleCollider>().height = other.GetComponent<CapsuleCollider>().height;
            gameObject.GetComponent<CapsuleCollider>().direction = other.GetComponent<CapsuleCollider>().direction;
            ((CapsuleCollider)currentForm.collider).center = other.GetComponent<CapsuleCollider>().center;
            ((CapsuleCollider)currentForm.collider).radius = other.GetComponent<CapsuleCollider>().radius;
            ((CapsuleCollider)currentForm.collider).height = other.GetComponent<CapsuleCollider>().height;
            ((CapsuleCollider)currentForm.collider).direction = other.GetComponent<CapsuleCollider>().direction;
        }
        else if (other.collider.GetType() == typeof(SphereCollider))
        {
            gameObject.AddComponent<SphereCollider>();
            currentForm.collider = new SphereCollider();
            gameObject.GetComponent<SphereCollider>().center = other.GetComponent<SphereCollider>().center;
            gameObject.GetComponent<SphereCollider>().radius = other.GetComponent<SphereCollider>().radius;
            ((SphereCollider)currentForm.collider).center = other.GetComponent<SphereCollider>().center;
            ((SphereCollider)currentForm.collider).radius = other.GetComponent<SphereCollider>().radius;
        }
        
        Destroy(other);
    }

    [RPC]
    private void CopyObject(Form form)
    {
        gameObject.GetComponent<MeshFilter>().mesh = form.mesh;

        gameObject.renderer.material = form.material;

        DestroyImmediate(gameObject.collider);

        if (form.collider.GetType() == typeof(BoxCollider))
        {
            gameObject.AddComponent<BoxCollider>();
            gameObject.GetComponent<BoxCollider>().center = ((BoxCollider)form.collider).center;
            gameObject.GetComponent<BoxCollider>().size = ((BoxCollider)form.collider).size;
        }
        else if (form.collider.GetType() == typeof(CapsuleCollider))
        {
            gameObject.AddComponent<CapsuleCollider>();
            gameObject.GetComponent<CapsuleCollider>().center = ((CapsuleCollider)form.collider).center;
            gameObject.GetComponent<CapsuleCollider>().radius = ((CapsuleCollider)form.collider).radius;
            gameObject.GetComponent<CapsuleCollider>().height = ((CapsuleCollider)form.collider).height;
            gameObject.GetComponent<CapsuleCollider>().direction = ((CapsuleCollider)form.collider).direction;
        }
        else if (form.collider.GetType() == typeof(SphereCollider))
        {
            gameObject.AddComponent<SphereCollider>();
            gameObject.GetComponent<SphereCollider>().center = ((SphereCollider)form.collider).center;
            gameObject.GetComponent<SphereCollider>().radius = ((SphereCollider)form.collider).radius;
        }
    }
}
