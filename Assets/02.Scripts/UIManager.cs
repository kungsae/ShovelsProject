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

            energyImage[i].sprite = hpSprite[2];
            if (i<player.maxEnergy/2)
            {
                energyImage[i].gameObject.SetActive(true);
            }
        }
        List<Image> images = energyImage.FindAll((x) => x.gameObject.activeSelf);


		for (int i = 0; i < images.Count*2; i++)
		{
            if (i % 2 == 0 && i != 0)
            {
                
            }
		}
        //for (int i = 0; i < player.energy; i++)
        //{
        //    energyImage[i].sprite = hpSprite[0];
        //    if (i == player.energy - 1)
        //    {
        //        if (i % 2 == 0)
        //            energyImage[i].sprite = hpSprite[1];
        //        else
        //            energyImage[i].sprite = hpSprite[0];
        //    }
        //}
    }
    public void UseAnimation()
    {
        for (int i = 0; i < player.energy; i++)
        {
            if (i % 2 == 0)
            {
                if (i == player.energy - 1) ;

            }
        }
    }
}
