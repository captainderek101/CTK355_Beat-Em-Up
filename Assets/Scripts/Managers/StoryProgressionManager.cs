using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryProgressionManager : MonoBehaviour
{
    private Dictionary<StoryPoint, bool> storyCheckpoints;
    public static StoryProgressionManager Instance;

    private List<string> storyPoints = new List<string>(new string[] { "TutorialDialogue", "Level0_Dialogue1", 
        "Level1_Dialogue1", "Level1_DialogueEnd", 
        "Level2_Dialogue1", "Level2_DialogueEnd", 
        "Level3_Dialogue1", "Level3_DialogueScreen1", "Level3_DialogueScreen3", "Level3_DialogueScreen4", 
        "Level3_DialogueScreen5", "Level3_DialogueEnd", "Level3_Credits" });

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
        foreach (int i in Enum.GetValues(typeof(StoryPoint)))
        {
            if (PlayerPrefs.GetInt(((StoryPoint)i).ToString(), 0) == 1)
            {
                SetCheckpoint(((StoryPoint)i), true);
            }
        }
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
        PlayerPrefs.SetInt(point.ToString(), reached ? 1 : 0);
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
        foreach (int i in Enum.GetValues(typeof(StoryPoint)))
        {
            PlayerPrefs.DeleteKey(((StoryPoint)i).ToString());
        }
        foreach (string key in storyPoints)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }

    [Serializable]
    public enum StoryPoint
    {
        None,
        Level1Beaten,
        Level2Beaten,
        Level3Beaten
    }
}