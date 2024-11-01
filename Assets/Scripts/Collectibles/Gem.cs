using System;
using UnityEngine;
 
public class Gem : MonoBehaviour, ICollectible
{
    public static event Action OnGemCollected;
 
    // Example of using particles
    //public ParticleSystem collectParticle;
 
    public void Collect()
    {
        OnGemCollected?.Invoke();
 
        // This is where you can play sounds or spawn particles
        //Instantiate(collectParticle, transform.position, Quaternion.identity);
 
        Debug.Log("Gem collected!");
        Destroy(gameObject);
    }
}