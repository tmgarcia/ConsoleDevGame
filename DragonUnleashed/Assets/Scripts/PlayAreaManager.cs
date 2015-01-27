using UnityEngine;
using System.Collections.Generic;

public class PlayAreaManager : MonoBehaviour
{
	void Start ()
	{
	
	}

	void Update ()
	{
		//gameObject.GetComponent<Material>().SetTextureOffset(gameObject.GetComponent<Material>().GetTextureOffset() + Time.deltaTime);
		//renderer.material.SetTextureOffset("PlayAreaLines", renderer.material.GetTextureOffset("PlayAreaLines") + new Vector2(Time.deltaTime, Time.deltaTime));
		renderer.material.mainTextureOffset += new Vector2(Time.deltaTime / 5, Time.deltaTime / 5);
	}
}
