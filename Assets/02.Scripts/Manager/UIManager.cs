using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    Animator animator;

    private Queue<Image> testImage = new Queue<Image>();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("ui 매니저 중복");
            Destroy(this);
        }
        instance = this;
        for (int i = 0; i < energyImage.Count; i++)
        {
            energyImageAnimators.Add(energyImage[i].GetComponent<Animator>());

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        coinUi();
        EnergySet();
        //Debug.Log(testImage.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.dead)
        {
            gameoverPanel.SetActive(true);
        }
    }
    public void StatUpdate(bool isDamage = false)
    {
        for (int i = 0; i < player.initHealth; i++)
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
    public void coinUi()
    {
        coinText.text = player.money.ToString("G");
    }
    public void use()
    {
        SceneManager.LoadScene(0);
    }
}
