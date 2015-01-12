using UnityEngine;
using System.Collections;

public class DragonMovement : MonoBehaviour {

    public bool airPlaneControls = true;
    private PhotonView photonView;
    private GameObject cam;
    public float speed = 5;
    private float speedLimiter = 0.95f;
    public bool isLocal = false;

	// Use this for initialization
	void Start () {
        photonView = transform.parent.gameObject.GetComponent<PhotonView>();
        cam = transform.FindChild("DragonCamera").gameObject;
        isLocal = transform.parent.GetComponent<NetworkAgent>().IsLocalCharacter();
	}
	
	// Update is called once per frame
	void Update () {
        if (isLocal)
        {
            if (!cam.GetActive())
            {
                cam.SetActive(true);
            }
            Vector3 forward = gameObject.transform.forward;
            Vector3 right = gameObject.transform.right;
            Vector3 accelaration = forward * speed;
            if (Input.GetKey(KeyCode.W)) accelaration *= 2;
            if (Input.GetKey(KeyCode.S)) accelaration *= 0.5f;
            if (Input.GetKeyDown(KeyCode.C)) airPlaneControls = !airPlaneControls;
            gameObject.GetComponent<Rigidbody>().velocity += accelaration * Time.deltaTime * speed;
            gameObject.GetComponent<Rigidbody>().velocity *= speedLimiter;

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
}
