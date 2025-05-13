using Photon.Pun.Demo.Procedural;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public GameObject button;
    public Renderer rend;
    private bool isPressed = false;
    public AudioSource click; 

    [Header("X Y Z")]
    public float X;
    public float y;
    public float z;
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = button.transform.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Finger")
        {
            if (!isPressed)
            {
                click.Play();
                isPressed = true;
                rend.material.color = Color.red;
                button.transform.localPosition = new Vector3(originalPosition.x + X, originalPosition.y + y, originalPosition.z + z);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Finger")
        {
            if (isPressed)
            {
                isPressed = false;
                rend.material.color = Color.white;
                button.transform.localPosition = originalPosition;
            }
        }
    }
}