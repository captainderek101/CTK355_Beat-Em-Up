using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : Menu
{
    [SerializeField] private GameObject settingsMenu;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
    }
}
