using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossFightManager : MonoBehaviour
{
    public List<Phase> phases;
    private Health bossHealth;

    private int activePhaseIndex = -1;
    private bool waitingForHealth = false;
    [SerializeField] private bool waitForReenable = true;

    private void Start()
    {
        TryGetComponent(out bossHealth);
    }

    private void OnDisable()
    {
        waitForReenable = false;
    }

    private void OnEnable()
    {
        if(!waitForReenable)
        {
            StartFight();
        }
    }

    public void StartFight()
    {
        Debug.Log("Starting Boss Fight!");
        PreparePhaseStart(0);
    }

    private void PreparePhaseStart(int phaseIndex) // make sure to not call this method twice for any given wave!!!
    {
        if (phases.Count <= phaseIndex) // we have no more waves
        {
            return;
        }
        if (phases[phaseIndex].type == PhaseType.StartWhenPercentOfHealthRemains)
        {
            if(bossHealth.GetCurrentHealth() / bossHealth.maxHealth < phases[phaseIndex].value + 0.001f)
            {
                StartPhase(phaseIndex);
            }
            else
            {
                waitingForHealth = true;
            }
        }
        else if (phases[phaseIndex].type == PhaseType.WaitForSeconds)
        {
            StartCoroutine("StartPhaseAfterSeconds", phases[phaseIndex].value);
        }
    }

    private void StartPhase(int phaseIndex)
    {
        activePhaseIndex = phaseIndex;
        if(phases[activePhaseIndex].phaseStarted != null)
        {
            phases[activePhaseIndex].phaseStarted.Invoke();
        }
        PreparePhaseStart(phaseIndex + 1);
    }

    private void FixedUpdate()
    {
        if(waitingForHealth)
        {
            if (bossHealth.GetCurrentHealth() / bossHealth.maxHealth < phases[activePhaseIndex + 1].value + 0.001f)
            {
                StartPhase(activePhaseIndex + 1);
                waitingForHealth = false;
            }
        }
    }

    private IEnumerable StartPhaseAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StartPhase(activePhaseIndex + 1);
    }

    [Serializable]
    public class Phase
    {
        public PhaseType type;
        [InspectorName("Health % OR # of seconds")]
        public float value;
        public UnityEvent phaseStarted;
    }

    public enum PhaseType
    {
        StartWhenPercentOfHealthRemains,
        WaitForSeconds
    }
}
