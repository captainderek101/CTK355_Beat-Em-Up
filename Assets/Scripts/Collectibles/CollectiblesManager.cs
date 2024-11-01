using UnityEngine;
using TMPro;
 
public class CollectiblesManager : MonoBehaviour
{
    public TextMeshProUGUI gemUI;
    [SerializeField] int numGemsCollected = 0;
 
    // Example of another collectible type
    //public TextMeshProUGUI diamondUI;
    //int numDiamondsCollected = 0;
 
    private void OnEnable()
    {
        Gem.OnGemCollected += GemCollected;
        Gem5.OnGem5Collected += Gem5Collected;
    }
 
    private void OnDisable()
    {
        Gem.OnGemCollected -= GemCollected;
        Gem5.OnGem5Collected -= Gem5Collected;
    }
 
    private void GemCollected()
    {
        numGemsCollected++;
        gemUI.text = numGemsCollected.ToString();
    }
 
    private void Gem5Collected()
    {
        numGemsCollected = numGemsCollected + 5;
        gemUI.text = numGemsCollected.ToString();
    }
}