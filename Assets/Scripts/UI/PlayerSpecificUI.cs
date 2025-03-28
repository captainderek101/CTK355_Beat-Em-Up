using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecificUI : MonoBehaviour
{
    [SerializeField] private GameObject managedObject;
    [SerializeField] private bool hideWhenPlayer2Active;
    [SerializeField] private bool hideWhenPlayer2Disabled;
    private void OnEnable()
    {
        if (hideWhenPlayer2Active)
        {
            managedObject.SetActive(!PlayerInputController.player2Enabled);
        }
        else if (hideWhenPlayer2Disabled)
        {
            managedObject.SetActive(PlayerInputController.player2Enabled);
        }
    }
}
