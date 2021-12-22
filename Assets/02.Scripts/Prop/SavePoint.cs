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

	public GameObject damageText;
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
		if (player.damage == 0f)
		{
			player.damage = 1f;
		}

		player.hp = player.maxHp;
		UIManager.instance.StatUpdate();

		GameManager.instance.savePointIndex = index;
		save = true;
		sprite.sprite = image;
		PlayerPrefs.SetFloat("damage", player.damage);
		PlayerPrefs.SetInt("maxEnergy", player.maxEnergy);
		PlayerPrefs.SetInt("maxHealth", player.maxHp);
		PlayerPrefs.SetInt("money", player.money);
		PlayerPrefs.SetInt("SavePoint", GameManager.instance.savePointIndex);

		GameObject text = Instantiate(damageText, transform.position + new Vector3(0, 0, -1), Quaternion.identity);
		text.GetComponent<DamageText>().text.text = "ภ๚ภๅ ตส!";
	}
}