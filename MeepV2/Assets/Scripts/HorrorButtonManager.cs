using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorButtonManager : MonoBehaviour
{
    public static HorrorButtonManager instance = null;

    [SerializeField] private GameObject door;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioSource buttonClickSrc;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioSource doorOpenSrc;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private GameObject[] buttons;

    void Start()
    {
        buttons = GameObject.FindGameObjectsWithTag("ButtonUnpressed");


        foreach (GameObject button in buttons)
        {
            button.GetComponent<Renderer>().material.color = Color.red;
            button.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red * 1.5f);
            button.AddComponent<ButtonCOLLIDER>();
        }
    }

    void Update()
    {
        bool allButtonsPressed = true;
        bool hasPlayerSRC = false;

        foreach (GameObject button in buttons)
        {
            if (button.tag == "ButtonPressed")
            {
                button.GetComponent<Renderer>().material.color = Color.green;
                button.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green * 1.5f);
            }
            else
            {
                allButtonsPressed = false;
            }
        }

        if (allButtonsPressed && hasPlayerSRC == false)
        {
            hasPlayerSRC = true;
            doorOpenSrc.PlayOneShot(doorOpen);
            door.GetComponent<Renderer>().enabled = false;
        }
    }


    public void ButttonClick(GameObject buttonObj)
    {
        foreach (GameObject button in buttons)
        {
            if(button == buttonObj)
            {
                if(button.tag == "ButtonPressed")
                {
                    return;
                }
                button.tag = "ButtonPressed";
                button.GetComponent<Renderer>().material.color = Color.green;
                button.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green * 1.5f);
                buttonClickSrc.PlayOneShot(buttonClick);
                break;
            }
        }
    }
}

public class ButtonCOLLIDER : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        HorrorButtonManager.instance.ButttonClick(gameObject);
    }
}