using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    [SerializeField] private int aggroLimitForLevel = 5;
    [SerializeField] private int currentAggroCount;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        currentAggroCount = 0;
    }

    public bool TryAggroEnemy()
    {
        if(currentAggroCount >=  aggroLimitForLevel)
        {
            return false;
        }
        else
        {
            currentAggroCount++;
            return true;
        }
    }

    public bool DeaggroEnemy()
    {
        if (currentAggroCount > 0)
        {
            currentAggroCount--;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
