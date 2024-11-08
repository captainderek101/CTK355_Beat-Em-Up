using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderTrigger : MonoBehaviour
{
    private bool alreadyEntered = false;
    private bool alreadyExited = false;

    public string collisionTag;
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    public UnityEvent onTriggerEnterOneShot;
    public UnityEvent onTriggerExitOneShot;

    private void OnTriggerEnter(Collider collision)
    {
        if (!string.IsNullOrEmpty(collisionTag) && !collision.CompareTag(collisionTag))
            return;

        onTriggerEnter?.Invoke();

        if (!alreadyEntered)
            onTriggerEnterOneShot?.Invoke();

        alreadyEntered = true;
    }

    private void OnTriggerExit(Collider collision)
    {
        if (!string.IsNullOrEmpty(collisionTag) && !collision.CompareTag(collisionTag))
            return;

        onTriggerExit?.Invoke();

        if (!alreadyExited)
            onTriggerExitOneShot?.Invoke();

        alreadyExited = true;
    }
}
