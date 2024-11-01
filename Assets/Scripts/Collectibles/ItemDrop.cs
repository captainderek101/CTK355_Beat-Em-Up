using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{

    private Rigidbody itemRB;
    public float dropForce = 5;

    // Start is called before the first frame update
    void Start()
    {
        itemRB = GetComponent<Rigidbody>();
        itemRB.AddForce(0, dropForce, 0, ForceMode.Impulse);
    }
}
