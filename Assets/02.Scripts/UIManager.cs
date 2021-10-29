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
    }
	// Start is called before the first frame update
	void Start()
    {
        test();
        Debug.Log(testImage.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.dead)
        {
            gameoverPanel.SetActive(true);
        }
    }
    public void StatUpdate()
    {
        for (int i = 0; i < player.initHealth; i++)
        {
            hpImage[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < player.maxEnergy; i++)
        {
            energyImage[i].sprite = energySprite[2];
            if (i %2==0)
            {
                energyImage[i].gameObject.SetActive(true);
            }
        }

        for (int i = 0; i < player.hp; i++)
        {
            hpImage[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < player.energy; i++)
        {
            energyImage[i].sprite = energySprite[0];
            if (i == player.energy - 1)
            {
                if (i % 2 == 0)
                {
                    energyImage[i].sprite = energySprite[1];
                }
                else
                {
                    energyImage[i].sprite = energySprite[2];
                }
            }
        }
	}

    public void test()
    {
		for (int i = 0; i < player.maxEnergy/2; i++)
		{
            //energyImage[i].gameObject.SetActive(true);
            testImage.Enqueue(energyImage[i]);
        }

    }
    public void testUpdate()
    {
		for (int i = 0; i < player.energy; i++)
		{
			if (i >= (testImage.Count * 2)-1)
			{

			}
		}
	}
    public void use()
    {
        SceneManager.LoadScene(0);
    }
}
