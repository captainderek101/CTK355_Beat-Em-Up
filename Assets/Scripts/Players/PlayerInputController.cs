using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController Instance;
    [HideInInspector] public PlayerInputs inputActions;
    [HideInInspector] public PlayerInput player;


    private void Awake()
    {
        TryGetComponent(out player);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(player);
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(player);
            Destroy(this);
        }
        ResetInputActions();
    }

    private void Start()
    {
    }

    public void ResetInputActions()
    {
        inputActions = new PlayerInputs();
        inputActions.Player.Enable();
        player.actions.FindAction("Pause").started += (e) =>
        {
            player.SwitchCurrentActionMap("UI");
            if (UIManager.Instance.pauseEvent != null)
                UIManager.Instance.pauseEvent.Invoke();
        };
        //inputActions.Player.Pause.started += (e) =>
        //{
        //    player.SwitchCurrentActionMap("UI");
        //    if (UIManager.Instance.pauseEvent != null)
        //        UIManager.Instance.pauseEvent.Invoke();
        //};
    }

    //public void ChangeInputBinding(string nameOrId, string path)
    //{
    //    inputActions.FindAction(nameOrId).ChangeBindingWithGroup(player.currentControlScheme).WithPath(path);
    //}
}
