using UnityEngine;
using System.Collections.Generic;

public class OVRManagerHelper : MonoBehaviour
{
	public bool IsLocalPlayerUsingOVR;
	public static OVRManagerHelper instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        IsLocalPlayerUsingOVR = IsOculusConnected();
        print("Found OVR? " + IsLocalPlayerUsingOVR);

        if (!IsLocalPlayerUsingOVR)
        {
            gameObject.SetActive(false);
        }
    }

	void Start ()
	{
		
	}

	void Update ()
	{
	
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
