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
    public PlayerInput[] players;
    public UnityAction<string> bindingChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            ResetInputActions();
        }
        else
        {
            //Debug.Log("action map set to " + player.defaultActionMap);
            foreach (PlayerInput player in Instance.players)
            {
                player.SwitchCurrentActionMap(players[0].defaultActionMap);
            }
            Destroy(this);
        }
    }

    public void ResetInputActions()
    {
        foreach (PlayerInput player in players)
        {
            player.actions.FindAction("Pause").started += (e) =>
            {
                player.SwitchCurrentActionMap("UI");
                if (UIManager.Instance.pauseEvent != null)
                    UIManager.Instance.pauseEvent.Invoke();
            };
        }
    }

    public void ChangeInputBinding(InputAction action, string newPath)
    {
        action.ChangeBindingWithGroup(players[0].currentControlScheme).WithPath(newPath);
        bindingChanged.Invoke(action.name);
    }
}
