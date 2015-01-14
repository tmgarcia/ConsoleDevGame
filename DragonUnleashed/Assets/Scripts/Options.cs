using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Options : MonoBehaviour
{
	public Button resolutionSelection;
	public Button[] resolutionOptions;
	public bool toggleOptions = false;

	void Start ()
	{
		resolutionSelection = GameObject.Find("ResButton").GetComponent<Button>();
		resolutionOptions = resolutionSelection.GetComponentsInChildren<Button>();

		foreach (Button b in resolutionOptions)
		{
			b.enabled = false;
			//resolutionSelection.onClick += b.
		}
	}

	void Update ()
	{
	
	}
}
