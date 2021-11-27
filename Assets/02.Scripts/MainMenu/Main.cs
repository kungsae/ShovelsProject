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
    private int isFirst = 0;
    // Start is called before the first frame update
    void Start()
    {
        isFirst = PlayerPrefs.GetInt("isFirst", isFirst);
        button.onClick.AddListener(()=> fade.DOFade(1, 1f));
        text.DOFade(0, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }
	private void Update()
	{
        if (Input.anyKeyDown)
        {
            button.onClick.Invoke();
        }
        if (fade.color.a >= 1)
        {
            if (isFirst == 0)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                SceneManager.LoadScene(2);
            }
        }
 

    }
}

