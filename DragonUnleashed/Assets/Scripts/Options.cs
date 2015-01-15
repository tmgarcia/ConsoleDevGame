using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Options : MonoBehaviour
{
	//allow for easy addition of solutions via outlet
	//public List<Vector2> ResolutionsWH;

	public List<Resolution> resolutionOptions;
	public Resolution current;
	public bool isFullScreen;

	void Start()
	{
		resolutionOptions = new List<Resolution>();

		foreach (var importedResolution in ResolutionsWH)
		{
			resolutionOptions.Add(new Resolution { width = (int)importedResolution.x, height = (int)importedResolution.y });
		}

		current = Screen.GetResolution[0];
	}

	void Update()
	{

	}

	public void SetResolution(int width, int height)
	{
		Screen.SetResolution(width, height, true);
	}

	public void SetResolution(Resolution resolution)
	{

	}

	public void SetResolution(ResolutionInfo ri)
	{

	}
}
