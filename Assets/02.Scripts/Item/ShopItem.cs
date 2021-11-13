using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    private bool inShop;
    [SerializeField] private TextMeshPro itmeText;
    [SerializeField] private GameObject sigh;
    [SerializeField] private GameObject itemName;
    public string itemManual;
    
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
        ItemName();
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
            Debug.Log("아이템 구입");
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
    private void ItemName()
    {
        itmeText.text = itemManual + "\n" + itemPrice + "G";
        float x = itmeText.preferredWidth;
        float y = itmeText.preferredHeight / 0.5f;
        x = (x > 2.5f) ? 2.5f : x + 0.5f;
        Debug.Log(itmeText.preferredHeight + "\n" + y);
        sigh.transform.localScale = new Vector3(x, itmeText.preferredHeight + y*0.5f);
		itemName.transform.position += new Vector3(0, y * 0.5f);
	}
}
