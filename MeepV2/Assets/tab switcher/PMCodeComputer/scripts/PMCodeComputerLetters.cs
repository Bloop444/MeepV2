using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PMCodeComputerLetters : MonoBehaviour
{
    public CodeManager manager;
    public string HandTag = "HandTag";
    public string Letter;
    private Color oldColor;
    public bool IsALetter;
    public bool BackSpace;
    public float Delay = 0.4f;

    private void Start()
    {
        oldColor = gameObject.GetComponent<Renderer>().material.color;
    }
    private IEnumerator color()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(1, 0, 0);
        yield return new WaitForSeconds(.5f);
        gameObject.GetComponent<Renderer>().material.color = oldColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == HandTag)
        {
            StartCoroutine(color());
            StartCoroutine(SODelayWorks());
        }
    }

    private IEnumerator SODelayWorks()
    {
        if (BackSpace)
        {
            yield return new WaitForSeconds(Delay);
            manager.NewRoomThingy = manager.NewRoomThingy.Remove(manager.NewRoomThingy.Length - 1);
        }
        if (IsALetter)
        {
            yield return new WaitForSeconds(Delay);
            manager.NewRoomThingy += Letter;
        }
    }
}
