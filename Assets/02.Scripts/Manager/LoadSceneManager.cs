using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
	public static LoadSceneManager instance;
	public Image fade;

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(this);
		}
		instance = this;
	}

	public void LoadScene(string StageName)
	{
		StartCoroutine(SceneFade(StageName));
	}
	IEnumerator SceneFade(string StageName)
	{
		fade.DOFade(1, 1f);
		while (true)
		{
			if (fade.color.a >= 1)
				break;
			yield return null;
		}
		SceneManager.LoadScene(StageName);		
	}
}
