using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : Menu
{
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public new void CloseMenu()
    {
        UIManager.Instance.paused = false;
        base.CloseMenu();
    }
}
