using UnityEngine;
using System.Collections;

public class VillagerMovement : MonoBehaviour {

    private PhotonView photonView;
    private float speed = 50;
    private float speedLimiter = 0.95f;
    private GameObject cam;
    private Transform tether;

	// Use this for initialization
	void Start () {
        photonView = gameObject.GetComponent<PhotonView>();
        tether = transform.FindChild("CamTetherPoint");
        cam = tether.FindChild("VillagerCamera").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (photonView.isMine)
        {
            Vector3 forward = gameObject.transform.forward;
            Vector3 right = Vector3.Cross(forward, Vector3.up);
            Vector3 accelaration = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.W)) accelaration += forward;
            if (Input.GetKey(KeyCode.A)) accelaration += right;
            if (Input.GetKey(KeyCode.S)) accelaration -= forward;
            if (Input.GetKey(KeyCode.D)) accelaration -= right;
            accelaration.Normalize();
            gameObject.GetComponent<Rigidbody>().velocity += accelaration * Time.deltaTime * speed;
            gameObject.GetComponent<Rigidbody>().velocity *= speedLimiter;

            float deltaX = Input.GetAxis("Mouse X");
            float deltaY = Input.GetAxis("Mouse Y");

            Vector3 newYEuler = tether.rotation.eulerAngles;
            newYEuler.x += deltaY;
            tether.rotation = Quaternion.Euler(newYEuler);

            Vector3 newEuler = gameObject.GetComponent<Rigidbody>().rotation.eulerAngles;
            newEuler.y += deltaX;
            gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(newEuler));

            CamCollide();
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, gameObject.GetComponent<NetworkAgent>().GetNetworkPosition(), Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, gameObject.GetComponent<NetworkAgent>().GetNetworkRotation(), Time.deltaTime * 5);
        }
	}

    private void CamCollide()
    {
        if (cam.transform.localPosition.z > -5)
        {
            Vector3 newPos = cam.transform.localPosition;
            newPos.z = -5;
            cam.transform.localPosition = newPos;
        }

        Vector3 target = transform.FindChild("CamTetherPoint").position;
        Vector3 camPos = cam.transform.position;
        Quaternion camRot = cam.transform.rotation;

        RaycastHit[] hit = new RaycastHit[8];

        for (int i = 0; i < hit.Length; i++)
        {
            hit[i] = new RaycastHit();
        }

        Vector3[] points = new Vector3[8];

        points[0] = new Vector3(0.0f, 0.3f, 0.0f);
        points[1] = new Vector3(0.5f, 0.0f, 0.0f);
        points[2] = new Vector3(0.0f, -0.3f, 0.0f);
        points[3] = new Vector3(-0.5f, 0.0f, 0.0f);
        points[4] = new Vector3(0.3f, 0.2f, 0.0f);
        points[5] = new Vector3(-0.3f, 0.2f, 0.0f);
        points[6] = new Vector3(0.3f, -0.2f, 0.0f);
        points[7] = new Vector3(-0.3f, -0.2f, 0.0f);

        Debug.DrawLine(target, camPos + (camRot * (points[0])), Color.blue);
        Debug.DrawLine(target, camPos + (camRot * (points[1])), Color.red);
        Debug.DrawLine(target, camPos + (camRot * (points[2])), Color.gray);
        Debug.DrawLine(target, camPos + (camRot * (points[3])), Color.green);
        Debug.DrawLine(target, camPos + (camRot * (points[4])), Color.magenta);
        Debug.DrawLine(target, camPos + (camRot * (points[5])), Color.cyan);
        Debug.DrawLine(target, camPos + (camRot * (points[6])), Color.black);
        Debug.DrawLine(target, camPos + (camRot * (points[7])), Color.white);

        bool ithit = false;
        int closest = 0;

        for (int i = 0; i < hit.Length; i++)
        {
            if (Physics.Linecast(target, camPos + (camRot * (points[i])), out hit[i]))
            {
                if (!hit[i].collider.isTrigger && hit[i].collider.gameObject != gameObject)
                {
                    ithit = true;
                    if (Vector3.Distance(target, new Vector3(hit[i].point.x, hit[i].point.y, hit[i].point.z)) < Vector3.Distance(target, new Vector3(hit[closest].point.x, hit[closest].point.y, hit[closest].point.z)))
                    {
                        closest = i;
                    }
                }
            }
        }
        if (ithit)
        {
            Debug.DrawLine(target, target + Vector3.Project(new Vector3(hit[closest].point.x, hit[closest].point.y, hit[closest].point.z) - target, camRot * new Vector3(0.0f, 0.0f, -1.0f)), Color.yellow, 1);
            cam.transform.position = target + Vector3.Project(new Vector3(hit[closest].point.x, hit[closest].point.y, hit[closest].point.z) - target, camRot * new Vector3(0.0f, 0.0f, -1.0f));
        }
    }
}
