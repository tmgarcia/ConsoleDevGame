using UnityEngine;
using System.Collections.Generic;

public class OVRManagerHelper : MonoBehaviour
{
	public bool IsLocalPlayerUsingOVR;
	public static OVRManagerHelper instance;

    public GameSetupManager gameSetupManager;
    public bool readyToPlay { get; set; }
    private bool dragonSet;
    private bool gameStarted;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            print("OVRManagerHelper is instantiated");
        }

        IsLocalPlayerUsingOVR = IsOculusConnected();
        print("Found OVR? " + IsLocalPlayerUsingOVR);

        if (!IsLocalPlayerUsingOVR)
        {
            gameObject.SetActive(false);
        }
        readyToPlay = false;
        dragonSet = false;
        gameStarted = false;
    }

	void Start ()
	{
		
	}

	void Update ()
	{
        if (IsLocalPlayerUsingOVR && readyToPlay && !dragonSet)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                gameSetupManager.MoveSelfToSeekers();
                gameSetupManager.SetSelfToReady();
                dragonSet = true;
            }
        }
        else if (dragonSet && !gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                gameStarted = true;
                gameSetupManager.StartGame();
            }
        }
	}

	private bool IsOculusConnected()
	{
		return Ovr.Hmd.Detect() > 0;
	}

	////////////FUTURE IMPLEMENTATION/////////////
	//public boolCheckOculusConnection();
	//print("connection: " + (Ovr.Hmd.Detect() > 0));
	//OVRManager.HMDAcquired += CheckOculusConnection;
	//OVRManager.HMDLost += CheckOculusConnection;
}
