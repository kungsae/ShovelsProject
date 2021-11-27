using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startData : MonoBehaviour
{
    public static startData instance { get; private set; }
	// Start is called before the first frame update
	private void Awake()
	{
		if (instance != null)
		{
			Destroy(this);
		}
		instance = this;
		DontDestroyOnLoad(this);
	}

}
