using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryProgressionManager : MonoBehaviour
{
    private Dictionary<StoryPoint, bool> storyCheckpoints;
    public static StoryProgressionManager Instance;

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
        storyCheckpoints = new Dictionary<StoryPoint, bool>();
    }

    public void SetCheckpoint(StoryPoint point, bool reached)
    {
        if (storyCheckpoints.ContainsKey(point))
        {
            storyCheckpoints[point] = reached;
        }
        else
        {
            storyCheckpoints.Add(point, reached);
        }
    }

    public bool GetCheckpoint(StoryPoint point)
    {
        if(point == StoryPoint.None)
        {
            return true;
        }
        if(storyCheckpoints.ContainsKey(point))
        {
            return storyCheckpoints[point];
        }
        else
        {
            return false;
        }
    }

    public void ResetCheckpoints()
    {
        storyCheckpoints = new Dictionary<StoryPoint, bool>();
    }

    [Serializable]
    public enum StoryPoint
    {
        None,
        Level1Beaten,
        Level2Beaten
    }
}