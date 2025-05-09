using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public static class UpgradeEffects
{
    public static bool ApplyUpgradeEffect(ItemScriptableObject upgrade, int level)
    {
        IEnumerable<ItemScriptableObject.UpgradeAttributes> searchedLevels = upgrade.upgradeAttributes.Where(x => x.level == level);
        if (!searchedLevels.Any())
        {
            return false; // could not find upgrade level
        }
        float upgradeValue = searchedLevels.First().value;
        switch (upgrade.title)
        {
            case "Taco Truck":
                PlayerStatManager.Instance.currentMaxHealthMultiplier = upgradeValue;
                PlayerPrefs.SetInt("TacoTruck", level);
                break;
            case "Hot Dog Stand":
                PlayerStatManager.Instance.currentAbilityChargeLimit = Mathf.RoundToInt(upgradeValue);
                PlayerPrefs.SetInt("HotDogTruck", level);
                break;
            case "Wing Truck":
                PlayerStatManager.Instance.currentDamageMultiplier = upgradeValue;
                PlayerPrefs.SetInt("WingTruck", level);
                break;
            case "Ice Cream Truck":
                PlayerStatManager.Instance.currentMoveSpeedMultiplier = upgradeValue;
                PlayerPrefs.SetInt("IceCreamTruck", level);
                break;
            default: // upgrade is not programmed here
                return false;
        }
        foreach (GameObject playerObject in GameManager.Instance.playerObjects)
        {
            PlayerStatManager.Instance.ApplyPlayerStats(playerObject); // apply new player stats
        }
        return true;
    }

    public static bool GetUpgradeAttributes(ItemScriptableObject upgrade, int level, out ItemScriptableObject.UpgradeAttributes levelAttributes)
    {
        IEnumerable<ItemScriptableObject.UpgradeAttributes> searchedLevels = upgrade.upgradeAttributes.Where(x => x.level == level);
        if (!searchedLevels.Any())
        {
            levelAttributes = new ItemScriptableObject.UpgradeAttributes();
            return false; // could not find upgrade level
        }
        levelAttributes = searchedLevels.First();
        return true;
    }
}
