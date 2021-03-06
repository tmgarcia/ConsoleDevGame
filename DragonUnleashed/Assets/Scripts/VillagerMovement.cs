﻿using UnityEngine;
using System.Collections;

public class VillagerMovement : MonoBehaviour {

    private PhotonView photonView;
    private VillagerFloorDetection floorDetection;
    public float speed = 30;
    public float aimSpeed = 15;
    public float jumpheight = 0.3f;
    private float speedLimiter = 0.1f;
    private float cameraSwitchSpeed = 30;
    private float cameraRestingDistance = 5;
    private GameObject cam;
    private Transform tether;
    private Vector3 overhead = new Vector3(0.0f,0.9f,0.0f);
    private Vector3 overshoulder = new Vector3(1.2f, 0.45f, 0.0f);
    public bool isLocal = false;
    public bool isGrounded { get; set; }

	// Use this for initialization
	void Start () 
    {
        isGrounded = true;
        photonView = gameObject.GetComponent<PhotonView>();
        floorDetection = gameObject.GetComponent<VillagerFloorDetection>();
        tether = transform.FindChild("CamTetherPoint");
        cam = tether.FindChild("VillagerCamera").gameObject;
        isLocal = GetComponent<NetworkAgent>().IsLocalCharacter();
	}
	
	// Update is called once per frame
	void Update () {
        if (isLocal)
        {
            if (!cam.GetComponent<Camera>().enabled)
            {
                cam.GetComponent<Camera>().enabled=true;
            }
            Vector3 forward = gameObject.transform.forward;
            Vector3 right = Vector3.Cross(forward, Vector3.up);
            Vector3 accelaration = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.W)) accelaration += forward;
            if (Input.GetKey(KeyCode.A)) accelaration += right;
            if (Input.GetKey(KeyCode.S)) accelaration -= forward;
            if (Input.GetKey(KeyCode.D)) accelaration -= right;
            accelaration.Normalize();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                floorDetection.CheckIfGrounded();
                if (floorDetection.isGrounded) accelaration += Vector3.up * jumpheight * (1 / Time.deltaTime);
            }

            gameObject.GetComponent<Rigidbody>().velocity += accelaration * Time.deltaTime * ((gameObject.GetComponent<Archery>().GetAiming())? aimSpeed:speed);
            float storedY = gameObject.GetComponent<Rigidbody>().velocity.y;
            gameObject.GetComponent<Rigidbody>().velocity *= Mathf.Pow(speedLimiter,Time.deltaTime);

            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(gameObject.GetComponent<Rigidbody>().velocity.x, storedY, gameObject.GetComponent<Rigidbody>().velocity.z);

            if (gameObject.GetComponent<Archery>().GetAiming()&&Vector3.Distance(tether.localPosition,overshoulder)>0.01f)
            {
                tether.localPosition = Vector3.Lerp(tether.localPosition, overshoulder, Time.deltaTime * cameraSwitchSpeed);
                cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(0, 0,-2), Time.deltaTime * cameraSwitchSpeed);
                cameraRestingDistance = Mathf.Lerp(cameraRestingDistance, 2, Time.deltaTime * cameraSwitchSpeed);
            }
            else if (!gameObject.GetComponent<Archery>().GetAiming() && Vector3.Distance(tether.localPosition, overhead) > 0.01f)
            {
                tether.localPosition = Vector3.Lerp(tether.localPosition, overhead, Time.deltaTime * cameraSwitchSpeed);
                cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(0.0f, 0.0f,-4.6f), Time.deltaTime * cameraSwitchSpeed);
                cameraRestingDistance = Mathf.Lerp(cameraRestingDistance, 5, Time.deltaTime * cameraSwitchSpeed);
            }

            float deltaX = Input.GetAxis("Mouse X");
            float deltaY = Input.GetAxis("Mouse Y");

            Vector3 newYEuler = tether.rotation.eulerAngles;
            newYEuler.x -= deltaY;
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

    private bool CamCollide()
    {
        if (cam.transform.localPosition.z > -cameraRestingDistance)
        {
            Vector3 newPos = cam.transform.localPosition;
            newPos.z = -cameraRestingDistance;
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

        bool ithit = false;
        int closest = 0;

        int layermask = 1 << 8;
        layermask = ~layermask;

        for (int i = 0; i < hit.Length; i++)
        {
            if (Physics.Linecast(target, camPos + (camRot * (points[i])), out hit[i],layermask))
            {
                if (!hit[i].collider.isTrigger)
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
            cam.transform.position = target + Vector3.Project(new Vector3(hit[closest].point.x, hit[closest].point.y, hit[closest].point.z) - target, camRot * new Vector3(0.0f, 0.0f, -1.0f));
        }
        return ithit;
    }
}










//bool result = false;
//int layermask = 1 << 8;
//layermask = ~layermask;

//Collider theCollider = transform.FindChild("Villager").gameObject.GetActive()? transform.FindChild("Villager").transform.FindChild("default").collider : collider;

//Debug.DrawLine(theCollider.bounds.center, theCollider.bounds.center - new Vector3(0.0f, theCollider.bounds.size.y * 0.5f + 0.05f, 0.0f), Color.yellow, 1.0f);
//RaycastHit hit = new RaycastHit();
//if (Physics.Linecast(theCollider.bounds.center, theCollider.bounds.center - new Vector3(0.0f, theCollider.bounds.size.y * 0.5f + 0.05f, 0.0f), out hit, layermask))
//{
//    if (hit.collider != theCollider) result = true;
//}