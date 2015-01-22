using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour 
{
    public GameObject HudUI;
    public GameObject GameSetupUI;
    public GameObject OptionsUI;
    public GameObject DebugUI;
    public GameObject GameOverUI;

    private GameObject OVRCameraRig;
    private GameObject dummyObject;

    bool isBillboarding;
    public float UISpacingFromCamera = 0.3f;

    float cameraFar = 1000;

	void Start () 
    {
        if (OVRManagerHelper.instance.IsLocalPlayerUsingOVR)
        {
            SetUIToBillboard();
        }
        //SetUIToBillboard();
	}
	
	void Update () 
    {
        if (isBillboarding)
        {
            Transform cameraTransform = OVRCameraRig.transform.FindChild("CenterEyeAnchor").transform;
            dummyObject.transform.position = cameraTransform.position;
            dummyObject.transform.rotation = cameraTransform.rotation;
            dummyObject.transform.position = cameraTransform.position + cameraTransform.forward * UISpacingFromCamera;
            dummyObject.transform.LookAt(dummyObject.transform.position + cameraTransform.rotation * Vector3.back, cameraTransform.rotation * Vector3.up);
            dummyObject.transform.Rotate(new Vector3(0, 1, 0), 180);
            float scalingFactor = UISpacingFromCamera / cameraFar;

            if (HudUI != null)
            {
                HudUI.transform.position = dummyObject.transform.position;
                HudUI.transform.rotation = dummyObject.transform.rotation;

                HudUI.transform.localScale = new Vector3(scalingFactor, scalingFactor, 1);
            }
            if (GameSetupUI != null)
            {
                GameSetupUI.transform.position = dummyObject.transform.position;
                GameSetupUI.transform.rotation = dummyObject.transform.rotation;

                GameSetupUI.transform.localScale = new Vector3(scalingFactor, scalingFactor, 1);
            }
            if (OptionsUI != null)
            {
                OptionsUI.transform.position = dummyObject.transform.position;
                OptionsUI.transform.rotation = dummyObject.transform.rotation;

                OptionsUI.transform.localScale = new Vector3(scalingFactor, scalingFactor, 1);
            }
            if (DebugUI != null)
            {
                DebugUI.transform.position = dummyObject.transform.position;
                DebugUI.transform.rotation = dummyObject.transform.rotation;

                DebugUI.transform.localScale = new Vector3(scalingFactor, scalingFactor, 1);
            }
            if (GameOverUI != null)
            {
                GameOverUI.transform.position = dummyObject.transform.position;
                GameOverUI.transform.rotation = dummyObject.transform.rotation;

                GameOverUI.transform.localScale = new Vector3(scalingFactor, scalingFactor, 1);
            }
        }
	}

    public void SetUIToBillboard()
    {
        isBillboarding = true;
        dummyObject = new GameObject();
        OVRCameraRig = GameObject.Find("OVRCameraRig");

        if (HudUI != null)
        {
            HudUI.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        }
        if (GameSetupUI != null)
        {
            GameSetupUI.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        }
        if (OptionsUI != null)
        {
            OptionsUI.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        }
        if (DebugUI != null)
        {
            DebugUI.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        }
        if (GameOverUI != null)
        {
            GameOverUI.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        }
        
        //OVRCameraRig = Camera.main.gameObject;
    }
}
