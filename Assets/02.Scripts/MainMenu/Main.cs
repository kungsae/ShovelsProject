using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public Text text;
    public Button button;
    public Image fade;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(()=> fade.DOFade(1, 1f));
        text.DOFade(0, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }
	private void Update()
	{
        if (fade.color.a >= 1)
            SceneManager.LoadScene(1);
    }
}

