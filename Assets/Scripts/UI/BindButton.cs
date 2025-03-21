using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class BindButton : MonoBehaviour
{
    [SerializeField] private TMP_Text bindingText;
    [SerializeField] private InputActionReference input;
    private InputAction action;

    private void FixedUpdate()
    {
        //Debug.Log(inputs.devices.HasValue ? inputs.devices.Value : "no devices");
    }

    private void OnEnable()
    {
        if(action == null)
        {
            action = PlayerInputController.Instance.players[0].actions.FindAction(input.action.id.ToString());
            //action = PlayerInputController.Instance.inputActions.FindAction(input.action.id.ToString());
        }
        UpdateBindingText();

        foreach (PlayerInput player in PlayerInputController.Instance.players)
        {
            player.onControlsChanged += (input) =>
            {
                UpdateBindingText();
            };
        }
    }

    private void OnDisable()
    {
        foreach (PlayerInput player in PlayerInputController.Instance.players)
        {
            player.onControlsChanged -= (input) =>
            {
                UpdateBindingText();
            };
        }
    }

    private void UpdateBindingText()
    {
        //Debug.Log(PlayerInputController.Instance.player.currentControlScheme);
        //bindingText.text = input.action.bindings[0].path;
        //Debug.Log(input.action.GetBindingForControl(input.action.controls[0]).ToString());

        //Debug.Log(input.action.controls.Count);
        //bindingText.text = input.action.controls[0].displayName;

        //Debug.Log(action.controls.Count);
        bindingText.text = action.controls[0].displayName;
    }

    public void BindNewInput()
    {
        bindingText.text = "Waiting...";
        InputSystem.onAnyButtonPress.CallOnce(input => {
            //Debug.Log(action.controls[0].path);
            //Debug.Log(input.path);
            PlayerInputController.Instance.ChangeInputBinding(action, input.path);
            //action.ChangeBindingWithGroup(PlayerInputController.Instance.player.currentControlScheme).WithPath(input.path);
            UpdateBindingText();
        });
    }

    //private IEnumerator WaitForNewInput()
    //{
    //    if(PlayerInputController.Instance.player.inputIsActive)
    //    {
    //        //PlayerInputController.Instance.player.
    //    }
    //    yield return new WaitForEndOfFrame();
    //}
}
