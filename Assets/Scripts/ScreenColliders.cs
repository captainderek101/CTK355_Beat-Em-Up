using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ScreenColliders : MonoBehaviour
{
    private Collider[] colliders;
    private Mesh mesh;
    private MeshCollider screenCollider;

    private void Start()
    {
        colliders = GetComponents<Collider>();
        mesh = GetComponent<MeshFilter>().mesh;
    }

    public void TurnOnColliders()
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
        mesh.triangles = mesh.triangles.Reverse().ToArray();
        screenCollider = gameObject.AddComponent<MeshCollider>();
    }

    public void TurnOffColliders()
    {
        if(screenCollider != null)
        {
            Destroy(screenCollider);
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }
    }
}
