using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPoint : MonoBehaviour
{
    private void Update()
    {
        Vector3 sum = Vector3.zero;
        foreach(GameObject player in GameManager.Instance.playerObjects)
        {
            if(player == null)
            {
                return;
            }
            sum += player.transform.position;
        }
        sum /= GameManager.Instance.playerObjects.Length;
        transform.position = sum;
    }
}
