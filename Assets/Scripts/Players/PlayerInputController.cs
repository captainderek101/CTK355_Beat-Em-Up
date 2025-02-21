using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController Instance;
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
            //Debug.Log("action map set to " + player.defaultActionMap);
            Instance.player.SwitchCurrentActionMap(player.defaultActionMap);
            Destroy(player);
            Destroy(this);
        }
        ResetInputActions();
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
}
