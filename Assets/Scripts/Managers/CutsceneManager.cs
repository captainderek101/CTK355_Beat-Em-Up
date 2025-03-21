using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;
    [SerializeField] private DialogueTrigger[] dialogues;

    private Action<CallbackContext> playNext_Handler;
    private Action<CallbackContext> prerequisite_Handler;

    private int currentDialogueIndex = 0;
    private int prerequisitesMet = 0;
    private int prerequisitesNeeded = 0;

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
        playNext_Handler += (e) =>
        {
            PlayNextDialogue();
        };
        prerequisite_Handler += (e) =>
        {
            PrerequisiteMet();
        };
    }

    public void StartCutscene()
    {
        PlayNextDialogue();
    }

    private void PlayNextDialogue()
    {
        if (DialogueManager.Instance.isDialogueActive)
        {
            DialogueManager.Instance.DisplayNextDialogueLine();
        }
        dialogues[currentDialogueIndex].TriggerDialogue();
        currentDialogueIndex++;
        WaitForAction();
    }

    private void WaitForAction()
    {
        switch(currentDialogueIndex)
        {
            case 1:
                foreach (PlayerInput player in PlayerInputController.Instance.players)
                {
                    player.actions.FindAction("Move").performed += playNext_Handler;
                }
                break;
            case 2:
                foreach (PlayerInput player in PlayerInputController.Instance.players)
                {
                    player.actions.FindAction("Move").performed -= playNext_Handler;
                    player.actions.FindAction("Dodgeroll").performed += playNext_Handler;
                }
                break;
            case 3:
                prerequisitesMet = 0;
                prerequisitesNeeded = 2;
                foreach (PlayerInput player in PlayerInputController.Instance.players)
                {
                    player.actions.FindAction("Dodgeroll").performed -= playNext_Handler;
                    player.actions.FindAction("LightAttack").performed += prerequisite_Handler;
                    player.actions.FindAction("StrongAttack").performed += prerequisite_Handler;
                }
                break;
            case 4:
                foreach (PlayerInput player in PlayerInputController.Instance.players)
                {
                    player.actions.FindAction("LightAttack").performed -= prerequisite_Handler;
                    player.actions.FindAction("StrongAttack").performed -= prerequisite_Handler;
                }
                break;
            default:
                Debug.LogWarning(nameof(CutsceneManager) + " attached to " + gameObject.name + " had WaitForAction()'s switch statement fall through to default!");
                break;
        }
    }

    private bool PrerequisiteMet()
    {
        prerequisitesMet++;
        if(prerequisitesMet >= prerequisitesNeeded)
        {
            prerequisitesMet = 0;
            PlayNextDialogue();
            return true;
        }
        else
        {
            return false;
        }
    }

}
