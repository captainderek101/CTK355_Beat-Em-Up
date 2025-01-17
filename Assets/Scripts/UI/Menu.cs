using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Menu : MonoBehaviour
{
    protected Animator animator;
    protected const string animatorShowBool = "Show";

    public void CloseMenu()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        GameManager.Instance.EnableOrDisablePlayer(true);
        animator.SetBool(animatorShowBool, false);
    }
}
