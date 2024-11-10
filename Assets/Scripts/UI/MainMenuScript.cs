using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.EnableOrDisablePlayer(false);
    }

    public void StartGame()
    {
        GameManager.Instance.EnableOrDisablePlayer(true);
        gameObject.SetActive(false);
    }
}
