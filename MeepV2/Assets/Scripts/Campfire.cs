using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    [Header("Campfire Settings")]
    public ParticleSystem fireParticles;
    public AudioSource fireSound; 
    public string logTag = "Log"; 
    public bool isLit = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(logTag) && !isLit)
        {
            LightFire();
        }
    }

    void LightFire()
    {
        isLit = true;
        fireParticles.Play(); 

        if (fireSound != null)
        {
            fireSound.Play(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(logTag) && isLit)
        {
            
          
            
           
        }
    }
}
