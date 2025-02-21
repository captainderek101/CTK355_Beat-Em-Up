using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMenu : Menu
{
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ResetStoryProgress()
    {
        StoryProgressionManager.Instance.ResetCheckpoints();
    }

    public void ResetPlayerUpgrades()
    {
        ItemInventoryManager.Instance.RemoveAllItems();
        PlayerStatManager.Instance.ResetPlayerStats();
    }
}
