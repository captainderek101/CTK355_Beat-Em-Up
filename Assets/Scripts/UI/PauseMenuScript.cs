using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    private Animator animator;
    private const string animatorShowBool = "Show";

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ResumeGame()
    {
        GameManager.Instance.EnableOrDisablePlayer(true);
        animator.SetBool(animatorShowBool, false);
    }
}
