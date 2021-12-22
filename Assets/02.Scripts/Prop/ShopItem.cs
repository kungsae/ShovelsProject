using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : InteractObj
{
    [SerializeField] private TextMeshPro itemText;
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
	void Start()
    {
        ItemName();
    }

	protected override void Interact()
	{
		base.Interact();
        Debug.Log(gameObject.name);
        if (BuyItem(itemPrice))
        {
            Image.SetActive(false);
            this.enabled = false;
        }
    }
	//아이템 구매
	public bool BuyItem(int price)
    {
        if (GameManager.instance.playerScript.money > price)
        {
            GameManager.instance.playerScript.money -= price;
            UIManager.instance.coinUi();
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
    //아이템 정보 (이름,가격)을 펫말로 표시하는 스크립트
    private void ItemName()
    {
        itemText.text = itemManual + "\n" + itemPrice + "G";
        float x = itemText.preferredWidth;
        float y = itemText.preferredHeight / 0.5f;
        x = (x > 2.5f) ? 2.5f : x + 0.5f;
        Debug.Log(itemText.preferredHeight + "\n" + y);
        sigh.transform.localScale = new Vector3(x, itemText.preferredHeight + y*0.5f);
		itemName.transform.position += new Vector3(0, y * 0.5f);
	}
}
