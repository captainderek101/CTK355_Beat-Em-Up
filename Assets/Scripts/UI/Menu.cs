using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(Animator))]
public class Menu : MonoBehaviour
{
    protected Animator animator;
    protected const string animatorShowBool = "Show";
    [SerializeField] protected GameObject firstSelected;

    private AudioPlayer audioPlayer;
    private const string openAudioName = "openMenu";

    public void CloseMenu()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.SetBool(animatorShowBool, false);
    }

    public void OpenMenu()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.SetBool(animatorShowBool, true);

        if(audioPlayer == null)
        {
            audioPlayer = Camera.main.GetComponent<AudioPlayer>();
        }
        audioPlayer.PlaySound(openAudioName);
    }

    public void LoadScene(string sceneName)
    {
        TransitionManager.Instance.LoadSceneAsync(sceneName);
    }

    public void SetPlayerActionMap()
    {
        //Debug.Log("action map set to Player");
        UIManager.Instance.SetPlayerActionMap();
    }

    public void SetUIActionMap()
    {
        //Debug.Log("action map set to UI");
        UIManager.Instance.SetUIActionMap();
    }

    public void SetFirstSelected()
    {
        if(firstSelected != null)
        {
            foreach (MultiplayerEventSystem eventSystem in FindObjectsOfType<MultiplayerEventSystem>())
            {
                eventSystem.SetSelectedGameObject(firstSelected);
            }
            SetUIActionMap();
        }
    }

    public void SelectObject(GameObject selected)
    {
        foreach (MultiplayerEventSystem eventSystem in FindObjectsOfType<MultiplayerEventSystem>())
        {
            eventSystem.SetSelectedGameObject(selected);
        }
        SetUIActionMap();
    }

    public virtual void OnEnable()
    {
        SetFirstSelected();
        //Debug.Log(PlayerInputController.Instance.player.currentActionMap.name);
    }



    [Serializable]
    public enum ActionMaps
    {
        Player,
        UI
    }
}
