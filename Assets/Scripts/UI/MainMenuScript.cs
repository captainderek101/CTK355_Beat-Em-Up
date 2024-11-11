using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MainMenuScript : MonoBehaviour
{
    private Animator animator;
    private const string animatorShowBool = "Show";

    private void Start()
    {
        if (GameManager.Instance.mainMenuDismissed)
        {
            gameObject.SetActive(false);
        }
        else
        {
            GameManager.Instance.EnableOrDisablePlayer(false);
            animator = GetComponent<Animator>();
        }
    }

    public void StartGame()
    {
        GameManager.Instance.EnableOrDisablePlayer(true);
        animator.SetBool(animatorShowBool, false);
        GameManager.Instance.mainMenuDismissed = true;
    }
}
