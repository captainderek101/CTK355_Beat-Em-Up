using System;
using UnityEngine;

public class Gem5 : MonoBehaviour, ICollectible
{
    public static event Action OnGem5Collected;

    // Example of using particles
    //public ParticleSystem collectParticle;

    public void Collect()
    {
        OnGem5Collected?.Invoke();

        // This is where you can play sounds or spawn particles
        //Instantiate(collectParticle, transform.position, Quaternion.identity);

        Debug.Log("Gem5 collected!");
        Destroy(gameObject);
    }
}
