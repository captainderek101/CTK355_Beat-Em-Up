using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, ICollectible
{
    public ItemScriptableObject item;
    public int quantity;
    public void Collect()
    {
        ItemInventoryManager.Instance.AddItemToInventory(item, quantity);
        Destroy(gameObject);
    }
}
