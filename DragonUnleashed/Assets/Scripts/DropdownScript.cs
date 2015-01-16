using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DropdownScript : MonoBehaviour
{
	public List<Button> DropDownButtons;
	public bool IsDropdownShown = false;

	void Start()
	{
		if (DropDownButtons == null)
		{
			DropDownButtons = new List<Button>();
		}

		DropDownButtons.AddRange(transform.GetComponentsInChildren<Button>());
		DropDownButtons.Remove(GetComponent<Button>());

		foreach (var item in DropDownButtons)
		{
			item.gameObject.SetActive(IsDropdownShown);
		}
		IsDropdownShown = false;
	}

	public void AddButton(Button button)
	{
		DropDownButtons.Add(button);
	}

	public void AddButtons(IEnumerable<Button> buttons)
	{
		DropDownButtons.AddRange(buttons);
	}

	public void RemoveButton(Button button)
	{
		DropDownButtons.Remove(button);
	}

	public void ToggleDropdown()
	{
		IsDropdownShown = !IsDropdownShown;

		foreach (var item in DropDownButtons)
		{
			item.gameObject.SetActive(IsDropdownShown);
		}
	}
}
