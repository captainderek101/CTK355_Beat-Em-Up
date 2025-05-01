using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class CutsceneManager : DialogueTrigger
{
    [SerializeField]
    private string dialogueID = "REPLACE THIS";

    public void StartCutscene()
    {
        if (PlayerPrefs.GetInt(dialogueID, 0) == 0)
        {
            TriggerDialogue();
            PlayerPrefs.SetInt(dialogueID, 1);
        }
    }
}
