using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ItemScriptableObject;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private ItemScriptableObject item;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private GameObject insufficientFundsMessage;
    private UpgradeAttributes currentAttributes;

    private void OnEnable()
    {
        LoadShopDetails();
    }

    public void LoadShopDetails()
    {
        if (item.type == ItemType.Upgrade)
        {
            int level = 1;
            if(ItemInventoryManager.Instance.items.ContainsKey(item))
            {
                level = ItemInventoryManager.Instance.items[item] + 1;
            }
            bool validAttributes = UpgradeEffects.GetUpgradeAttributes(item, level, out currentAttributes);
            if(validAttributes)
            {
                titleText.text = item.title;
                levelText.text = "Level " + currentAttributes.level.ToString();
                descriptionText.text = item.description;
                costText.text = "Cost: " + currentAttributes.cost.ToString();
                insufficientFundsMessage.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void BuyItem()
    {
        if (item.type == ItemType.Upgrade)
        {
            if(ItemInventoryManager.Instance.RemoveItemFromInventory(ItemInventoryManager.Instance.coinObject, currentAttributes.cost))
            {
                ItemInventoryManager.Instance.AddItemToInventory(item, 1);
                insufficientFundsMessage.SetActive(false);
                LoadShopDetails();
            }
            else
            {
                insufficientFundsMessage.SetActive(true);
            }
        }
    }
}
