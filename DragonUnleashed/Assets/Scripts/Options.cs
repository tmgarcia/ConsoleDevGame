using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Options : MonoBehaviour
{
	//allow for easy addition of solutions via outlet
	public List<Vector2> ResolutionsWH;
	public List<Resolution> resolutionOptions;
	public Resolution current;
	private Canvas optionsCanvas;

	void Start()
	{
		resolutionOptions = new List<Resolution>();

		foreach (var importedResolution in ResolutionsWH)
		{
			resolutionOptions.Add(new Resolution { width = (int)importedResolution.x, height = (int)importedResolution.y });
		}

		current = Screen.GetResolution[0];
		optionsCanvas = GetComponent<Canvas>();
		optionsCanvas.enabled = false;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			optionsCanvas.enabled = !optionsCanvas.enabled;
            Screen.lockCursor = !optionsCanvas.enabled;
		}
	}

	public void SetResolution(int width, int height)
	{
		if (current.height != height || current.width != width)
		{
			current = new Resolution { width = width, height = height };
			Screen.SetResolution(width, height, Screen.fullScreen);
		}
	}

	public void SetResolution(Resolution resolution)
	{
		if (current.height != resolution.height || current.width != resolution.width)
		{
			current = resolution;
			Screen.SetResolution(current.width, current.height, Screen.fullScreen);
		}
	}

	public void SetResolution(ResolutionInfo ri)
	{
		if (current.height != ri.Height || current.width != ri.Width)
		{
			current = new Resolution { height = ri.Height, width = ri.Width };
			Screen.SetResolution(current.width, current.height, Screen.fullScreen);
		}
	}

	public void ToggleFullscreen()
	{
		Screen.fullScreen = !Screen.fullScreen;
	}
}
