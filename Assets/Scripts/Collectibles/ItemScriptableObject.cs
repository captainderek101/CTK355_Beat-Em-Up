using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item")]
public class ItemScriptableObject : ScriptableObject
{
    public string title;
    public string description;
    public ItemType type;
    public UpgradeAttributes[] upgradeAttributes;
    public enum ItemType
    {
        Currency,
        Upgrade
    }
    [Serializable]
    public struct UpgradeAttributes
    {
        public int level;
        public int cost;
        public float value;
        public UpgradeValueType valueType;
        public enum UpgradeValueType
        {
            Multiplier,
            Flat
        }
    }
}
