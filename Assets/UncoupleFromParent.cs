using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UncoupleFromParent : MonoBehaviour
{
    private void Start()
    {
        transform.parent = null;
    }
}
