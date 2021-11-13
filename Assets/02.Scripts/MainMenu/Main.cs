using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public Text text;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(()=>SceneManager.LoadScene(1));
        text.DOFade(0, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }

}
