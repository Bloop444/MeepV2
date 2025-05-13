using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonColorChangeScript : MonoBehaviour
{

    [Header("THIS SCRIPT WAS MADE BY MMPro/Bloox/bluu.")]
    [Header("PLEASE DO NOT DISTRIBUTE OR EDIT THIS SCRIPT WITHOUT THE OWNERS PERMISSION")]

   // public GameObject Button;
    public Material buttonPressed;
    public Material buttonUnPressed;

    // Start is called before the first frame update
    void OnTriggerEnter()
    {
        gameObject.GetComponent<MeshRenderer>().material = buttonPressed;
        //new WaitForSeconds(0.4f);
        //Button.GetComponent<MeshRenderer>().material = buttonUnPressed;
    }

    // Update is called once per frame
    void OnTriggerExit()
    {
        gameObject.GetComponent<MeshRenderer>().material = buttonUnPressed;
    }
}
