using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ItemScriptableObject.ItemType;
using UnityEngine.SceneManagement;

public class ItemInventoryManager : MonoBehaviour
{
    public static ItemInventoryManager Instance;
    public Dictionary<ItemScriptableObject, int> items;
    public ItemScriptableObject coinObject;
    [SerializeField] private bool DEBUG_add1000Coins;

    private AudioPlayer audioPlayer;
    private const string currencyAudioName = "pickupCoin";
    private const string upgradeAudioName = "purchaseUpgrade";

    [SerializeField] private ItemScriptableObject hotDog;
    [SerializeField] private ItemScriptableObject iceCream;
    [SerializeField] private ItemScriptableObject taco;
    [SerializeField] private ItemScriptableObject wing;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        items = new Dictionary<ItemScriptableObject, int>();

        SceneManager.sceneLoaded += (x, y) =>
        {
            if (items.ContainsKey(coinObject))
            {
                UIManager.Instance.SetCoinUI(items[coinObject]);
            }
        };

        int hotDogLevels = PlayerPrefs.GetInt("HotDogTruck", 0);
        AddItemToInventory(hotDog, hotDogLevels, false);

        int iceCreamLevels = PlayerPrefs.GetInt("IceCreamTruck", 0);
        AddItemToInventory(iceCream, iceCreamLevels, false);

        int tacoLevels = PlayerPrefs.GetInt("TacoTruck", 0);
        AddItemToInventory(taco, tacoLevels, false);

        int wingLevels = PlayerPrefs.GetInt("WingTruck", 0);
        AddItemToInventory(wing, wingLevels, false);
    }


    private void OnValidate()
    {
        if (DEBUG_add1000Coins)
        {
            AddItemToInventory(coinObject, 1000);
            DEBUG_add1000Coins = false;
        }
    }

    /** Adds the specified item quantity to the inventory
     */
    public void AddItemToInventory(ItemScriptableObject item, int quantity, bool playAudio = true)
    {
        IEnumerable<ItemScriptableObject> searchedItems = items.Keys.Where(x => x.name == item.name);
        if (searchedItems.Any())
        {
            var matchingItem = searchedItems.First();
            items[matchingItem] += quantity;
        }
        else
        {
            items.Add(item, quantity);
        }
        audioPlayer = Camera.main.GetComponent<AudioPlayer>();
        if (playAudio)
        {
            switch (item.type)
            {
                case Currency:
                    audioPlayer.PlaySound(currencyAudioName);
                    break;
                case Upgrade:
                    audioPlayer.PlaySound(upgradeAudioName);
                    break;
                default:
                    break;
            }
        }
        UpdateInventoryUI(item);
        UpdateItemEffects(item);
    }

    /** Removes the specified item quantity from the inventory
     *    returns true if the request was completed
     *    returns false if the request could not be completed
     */
    public bool RemoveItemFromInventory(ItemScriptableObject item, int quantity)
    {
        IEnumerable<ItemScriptableObject> searchedItems = items.Keys.Where(x => x.name == item.name);
        if (!searchedItems.Any())
        {
            return false; // removal failed
        }
        var matchingItem = searchedItems.First();
        if (items[matchingItem] < quantity)
        {
            return false; // removal failed
        }
        items[matchingItem] -= quantity;
        UpdateInventoryUI(item);
        UpdateItemEffects(item);
        return true; // we had enough of the item to be removed
    }

    public void RemoveAllItems()
    {
        items = new Dictionary<ItemScriptableObject, int>();
        UIManager.Instance.SetCoinUI(0);
        PlayerPrefs.DeleteKey("HotDogTruck");
        PlayerPrefs.DeleteKey("IceCreamTruck");
        PlayerPrefs.DeleteKey("TacoTruck");
        PlayerPrefs.DeleteKey("WingTruck");
    }

    private void UpdateInventoryUI(ItemScriptableObject item)
    {
        if (item == coinObject)
        {
            UIManager.Instance.SetCoinUI(items[item]);
        }
    }

    private void UpdateItemEffects(ItemScriptableObject item)
    {
        if(item.type == Upgrade)
        {
            UpgradeEffects.ApplyUpgradeEffect(item, items[item]);
        }
    }
}
