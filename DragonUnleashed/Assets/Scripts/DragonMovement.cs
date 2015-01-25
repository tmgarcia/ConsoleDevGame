using UnityEngine;
using System.Collections;

public class DragonMovement : MonoBehaviour {

    public bool airPlaneControls = true;
    private PhotonView photonView;
    private GameObject cam;
    public float speed = 6;
    private float speedLimiter = 0.1f;
    public bool isLocal = false;

	// Use this for initialization
	void Start () 
    {
        photonView = transform.parent.gameObject.GetComponent<PhotonView>();
        if (OVRManagerHelper.instance.IsLocalPlayerUsingOVR)
        {
            setUpOVR();
        }
        else
        {
            cam = transform.FindChild("DragonCamera").gameObject;
        }
        isLocal = transform.parent.GetComponent<NetworkAgent>().IsLocalCharacter();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (isLocal)
        {
            if (!OVRManagerHelper.instance.IsLocalPlayerUsingOVR)
            {
                if (!cam.GetComponent<Camera>().enabled)
                {
                    cam.GetComponent<Camera>().enabled = true;
                }
            }
            Vector3 forward = gameObject.transform.forward;
            Vector3 right = gameObject.transform.right;
            Vector3 accelaration = forward * speed;
            if (Input.GetKey(KeyCode.W)) accelaration *= 3;
            if (Input.GetKey(KeyCode.S)) accelaration *= 0.0f;
            if (Input.GetKeyDown(KeyCode.C)) airPlaneControls = !airPlaneControls;
            gameObject.GetComponent<Rigidbody>().velocity += accelaration * Time.deltaTime * speed;
            gameObject.GetComponent<Rigidbody>().velocity *= Mathf.Pow(speedLimiter,Time.deltaTime);

            GameObject.Find("CustomFire2").GetComponent<ParticleSystem>().startSpeed = 15.52f + gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            float deltaX = Input.GetAxis("Mouse X");
            float deltaY = Input.GetAxis("Mouse Y");

            if (airPlaneControls)
            {
                transform.Rotate(new Vector3(0, 0, 1),-deltaX, Space.Self);
                transform.Rotate(new Vector3(1, 0, 0), deltaY, Space.Self);
            }
            else
            {
                Vector3 newEuler = gameObject.GetComponent<Rigidbody>().rotation.eulerAngles;
                newEuler.x -= deltaY;
                newEuler.y += deltaX;
                gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(newEuler));
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, transform.parent.gameObject.GetComponent<NetworkAgent>().GetNetworkPosition(), Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, transform.parent.gameObject.GetComponent<NetworkAgent>().GetNetworkRotation(), Time.deltaTime * 5);
        }
	}

    void setUpOVR()
    {
        Transform camPosition = transform.FindChild("CameraPosition").gameObject.transform;
        cam = GameObject.Find("OVRCameraRig").gameObject;
        GameObject fire = this.transform.FindChild("CustomFire2").gameObject;

        cam.transform.position = camPosition.position;
        cam.transform.rotation = camPosition.rotation;
        cam.transform.GetChild(0).transform.rotation = camPosition.rotation;
        cam.transform.GetChild(1).transform.rotation = camPosition.rotation;
        cam.transform.GetChild(2).transform.rotation = camPosition.rotation;
        cam.transform.parent = this.transform;
        cam.transform.localPosition += new Vector3(0.0f, 0.23f, 0.0f);
        cam.SetActive(true);

        fire.transform.parent = cam.transform.GetChild(1);
    }
}
