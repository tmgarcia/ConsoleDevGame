using UnityEngine;
using System.Collections;

public class DragonMovement : MonoBehaviour {

    public bool airPlaneControls = true;
    private PhotonView photonView;
    private GameObject cam;
    public float speed = 5;
    private float speedLimiter = 0.95f;

	// Use this for initialization
	void Start () {
        photonView = gameObject.GetComponent<PhotonView>();
        PhotonNetwork.ConnectUsingSettings("0.1");
        cam = transform.FindChild("DragonCamera").gameObject;
	}

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    void OnJoinedLobby()
    {
        Debug.Log("JoinRandom");
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }

    void OnJoinedRoom()
    {
        if (photonView.isMine)
        {
            cam.GetComponent<Camera>().enabled = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (photonView.isMine)
        {
            Vector3 forward = gameObject.transform.forward;
            Vector3 right = gameObject.transform.right;
            Vector3 accelaration = forward * speed;
            if (Input.GetKey(KeyCode.W)) accelaration *= 2;
            if (Input.GetKey(KeyCode.S)) accelaration *= 0.5f;
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
            transform.position = Vector3.Lerp(transform.position, gameObject.GetComponent<NetworkAgent>().GetNetworkPosition(), Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, gameObject.GetComponent<NetworkAgent>().GetNetworkRotation(), Time.deltaTime * 5);
        }
	}
}
