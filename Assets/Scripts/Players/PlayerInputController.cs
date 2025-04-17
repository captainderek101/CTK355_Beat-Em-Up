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
    [SerializeField] private PlayerInput player2Input;
    [HideInInspector] public GameObject deathScreen;

    private GameObject deadPlayer;

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
        player2Input.gameObject.SetActive(true);
        player2.GetComponent<EntityUIManager>().ShowHealthBar(true);
        players.Add(player2Input);
        ResetPlayerInputs();

        PlayerInput player1Input = players[0];
        if (player1Input.currentControlScheme != "Keyboard&Mouse")
        {
            player2Input.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);
        }
        else
        {
            player2Input.SwitchCurrentControlScheme("Gamepad", Gamepad.current);
        }
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
        player2Input.gameObject.SetActive(false);
        players.Remove(player2Input);
        ResetPlayerInputs();
        GameManager.Instance.LoadPlayer();
    }

    public void ProcessPlayerDeath(GameObject player)
    {
        if (player2Enabled && deadPlayer == null)
        {
            deadPlayer = player;
        }
        else
        {
            deathScreen.SetActive(true);
        }
    }

    public void RespawnDeadPlayers()
    {
        if(deadPlayer != null)
        {
            deadPlayer.SetActive(true);
            if(deadPlayer.TryGetComponent(out Health health))
            {
                health.dead = false;
                health.ChangeHealth(health.maxHealth);
            }
            if (deadPlayer.TryGetComponent(out Animator animator))
            {
                animator.SetTrigger("Reset");
            }
            if (deadPlayer.TryGetComponent(out Rigidbody rigidbody))
            {
                Transform livingPlayer = null;
                for (int i = 0; i < GameManager.Instance.playerObjects.Length; i++)
                {
                    livingPlayer = GameManager.Instance.playerObjects[i].transform;
                    if (livingPlayer != deadPlayer.transform)
                    {
                        break;
                    }
                }
                if(livingPlayer != null)
                {
                    rigidbody.MovePosition(livingPlayer.position + Vector3.left);
                }
            }
            deadPlayer = null;
        }
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
