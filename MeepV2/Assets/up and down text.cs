using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSelector : MonoBehaviour
{
    public List<TextMeshPro> textObjects; 
    public GameObject upButton; 
    public GameObject downButton; 
    public float enlargedSize = 2f; 
    public float normalSize = 1f;

    private int currentIndex = 0;

    private void Start()
    {
        UpdateTextSize();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag"))
        {
            if (other.gameObject == upButton)
            {
                MoveSelection(-1);
            }
            else if (other.gameObject == downButton)
            {
                MoveSelection(1);
            }
        }
    }

    private void MoveSelection(int direction)
    {
        int newIndex = currentIndex + direction;

        if (newIndex >= 0 && newIndex < textObjects.Count)
        {
            currentIndex = newIndex;
            UpdateTextSize();
        }
    }

    private void UpdateTextSize()
    {
        for (int i = 0; i < textObjects.Count; i++)
        {
            if (i == currentIndex)
                textObjects[i].fontSize = enlargedSize;
            else
                textObjects[i].fontSize = normalSize;
        }
    }
}
