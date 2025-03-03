using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    [Header("Script by Keo.CS")]
    [Header("No need for credits")]
    public string TargetTag;
    [Header("Realitiv Offset")]
    public Vector3 PushOffset;
    [Header("Prob not best with hitsounds try to outclude it so yeah")]
    public bool UsePushAudio;
    public AudioSource PressSound;
    public AudioSource UnPressSound;
    [Header("Change the material")]
    public bool UsePushMaterial;
    public Material PressedMaterial;
    private Material StartMaterial;
    [Header("Fix Script")]
    public bool FixScript;
    public float CooldownTime;
    private bool CoolDownActive;

    private Vector3 PushTo;
    private Vector3 StartPos;


    private void Start()
    {
        CoolDownActive = false;
        Renderer renderer = GetComponent<Renderer>();
        
        if (renderer != null)
        {
            StartMaterial = renderer.material;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(TargetTag))
        {
            if (!CoolDownActive)
            {
                PushObj();
                StartCooldown();
            }
        }
    }

    public void PushObj()
    {
        if (UsePushAudio)
        {
            PressSound.Play();
        }
        if (UsePushMaterial)
        {
            GetComponent<Renderer>().material = PressedMaterial;
        }
        if (FixScript)
        {
        }
        StartPos = transform.position;
        PushTo = StartPos + PushOffset;
        gameObject.transform.position = PushTo;
    }

    public void BackObj()
    {
        if (UsePushAudio)
        {
            UnPressSound.Play();
        }
        if (UsePushMaterial)
        {
            GetComponent<Renderer>().material = StartMaterial;
        }
        if (FixScript)
        {
        }
        gameObject.transform.position = StartPos;
    }

    public void StartCooldown()
    {
        CoolDownActive = true;
        Invoke("ResetCooldown", CooldownTime);
    }

    private void ResetCooldown()
    {
        CoolDownActive = false;
        BackObj();
    }
}
