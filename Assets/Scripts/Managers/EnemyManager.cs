using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    [SerializeField] private int aggroLimitForLevel = 5;
    [SerializeField] private int currentAggroCount;
    private List<AggroPriority> aggroVirgins;
    private List<AggroPriority> aggroChads;
    [SerializeField] private float aggroPriorityTTL = 1f;

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
        aggroVirgins = new List<AggroPriority>();
        aggroChads = new List<AggroPriority>();
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

    public void RecordEnemyDeath()
    {
        foreach (GameObject playerObject in GameManager.Instance.playerObjects)
        {
            playerObject.GetComponent<PlayerAttackController>().ChargeAbility(1);
        }
    }

    public bool CanIContinue(GameObject me, GameObject them)
    {
        if (them.TryGetComponent(out EnemyAggroController aggroController) && !aggroController.aggroed)
        {
            return true;
        }
        bool meVirgin = aggroVirgins.Where(x => x.enemy == me).Count() > 0;
        bool themVirgin = aggroVirgins.Where(x => x.enemy == them).Count() > 0;
        if (meVirgin && !themVirgin)
        {
            return false;
        }
        else if(!meVirgin && themVirgin)
        {
            return true;
        }
        else
        {
            if (UnityEngine.Random.value < 0.5f)
            {
                aggroChads.Add(new AggroPriority() { enemy = me, priorityTimeToLive = aggroPriorityTTL });
                if(meVirgin)
                {
                    aggroVirgins.Remove(aggroVirgins.Find(x => x.enemy == me));
                }
                else
                {
                    aggroVirgins.Add(new AggroPriority() { enemy = them, priorityTimeToLive = aggroPriorityTTL });
                }
                return true;
            }
            else
            {
                aggroChads.Add(new AggroPriority() { enemy = them, priorityTimeToLive = aggroPriorityTTL });
                if (themVirgin)
                {
                    aggroVirgins.Remove(aggroVirgins.Find(x => x.enemy == them));
                }
                else
                {
                    aggroVirgins.Add(new AggroPriority() { enemy = me, priorityTimeToLive = aggroPriorityTTL });
                }
                return false;
            }
        }
    }

    private void FixedUpdate()
    {
        int i = 0;
        while (i < aggroVirgins.Count)
        {
            float newTTL = aggroVirgins[i].priorityTimeToLive - Time.fixedDeltaTime;
            if (newTTL < 0.01)
            {
                aggroVirgins.RemoveAt(i);
            }
            else
            {
                aggroVirgins[i].priorityTimeToLive = newTTL;
                i++;
            }
        }
        i = 0;
        while (i < aggroChads.Count)
        {
            float newTTL = aggroChads[i].priorityTimeToLive - Time.fixedDeltaTime;
            if (newTTL < 0.01)
            {
                aggroChads.RemoveAt(i);
            }
            else
            {
                aggroChads[i].priorityTimeToLive = newTTL;
                i++;
            }
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    [Serializable]
    public class AggroPriority
    {
        public GameObject enemy;
        public float priorityTimeToLive;
    }
}
