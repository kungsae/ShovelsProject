using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Story : MonoBehaviour
{
	public TextMeshProUGUI text;
	public Image fade;

	private bool next = true;
	public Image[] image;
	public string[] storys;
	public string story;
	private int count = 0;
	private int imageCount = 0;
	private int isFirst = 0;

	public GameObject nextButton;
	private void Start()
	{
		textOut();
	}
	private void Update()
	{
		if (Input.anyKeyDown)
		{
			textOut();
		}
	}

	public void textOut()
	{
		nextButton.SetActive(false);
		if (count == storys.Length&&next)
		{
			StartCoroutine(SceneLoad());
		}
		if (count % 3 == 0 && next)
		{
			StartCoroutine(delay());

		}

		if(next&&count != storys.Length)
		{
			text.text = "";
			next = false;
			story = storys[count];
			StartCoroutine(Typing(story));
			count++;
		}
	}
	IEnumerator SceneLoad()
	{
		fade.DOFade(1, 1);
		isFirst++;
		PlayerPrefs.SetInt("isFirst", isFirst);
		yield return new WaitForSeconds(1.1f);
		SceneManager.LoadScene(2);
	}
	IEnumerator delay()
	{
		print(imageCount);
		next = false;
		for (int i = 0; i < image.Length; i++)
		{
			if (i != imageCount)
				image[i].DOFade(0f, 2f);
		}
		if(imageCount!=0)
		yield return new WaitForSeconds(2f);
		image[imageCount].DOFade(1f, 2f);
		yield return new WaitForSeconds(2.5f);
		text.text = "";
		next = false;
		story = storys[count];
		StartCoroutine(Typing(story));
		count++;
		imageCount++;
	}
	IEnumerator Typing(string _text)
	{
		char[] a = _text.ToCharArray();
		for (int i = 0; i < _text.Length; i++)
		{
			if (a[i] == 'n')
			{
				text.text += "\n";
				yield return new WaitForSeconds(0.1f);
				continue;
			}
				text.text += a[i].ToString();
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(0.1f);
		next = true;
		nextButton.SetActive(true);
	}
}
