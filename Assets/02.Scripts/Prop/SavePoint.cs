using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
	public SpriteRenderer sprite;
    public Sprite image;
	public bool save = false;
	PlayerMove player;
	public int index;
	private void Start()
	{
		sprite = GetComponent<SpriteRenderer>();
		player = GameManager.instance.playerScript;
		if(save)
		{
			sprite.sprite = image; 
		}
	}
    public void Save()
    {
		GameManager.instance.savePointIndex = index;
		save = true;
		sprite.sprite = image;
		PlayerPrefs.SetInt("maxEnergy", player.maxEnergy);
		PlayerPrefs.SetInt("maxHealth", player.initHealth);
		PlayerPrefs.SetInt("money", player.money);
		PlayerPrefs.SetInt("SavePoint", GameManager.instance.savePointIndex);
		Debug.Log("¿˙¿Â");
    }
}