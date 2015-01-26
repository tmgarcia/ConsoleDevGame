using UnityEngine;
using System.Collections.Generic;

public class OVRManagerHelper : MonoBehaviour
{
	public bool IsLocalPlayerUsingOVR;
	public static OVRManagerHelper instance;

    public GameSetupManager gameSetupManager;
    public bool readyToPlay { get; set; }
    private bool dragonSet;
    private bool dragonReady;
    private bool gameStarted;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //print("OVRManagerHelper is instantiated");
        }

        IsLocalPlayerUsingOVR = IsOculusConnected();
        //print("Found OVR? " + IsLocalPlayerUsingOVR);

        if (!IsLocalPlayerUsingOVR)
        {
            gameObject.SetActive(false);
        }
        readyToPlay = false;
        dragonSet = false;
        dragonReady = false;
        gameStarted = false;
    }

	void Start ()
	{
		
	}

	void Update ()
	{
        if (IsLocalPlayerUsingOVR && readyToPlay && !dragonSet && !dragonReady)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                gameSetupManager.MoveSelfToSeekers();
                dragonSet = true;
            }
        }
        else if (dragonSet && !dragonReady)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                gameSetupManager.SetSelfToReady();
                dragonReady = true;
            }
        }
        else if (dragonSet && dragonReady && !gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                gameStarted = true;
                gameSetupManager.StartGame();
            }
        }

        if (gameStarted && Input.GetKeyDown(KeyCode.A))
        {
            Calibrate();
        }
	}

	private bool IsOculusConnected()
	{
		return Ovr.Hmd.Detect() > 0;
	}

    private void Calibrate()
    {
        if (transform.parent != null)
        {
            Transform parentObject = transform.parent;
            transform.rotation = parentObject.rotation;
            transform.GetChild(0).transform.rotation = parentObject.rotation;
            transform.GetChild(1).transform.rotation = parentObject.rotation;
            transform.GetChild(2).transform.rotation = parentObject.rotation;
        }
    }

	////////////FUTURE IMPLEMENTATION/////////////
	//public boolCheckOculusConnection();
	//print("connection: " + (Ovr.Hmd.Detect() > 0));
	//OVRManager.HMDAcquired += CheckOculusConnection;
	//OVRManager.HMDLost += CheckOculusConnection;
}
