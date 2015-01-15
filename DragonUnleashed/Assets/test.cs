using UnityEngine;
using System.Collections.Generic;

public class test : MonoBehaviour
{
	public static GameObject camRef;

	void Awake()
	{
		camRef = gameObject;
		gameObject.SetActive(false);
	}

	void Start ()
	{
	
	}

	void Update ()
	{
	
	}
}
