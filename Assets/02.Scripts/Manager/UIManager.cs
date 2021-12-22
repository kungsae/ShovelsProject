using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private PlayerMove player;
    [SerializeField] private Sprite[] energySprite;
    public GameObject gameoverPanel;

    public List<GameObject> energyGroup = new List<GameObject>();
    public List<Image> energyImage = new List<Image>();
    private List<Animator> energyImageAnimators = new List<Animator>();
    public List<Image> hpImage = new List<Image>();

    public ParticleSystem hpParticle;

    public Text coinText;
    public Image fade;

    Animator animator;

    private Queue<Image> testImage = new Queue<Image>();
    public GameObject escPanel;
    public Button exit;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("ui 매니저 중복");
            Destroy(this);
        }
        instance = this;
        if (energyImage.Count > 0)
        {
            for (int i = 0; i < energyImage.Count; i++)
            {
                energyImageAnimators.Add(energyImage[i].GetComponent<Animator>());

            }
            exit.onClick.AddListener(ExitGame);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (energyImage.Count > 0)
        {
            coinUi();
            EnergySet();
        }
        StartCoroutine(StartFade());
        //Debug.Log(testImage.Count);
    }
    IEnumerator StartFade()
    {
        fade.gameObject.SetActive(true);
        fade.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(1f);

        fade.DOFade(0, 1f);
    }
    // Update is called once per frame
    void Update()
    {
        if (energyImage.Count <= 0)
        {
            return;
        }
        if (player.dead)
        {
            gameoverPanel.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escPanel.SetActive(!escPanel.activeSelf);
            Time.timeScale = escPanel.activeSelf ? 0 : 1;
        }
    }

    public void StatUpdate(bool isDamage = false)
    {
        for (int i = 0; i < player.maxHp; i++)
        {
            hpImage[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < player.hp; i++)
        {
            hpImage[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < player.maxEnergy / 2; i++)
        {
            energyGroup[i].SetActive(true);
        }
        if (isDamage)
        {
            hpParticle.transform.position = Camera.main.ScreenToWorldPoint(hpImage.Find(x => x.gameObject.activeSelf == false).gameObject.transform.position + new Vector3(0,-1f,0));
            hpParticle.Play();
        }
    }

    public void EnergySet()
    {
        for (int i = 0; i < player.maxEnergy; i++)
        {
            energyImage[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < player.maxEnergy / 2; i++)
        {
            energyGroup[i].SetActive(true);
        }


    }
    public void EnergyUpdate(bool isDown)
    {
        if (isDown)
            energyImageAnimators[player.energy - 1].SetInteger("index", 1);
        else
            energyImageAnimators[player.energy].SetInteger("index", 2);
    }
    //현재 돈 표시
    public void coinUi()
    {
        coinText.text = player.money.ToString("G");
    }
    //씬 다시 불러오기(게임 재시작)
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
