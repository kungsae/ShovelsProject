using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadObj : InteractObj
{
    public string stageName;
	protected override void Interact()
	{
		base.Interact();
        print("�� �ε�");
        LoadSceneManager.instance.LoadScene(stageName);
	}
}
