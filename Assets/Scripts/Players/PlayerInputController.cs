using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController Instance;
    public PlayerInputs inputActions;


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
        inputActions = new PlayerInputs();
        inputActions.Player.Enable();
        inputActions.Player.Pause.started += (e) =>
        {
            if (UIManager.Instance.pauseEvent != null)
                UIManager.Instance.pauseEvent.Invoke();
        };
    }

    private void Update()
    {
    }
}
