using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]private PlayerMove player;
    [SerializeField] private Sprite[] hpSprite;

    public List<Image> energyImage = new List<Image>();
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
		for (int i = 0; i < energyImage.Count; i++)
		{
            energyImage[i].gameObject.SetActive(false);
            energyImage[i].sprite = hpSprite[0];
        }
        for (int i = 0; i < player.energy; i++)
        {
            if (i % 2 == 0)
            {
                energyImage[i].gameObject.SetActive(true);
                if (i == player.energy - 1)
                energyImage[i].sprite = hpSprite[1];
            }
        }
    }
}
