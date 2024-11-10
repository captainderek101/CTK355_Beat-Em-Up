using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public void ResumeGame()
    {
        GameManager.Instance.EnableOrDisablePlayer(true);
        gameObject.SetActive(false);
    }
}
