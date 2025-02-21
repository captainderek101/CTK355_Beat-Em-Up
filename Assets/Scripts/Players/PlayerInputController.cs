using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController Instance;
    [HideInInspector] public PlayerInput player;
    public UnityAction<string> bindingChanged;

    private void Awake()
    {
        TryGetComponent(out player);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(player);
            DontDestroyOnLoad(this);
            ResetInputActions();
        }
        else
        {
            //Debug.Log("action map set to " + player.defaultActionMap);
            Instance.player.SwitchCurrentActionMap(player.defaultActionMap);
            Destroy(player);
            Destroy(this);
        }
    }

    public void ResetInputActions()
    {
        player.actions.FindAction("Pause").started += (e) =>
        {
            player.SwitchCurrentActionMap("UI");
            if (UIManager.Instance.pauseEvent != null)
                UIManager.Instance.pauseEvent.Invoke();
        };
    }

    public void ChangeInputBinding(InputAction action, string newPath)
    {
        action.ChangeBindingWithGroup(player.currentControlScheme).WithPath(newPath);
        bindingChanged.Invoke(action.name);
    }
}
