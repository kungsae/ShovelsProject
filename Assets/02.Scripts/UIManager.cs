using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]private PlayerMove player;
    [SerializeField] private Sprite[] energySprite;

    public List<Image> energyImage = new List<Image>();
    public List<Image> hpImage = new List<Image>();

    Animator animator;

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
        
    }

    // Update is called once per frame
    void Update()
    {
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
    public void UseAnimation()
    {
    }
}
