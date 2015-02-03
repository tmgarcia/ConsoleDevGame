using UnityEngine;
using System.Collections;

public class DragonMovement : MonoBehaviour
{
    private PhotonView photonView;
    private GameObject cam;
    public float glideSpeed = 120;
    public float hoverSpeed = 30;
    private float speedLimiter = 0.1f;
    public bool isLocal = false;
    public GameObject UIHelper;

    private GameObject transformOVR;
    private Transform camPosition;
    private bool mouseControls;

    // Use this for initialization
    void Start()
    {
        mouseControls = false;
        photonView = transform.parent.gameObject.GetComponent<PhotonView>();
        isLocal = transform.parent.GetComponent<NetworkAgent>().IsLocalCharacter();
        if (OVRManagerHelper.instance.IsLocalPlayerUsingOVR)
        {
            setUpOVR();
        }
        else if (isLocal)
        {
            cam = transform.FindChild("DragonCamera").gameObject;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLocal)
        {
            if (Options.instance.toggleShift)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) { mouseControls = !mouseControls; }
            }
            else
            {
                mouseControls = Input.GetKey(KeyCode.LeftShift);
            }
#region Oculs Controls
            if (OVRManagerHelper.instance.IsLocalPlayerUsingOVR)
            {
                if (mouseControls)
                {
                    Vector3 forward = transformOVR.transform.forward;
                    Vector3 right = transformOVR.transform.right;
                    Vector3 accelaration = forward * glideSpeed;
                    if (Input.GetMouseButton(1)) accelaration *= 0.3f;
                    gameObject.GetComponent<Rigidbody>().velocity += accelaration * Time.deltaTime;
                    gameObject.GetComponent<Rigidbody>().velocity *= Mathf.Pow(speedLimiter, Time.deltaTime);

                    GameObject.Find("CustomFire2").GetComponent<ParticleSystem>().startSpeed = 15.52f + gameObject.GetComponent<Rigidbody>().velocity.magnitude;

                    float deltaX = Input.GetAxis("Mouse X");
                    float deltaY = Input.GetAxis("Mouse Y");

                    if (Options.instance.airplaneMode)
                    {
                        transformOVR.transform.Rotate(new Vector3(0, 0, 1), -deltaX, Space.Self);
                        transformOVR.transform.Rotate(new Vector3(1, 0, 0), deltaY, Space.Self);
                    }
                    else
                    {
                        Vector3 newEuler = transformOVR.transform.rotation.eulerAngles;
                        newEuler.x -= deltaY;
                        newEuler.y += deltaX;
                        if (newEuler.x > 88 && newEuler.x < 268)
                        {
                            if (newEuler.x > 180) { newEuler.x = 268; }
                            if (newEuler.x < 180) { newEuler.x = 88; }
                        }
                        transformOVR.transform.rotation = Quaternion.Euler(newEuler);
                    }
                }
                else
                {
                    Vector3 direction = new Vector3(0, 0, 0);

                    Vector3 forward = transformOVR.transform.forward;
                    forward.y = 0;
                    forward = Vector3.Normalize(forward);

                    Vector3 right = transformOVR.transform.right;

                    if (Input.GetKey(KeyCode.W)) direction += forward;
                    if (Input.GetKey(KeyCode.A)) direction -= right;
                    if (Input.GetKey(KeyCode.S)) direction -= forward;
                    if (Input.GetKey(KeyCode.D)) direction += right;
                    if (Input.GetKey(KeyCode.E)) direction += Vector3.up;
                    if (Input.GetKey(KeyCode.Q)) direction -= Vector3.up;
                    if (direction.magnitude > 0) direction = Vector3.Normalize(direction);

                    Vector3 accelaration = direction * hoverSpeed;
                    gameObject.GetComponent<Rigidbody>().velocity += accelaration * Time.deltaTime;
                    gameObject.GetComponent<Rigidbody>().velocity *= Mathf.Pow(speedLimiter, Time.deltaTime);

                    GameObject.Find("CustomFire2").GetComponent<ParticleSystem>().startSpeed = 15.52f + gameObject.GetComponent<Rigidbody>().velocity.magnitude;

                    float deltaX = Input.GetAxis("Mouse X");
                    //float deltaY = Input.GetAxis("Mouse Y");
                    Vector3 newEuler = transformOVR.transform.rotation.eulerAngles;
                    //newEuler.x -= deltaY;
                    newEuler.y += deltaX;
                    transformOVR.transform.rotation = Quaternion.Euler(newEuler);
                }

                cam.transform.position = camPosition.position;
                cam.transform.rotation = transformOVR.transform.rotation;
                transform.rotation = cam.transform.GetChild(1).rotation;
#endregion
            }
            else
            {
#region Non-OculusControls
                if (!cam.GetComponent<Camera>().enabled)
                {
                    cam.GetComponent<Camera>().enabled = true;
                }
                if (mouseControls)
                {
                    Vector3 forward = gameObject.transform.forward;
                    Vector3 right = gameObject.transform.right;
                    Vector3 accelaration = forward * glideSpeed;
                    if (Input.GetKey(KeyCode.Mouse1)) accelaration *= 0.3f;
                    gameObject.GetComponent<Rigidbody>().velocity += accelaration * Time.deltaTime;
                    gameObject.GetComponent<Rigidbody>().velocity *= Mathf.Pow(speedLimiter, Time.deltaTime);

                    transform.parent.GetComponent<FireBreathing>().Fire.startSpeed = 15.52f + gameObject.GetComponent<Rigidbody>().velocity.magnitude;

                    float deltaX = Input.GetAxis("Mouse X");
                    float deltaY = Input.GetAxis("Mouse Y");

                    if (Options.instance.airplaneMode)
                    {
                        transform.Rotate(new Vector3(0, 0, 1), -deltaX, Space.Self);
                        transform.Rotate(new Vector3(1, 0, 0), deltaY, Space.Self);
                    }
                    else
                    {
                        Vector3 newEuler = gameObject.GetComponent<Rigidbody>().rotation.eulerAngles;
                        newEuler.x -= deltaY;
                        newEuler.y += deltaX;
                        if (newEuler.x > 88 && newEuler.x < 268)
                        {
                            if (newEuler.x > 180) { newEuler.x = 268; }
                            if (newEuler.x < 180) { newEuler.x = 88; }
                        }
                        gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(newEuler));
                    }
                }
                else
                {
                    if (Options.instance.airplaneMode)
                    {
                        if (transform.rotation.eulerAngles.z > 5.0f || transform.rotation.eulerAngles.z < 355.0f) 
                            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,0.0f), Time.deltaTime * 5);
                    }
                    
                    Vector3 direction = new Vector3(0, 0, 0);

                    Vector3 forward = gameObject.transform.forward;
                    forward.y = 0;
                    forward = Vector3.Normalize(forward);

                    Vector3 right = Vector3.Cross(forward,Vector3.up);

                    if (Input.GetKey(KeyCode.W)) direction += forward;
                    if (Input.GetKey(KeyCode.A)) direction += right;
                    if (Input.GetKey(KeyCode.S)) direction -= forward;
                    if (Input.GetKey(KeyCode.D)) direction -= right;
                    if (Input.GetKey(KeyCode.E)) direction += Vector3.up;
                    if (Input.GetKey(KeyCode.Q)) direction -= Vector3.up;
                    if(direction.magnitude>0)direction = Vector3.Normalize(direction);

                    Vector3 accelaration = direction * hoverSpeed;
                    gameObject.GetComponent<Rigidbody>().velocity += accelaration * Time.deltaTime;
                    gameObject.GetComponent<Rigidbody>().velocity *= Mathf.Pow(speedLimiter, Time.deltaTime);

                    transform.parent.GetComponent<FireBreathing>().Fire.startSpeed = 15.52f + gameObject.GetComponent<Rigidbody>().velocity.magnitude;

                    float deltaX = Input.GetAxis("Mouse X");
                    float deltaY = Input.GetAxis("Mouse Y");
                    Vector3 newEuler = gameObject.GetComponent<Rigidbody>().rotation.eulerAngles;
                    newEuler.x -= deltaY;
                    newEuler.y += deltaX;
                    if (newEuler.x > 88 && newEuler.x < 268)
                    {
                        if (newEuler.x > 180) { newEuler.x = 268; }
                        if (newEuler.x < 180) { newEuler.x = 88; }
                    }
                    gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(newEuler));
                }
#endregion
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
        GameObject directionHelper = GameObject.Instantiate(UIHelper) as GameObject;
        transformOVR = GameObject.Find("DragonOVR_Rotation");
        camPosition = transform.FindChild("CameraPosition").gameObject.transform;
        cam = GameObject.Find("OVRCameraRig").gameObject;

        Quaternion startingRotation = cam.transform.GetChild(0).transform.rotation;
        transform.rotation = startingRotation;
        transformOVR.transform.rotation = startingRotation;
        cam.transform.position = camPosition.position;
        cam.transform.parent = transform;
        transform.parent.GetComponent<FireBreathing>().Fire.gameObject.transform.parent = cam.transform.GetChild(1);
        OVRManagerHelper.instance.CalibrateOVR();
    }
}