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
        animator.SetBool(animatorShowBool, false);
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
            EventSystem.current.SetSelectedGameObject(firstSelected);
            SetUIActionMap();
        }
    }

    public void SelectObject(GameObject selected)
    {
        EventSystem.current.SetSelectedGameObject(selected);
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
