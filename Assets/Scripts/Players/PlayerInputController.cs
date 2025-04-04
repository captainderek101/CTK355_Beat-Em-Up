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
    public static bool player2Enabled = false;
    public List<PlayerInput> players;
    public UnityAction<string> bindingChanged;

    [SerializeField] private GameObject player2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if(player2Enabled)
            {
                AddPlayer2();
                UIManager.Instance.SetPlayerActionMap();
            }
            else
            {
                ResetPlayerInputs();
            }
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

    public void ChangeInputBinding(InputAction action, string newPath)
    {
        action.ChangeBindingWithGroup(players[0].currentControlScheme).WithPath(newPath);
        bindingChanged.Invoke(action.name);
    }

    public void AddPlayer2()
    {
        player2Enabled = true;
        player2.SetActive(true);
        player2.GetComponent<EntityUIManager>().ShowHealthBar(true);
        players.Add(player2.GetComponent<PlayerInput>());
        ResetPlayerInputs();
        GameManager.Instance.LoadPlayer();
        Transform player1 = null;
        for (int i = 0; i < GameManager.Instance.playerObjects.Length; i++)
        {
            player1 = GameManager.Instance.playerObjects[i].transform;
            if (player1 != player2.transform)
            {
                break;
            }
        }
        player2.GetComponent<Rigidbody>().MovePosition(player1.position + Vector3.left);
    }

    public void RemovePlayer2()
    {
        player2Enabled = false;
        player2.GetComponent<EntityUIManager>().ShowHealthBar(false);
        player2.SetActive(false);
        players.Remove(player2.GetComponent<PlayerInput>());
        ResetPlayerInputs();
        GameManager.Instance.LoadPlayer();
    }

    public void ResetPlayerInputs()
    {
        foreach (PlayerInput player in players)
        {
            player.actions.FindAction("Pause").started -= (e) =>
            {
                if (UIManager.Instance.paused == false)
                {
                    UIManager.Instance.paused = true;
                    if (UIManager.Instance.pauseEvent != null)
                        UIManager.Instance.pauseEvent.Invoke();
                }
            };
            player.actions.FindAction("Unpause").started -= (e) =>
            {
                if (UIManager.Instance.paused == true)
                {
                    UIManager.Instance.paused = false;
                    if (UIManager.Instance.pauseExitEvent != null)
                        UIManager.Instance.pauseExitEvent.Invoke();
                }
            };
        }
        foreach (PlayerInput player in players)
        {
            player.actions.FindAction("Pause").started += (e) =>
            {
                if (UIManager.Instance.paused == false)
                {
                    UIManager.Instance.paused = true;
                    if (UIManager.Instance.pauseEvent != null)
                        UIManager.Instance.pauseEvent.Invoke();
                }
            };
            player.actions.FindAction("Unpause").started += (e) =>
            {
                if (UIManager.Instance.paused == true)
                {
                    UIManager.Instance.paused = false;
                    if (UIManager.Instance.pauseExitEvent != null)
                        UIManager.Instance.pauseExitEvent.Invoke();
                }
            };
        }
    }
}
