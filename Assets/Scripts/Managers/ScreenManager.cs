using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class ScreenManager : MonoBehaviour
{
    public bool loadChildrenButton;
    public bool validateSpawnsButton;
    public bool blockUntilWavesClear = true;
    public List<Spawn> spawns;
    public List<Wave> waves;
    public string tagToSpawn = "Enemy";

    [SerializeField] private GameObject spawningEffectPrefab;

    private int activeSpawns;
    private int activeWaveIndex;

    public UnityEvent onWavesCleared;

    private void OnValidate()
    {
        if(loadChildrenButton)
        {
            LoadChildrenIntoSpawns();
            loadChildrenButton = false;
        }
        if(validateSpawnsButton)
        {
            ValidateSpawns();
            validateSpawnsButton = false;
        }
        PopulateWaves();
    }

    private void Start()
    {
        PrepareScreen();
    }

    public void StartScreen()
    {
        PrepareWaveStart(0);
        if(blockUntilWavesClear)
        {

        }
    }

    public void SpawnDefeated()
    {
        activeSpawns--;
        if(activeSpawns < 0)
        {
            Debug.LogWarning(gameObject.name + "'s ScreenManager has an activeSpawn count of " + activeSpawns + "!");
        }
        // if the next wave starts when a number of spawns remain, check to see if we should start it yet
        if (waves.Count > activeWaveIndex + 1 && waves[activeWaveIndex + 1].type == WaveType.StartWhenNumberOfSpawnsRemain)
        {
            if (activeSpawns <= waves[activeWaveIndex + 1].value)
            {
                StartWave(activeWaveIndex + 1);
            }
        }
        else if (waves.Count <= activeWaveIndex + 1)
        {
            if (activeSpawns <= 0)
            {
                WavesClear();
            }
        }
    }

    private void PrepareScreen()
    {
        // hide spawns
        foreach (Spawn spawn in spawns)
        {
            spawn.toSpawn.SetActive(false);
        }
        activeSpawns = 0;
        AssignSpawnsToWaves();
    }

    private void WavesClear()
    {
        if(onWavesCleared != null)
        {
            onWavesCleared.Invoke();
        }
    }

    private void LoadChildrenIntoSpawns()
    {
        if (spawns == null)
        {
            spawns = new List<Spawn>();
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.tag == tagToSpawn && spawns.FindAll(x => x.toSpawn == child).Count == 0)
            {
                spawns.Add(new Spawn() { toSpawn = transform.GetChild(i).gameObject, wave = 1, instantiateNew = false });
            }
        }
    }

    private void ValidateSpawns()
    {
        int i = 0;
        while (i < spawns.Count)
        {
            if(spawns[i].toSpawn == null || spawns[i].toSpawn.transform.parent != transform || spawns[i].toSpawn.tag != tagToSpawn)
            {
                spawns.Remove(spawns[i]);
            }
            else
            {
                i++;
            }
        }
    }

    private void PopulateWaves()
    {
        if(spawns == null)
        {
            return;
        }
        int highestSpawnWave = 0;
        foreach (Spawn spawn in spawns)
        {
            if (spawn.wave > highestSpawnWave)
            {
                highestSpawnWave = spawn.wave;
            }
        }
        if (highestSpawnWave == waves.Count)
        {
            // all good
        }
        else if (highestSpawnWave < waves.Count)
        {
            // too many waves
            while (highestSpawnWave < waves.Count)
            {
                waves.RemoveAt(waves.Count - 1);
            }
        }
        else if (highestSpawnWave > waves.Count)
        {
            // not enough waves
            while (highestSpawnWave > waves.Count)
            {
                waves.Add(new Wave() { number = waves.Count + 1, type = WaveType.StartImmediately, value = 0, spawns = null });
            }
        }
    }

    private void AssignSpawnsToWaves()
    {
        // PopulateWaves() should have already been called automatically, so we have the waves we need
        for (int i = 0; i < waves.Count; i++)
        {
            waves[i].spawns = new List<Spawn>();
        }
        foreach (Spawn spawn in spawns)
        {
            waves[spawn.wave - 1].spawns.Add(spawn);
        }
    }

    private void PrepareWaveStart(int waveIndex) // make sure to not call this method twice for any given wave!!!
    {
        if(waves.Count <= waveIndex) // we have no more waves
        {
            if (activeSpawns <= 0)
            {
                WavesClear();
            }
            return;
        }
        if (waves[waveIndex].type == WaveType.StartImmediately)
        {
            StartWave(waveIndex);
        }
        else if (waves[waveIndex].type == WaveType.WaitForSeconds)
        {
            StartCoroutine("StartWaveAfterSeconds", waves[waveIndex].value);
        }
        else if (waves[waveIndex].type == WaveType.StartWhenNumberOfSpawnsRemain)
        {
            if (activeSpawns <= waves[waveIndex].value)
            {
                StartWave(waveIndex);
            }
        }
    }

    private void StartWave(int waveIndex)
    {
        activeWaveIndex = waveIndex;
        foreach (Spawn spawn in waves[waveIndex].spawns)
        {
            if (spawn.instantiateNew)
            {
                Instantiate(spawn.toSpawn, transform.position, Quaternion.identity);
            }
            else
            {
                spawn.toSpawn.SetActive(true);
                if(spawningEffectPrefab != null)
                {
                    GameObject spawnEffect = Instantiate(spawningEffectPrefab, spawn.toSpawn.transform);
                    spawnEffect.transform.parent = null;
                }
            }
            activeSpawns++;
        }
        PrepareWaveStart(waveIndex + 1);
    }

    private IEnumerable StartWaveAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        StartWave(activeWaveIndex + 1);
    }

    [Serializable]
    public struct Spawn
    {
        public GameObject toSpawn;
        public int wave;
        public bool instantiateNew;
    }

    [Serializable]
    public class Wave
    {
        public int number;
        public WaveType type;
        [InspectorName("Number of Seconds or Spawns")]
        public int value;
        [HideInInspector]
        public List<Spawn> spawns { get; set; }
    }

    public enum WaveType
    {
        StartImmediately,
        WaitForSeconds,
        StartWhenNumberOfSpawnsRemain
    }
}
