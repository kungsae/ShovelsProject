using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public Text text;
    public Button newtBtn;
    public Button loadBtn;
    public Image fade;
    private int isFirst = 0;
    // Start is called before the first frame update
    void Start()
    {
        isFirst = PlayerPrefs.GetInt("isFirst", isFirst);
        newtBtn.onClick.AddListener(() =>
        {
            PlayerPrefs.DeleteAll();
            isFirst = 0;
            LoadSceneManager.instance.LoadScene("CutScene");
            });
        if (isFirst == 0)
        {
            loadBtn.interactable = false;
        }
        loadBtn.onClick.AddListener(()=> LoadSceneManager.instance.LoadScene("MainStage"));

        text.DOFade(0, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }
}

