using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TextPreprocessor : MonoBehaviour
{
    public static TextPreprocessor Instance;
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
    }

    public string PreprocessText(string input)
    {
        while (input.Contains("{control:"))
        {
            int substringStart = input.IndexOf("{control:");
            int substringLength = input.IndexOf("}") - substringStart;
            string controlName = input.Substring(substringStart + "{control:".Length, substringLength - "{control:".Length);
            InputAction action = PlayerInputController.Instance.players[0].actions.FindAction(controlName);
            string keybindName = action.controls[0].displayName;
            if (action.expectedControlType == "Vector2")
            {
                for(int i = 1; i < action.controls.Count; i++)
                {
                    keybindName += action.controls[i].displayName;
                }
            }
            input = input.Replace(input.Substring(substringStart, substringLength + 1), keybindName);
        }
        return input;
    }
}
