using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : Menu
{
    private void Start()
    {
        if (GameManager.Instance.inFirstLoadedScene == false)
        {
            gameObject.SetActive(false);
        }
        else
        {
            GameManager.Instance.EnableOrDisablePlayer(false);
            animator = GetComponent<Animator>();
        }
    }
}
