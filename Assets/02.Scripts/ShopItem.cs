using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    private bool inShop;
    [SerializeField] private int itemPrice = 100;
    private enum ShopItemType
    {
        Potion,
        DamageUp,
        ParryingItem,
        DoubleJump
    }
    [SerializeField]private ShopItemType itemType;
    public GameObject Image;
    public GameObject itemPrefab;
	// Start is called before the first frame update
	private void Awake()
	{

    }
	void Start()
    {

    }

	// Update is called once per frame
	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.X)&& inShop)
        {
            Debug.Log(gameObject.name);
            if (BuyItem(itemPrice))
            {
                Image.SetActive(false);
                this.enabled = false;
            }
                
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inShop = true;
        }
    }
	private void OnTriggerExit2D(Collider2D collision)
	{
        if (collision.gameObject.CompareTag("Player"))
        {
            inShop = false;
        }
    }
    public bool BuyItem(int price)
    {
        if (GameManager.instance.playerScript.money > price)
        {
            GameManager.instance.playerScript.money -= price;
            Debug.Log("������ ����");
            GameObject item = Instantiate(itemPrefab, gameObject.transform.position, Quaternion.identity);
            Item itemScript = item.GetComponent<Item>();

            switch (itemType)
			{
				case ShopItemType.Potion:
                    itemScript.itemType = Item.ItemType.Potion;
                    break;
				case ShopItemType.DamageUp:
                    itemScript.itemType = Item.ItemType.DamageUp;
                    break;
				case ShopItemType.ParryingItem:
                    itemScript.itemType = Item.ItemType.ParryingItem;
                    break;
				case ShopItemType.DoubleJump:
                    itemScript.itemType = Item.ItemType.DoubleJump;
                    break;
				default:
					break;
			}
            itemScript.ItemSet();
            itemScript.SpawnItem();
            return true;
        }
        return false;
    }
}