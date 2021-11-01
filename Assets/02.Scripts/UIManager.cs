using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]private PlayerMove player;
    [SerializeField] private Sprite[] energySprite;
    public GameObject gameoverPanel;

    public List<Image> energyImage = new List<Image>();
    private List<Animator> energyImageAnimators = new List<Animator>();
    public List<Image> hpImage = new List<Image>();

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
        testSet();
        //Debug.Log(testImage.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.dead)
        {
            gameoverPanel.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            //testSet();
        }
    }
    public void StatUpdate()
    {
		for (int i = 0; i < player.initHealth; i++)
		{
			hpImage[i].gameObject.SetActive(false);
		}


		for (int i = 0; i < player.hp; i++)
		{
			hpImage[i].gameObject.SetActive(true);
		}
	}

    public void testSet()
    {
		for (int i = 0; i < player.maxEnergy; i++)
		{
			energyImage[i].gameObject.SetActive(true);
		}


	}
    public void testUpdate(bool isDown)
    {
        //energyImage[player.energy - 1].gameObject.SetActive(false);
        if (isDown)
            energyImageAnimators[player.energy - 1].SetInteger("index", 1);
        else
            energyImageAnimators[player.energy].SetInteger("index", 2);
        //      for (int i = 0; i < player.energy; i++)
        //{
        //	if (i == player.energy-1)
        //		
        //		energyImage[i].gameObject.SetActive(false);
        //}
    }
    public void use()
    {
        SceneManager.LoadScene(0);
    }
}
