using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.VR.Player;
using Photon.VR.Testing;
using Photon.VR;

public class SetColor : MonoBehaviour
{
    public float red;
    public float green;
    public float blue;

    [Header("Set This To r g or b")]
    public string rgb;
    public float value;

    private void OnTriggerEnter(Collider other)
    {
        GameObject[] colorbuts = GameObject.FindGameObjectsWithTag("ColorTag");
        if (other.CompareTag("HandTag"))
        {
            if (rgb == "r")
            {
                red = value;
                foreach (GameObject obj in colorbuts)
                {
                    SetColor setColor = obj.GetComponent<SetColor>();
                    setColor.red = value;
                }
                PlayerPrefs.SetFloat("r", red);
                ChangeColor();
            }
            else
            {
                if (rgb == "g")
                {
                    green = value;
                    foreach (GameObject obj in colorbuts)
                    {
                        SetColor setColor = obj.GetComponent<SetColor>();
                        setColor.green = value;
                    }
                    PlayerPrefs.SetFloat("g", green);
                    ChangeColor();
                }
                else
                {
                    if (rgb == "b")
                    {
                        blue = value;
                        foreach (GameObject obj in colorbuts)
                        {
                            SetColor setColor = obj.GetComponent<SetColor>();
                            setColor.blue = value;
                        }
                        PlayerPrefs.SetFloat("b", blue);
                        ChangeColor();
                    }
                }
            }
        }
    }
    void ChangeColor()
    {
        Color color = new Color(red, green, blue);
        PhotonVRManager.SetColour(color);
    }
    private void Start()
    {
        red = PlayerPrefs.GetFloat("r");
        green = PlayerPrefs.GetFloat("g");
        blue = PlayerPrefs.GetFloat("b");
        Color color = new Color(red, green, blue);
        PhotonVRManager.SetColour(color);
    }
}
