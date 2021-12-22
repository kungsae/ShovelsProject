using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : MonoBehaviour
{
	public enum ItemType
	{
		Potion,
		DamageUp,
		ParryingItem,
		DoubleJump
	}
	public ItemType itemType;
	Rigidbody2D rig;
	public SpriteRenderer spriteRenderer;
	public float followSpeed;
	private bool canGet = false;
	public float testPower;

	[SerializeField] private List<Sprite> itemIamge = new List<Sprite>();

	private PlayerMove player;

	private void Awake()
	{
		rig = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		player = GameManager.instance.playerScript;

	}
	private void Start()
	{
	}

	void Update()
    {

		if (Physics2D.OverlapCircle(transform.position, 3, 1 << 7) != null && canGet)
		{
			transform.position = Vector2.MoveTowards(transform.position,GameManager.instance.player.transform.position,followSpeed*Time.deltaTime);
			if (Physics2D.OverlapCircle(transform.position, 0.2f, 1 << 7) != null)
			{
				GetItem();
				Destroy(gameObject);
			}
		}

	}
	private void canGetCoin()
	{
		canGet = true;
	}
	public void ItemSet()
	{
		switch (itemType)
		{
			case ItemType.Potion:
				spriteRenderer.sprite = itemIamge[0];
				break;
			case ItemType.DamageUp:
				spriteRenderer.sprite = itemIamge[1];
				break;
			case ItemType.ParryingItem:
				spriteRenderer.sprite = itemIamge[2];
				break;
			case ItemType.DoubleJump:
				spriteRenderer.sprite = itemIamge[3];
				break;
			default:
				break;
		}
	}
	public void SpawnItem()
	{
		gameObject.SetActive(true);
		rig.AddForce(new Vector2(0, 10) * testPower);
		Invoke("canGetCoin", 1f);
	}
	private void GetItem()
	{
		switch (itemType)
		{
			case ItemType.Potion:
				if(player.hp<player.maxHp)
				player.hp++;
				UIManager.instance.StatUpdate();
				break;
			case ItemType.DamageUp:
				player.damage++;
				break;
			case ItemType.ParryingItem:
				Debug.Log("패링 기능 추가 예정");
				break;
			case ItemType.DoubleJump:
				Debug.Log("더블 점프 기능 추가 예정");
				break;
			default:
				break;
		}
		Debug.Log("A");
	}
}
