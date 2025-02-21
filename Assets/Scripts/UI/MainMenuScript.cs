using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuScript : Menu
{
    private void Start()
    {
        if (GameManager.Instance.inFirstLoadedScene == false)
        {
            gameObject.SetActive(false);
            SetPlayerActionMap();
        }
        else
        {
            animator = GetComponent<Animator>();
            SetFirstSelected();
        }
    }

    public override void OnEnable()
    {
        return;
    }
}
