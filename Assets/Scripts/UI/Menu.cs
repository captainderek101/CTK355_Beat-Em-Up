using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class Menu : MonoBehaviour
{
    protected Animator animator;
    protected const string animatorShowBool = "Show";
    [SerializeField] protected GameObject firstSelected;

    public void CloseMenu()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        GameManager.Instance.EnableOrDisablePlayer(true);
        animator.SetBool(animatorShowBool, false);
    }

    public void SetPlayerActionMap()
    {
        //Debug.Log("action map set to Player");
        PlayerInputController.Instance.player.SwitchCurrentActionMap("Player");
    }

    public void SetUIActionMap()
    {
        //Debug.Log("action map set to UI");
        PlayerInputController.Instance.player.SwitchCurrentActionMap("UI");
    }

    public void SetFirstSelected()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
        SetUIActionMap();
    }

    private void OnEnable()
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
