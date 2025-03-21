using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeybindDisplay : MonoBehaviour
{
    [SerializeField] private string actionName = "";
    private TMP_Text keybindText;
    private InputAction action;
    private void Start()
    {
        TryGetComponent(out keybindText);
        PlayerInputController.Instance.bindingChanged += (binding) =>
        {
            if (binding == actionName)
            {
                UpdateKeybindText();
            }
        };

        foreach (PlayerInput player in PlayerInputController.Instance.players)
        {
            player.onControlsChanged += (input) =>
            {
                UpdateKeybindText();
            };
        }
        UpdateKeybindText();
    }

    private void UpdateKeybindText()
    {
        if(action == null)
        {
            action = PlayerInputController.Instance.players[0].actions.FindAction(actionName);
        }
        if(action.controls.Count > 0)
        {
            keybindText.text = action.controls[0].displayName;
        }
        else
        {
            Debug.LogWarning("Keybind display for " + actionName + " has zero controls!");
        }
    }
}
