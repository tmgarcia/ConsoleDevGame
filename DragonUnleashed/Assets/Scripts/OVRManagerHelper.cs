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
    private OVRManager ovrManager;

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
        ovrManager = GetComponent<OVRManager>();
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CalibrateOVR();
        }
	}

	private bool IsOculusConnected()
	{
		return Ovr.Hmd.Detect() > 0;
	}

    public void CalibrateOVR()
    {
        OVRManager.capiHmd.RecenterPose();
    }

	////////////FUTURE IMPLEMENTATION/////////////
	//public boolCheckOculusConnection();
	//print("connection: " + (Ovr.Hmd.Detect() > 0));
	//OVRManager.HMDAcquired += CheckOculusConnection;
	//OVRManager.HMDLost += CheckOculusConnection;
}
