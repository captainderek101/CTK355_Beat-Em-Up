using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UncoupleFromParent : MonoBehaviour
{
    private void Start()
    {
        transform.parent = null;


        // test

        //PlayerInput input = GetComponent<PlayerInput>();
        //PlayerInput player1input = GetComponent<PlayerInput>();
    }
}
