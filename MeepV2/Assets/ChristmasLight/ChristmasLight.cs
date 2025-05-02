using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChristmasLight : MonoBehaviour
{
    // Script made by HeriXD3
    // Discord: https://discord.gg/pQGJU4q8kj

    [Header("Made by HeriXD3 Credit Me in tutorials,")] 
    [Header("Discord Server https://discord.gg/pQGJU4q8k ")] 
    [Tooltip("Discord: https://discord.gg/pQGJU4q8kj")]
    public GameObject[] lightBulbs;    // Array to support multiple parent objects (renamed from 'torno' to 'lightBulb')
    public float blinkSpeed = 0.5f;    // How fast the lights blink

    public Material[] lightMaterials;  // Array of materials for different colors

    private List<Renderer> allLightRenderers = new List<Renderer>(); // List to store all child renderers across multiple lightBulbs

    private void Start()
    {
        // Collect all renderers from all lightBulbs
        foreach (GameObject lightBulb in lightBulbs)
        {
            if (lightBulb != null)
            {
                Renderer[] renderers = lightBulb.GetComponentsInChildren<Renderer>();
                allLightRenderers.AddRange(renderers);
            }
        }

        // Start blinking the lights
        StartCoroutine(BlinkLights());
    }

    private IEnumerator BlinkLights()
    {
        while (true)
        {
            foreach (Renderer renderer in allLightRenderers)
            {
                if (renderer != null)
                {
                    // Randomly decide whether to change the light's color
                    if (Random.value > 0.3f) // 70% chance to change color
                    {
                        // Pick a random material from the lightMaterials array
                        Material newMaterial = lightMaterials[Random.Range(0, lightMaterials.Length)];

                        // Assign the new material to the renderer
                        renderer.material = newMaterial;
                    }
                    else
                    {
                        // Turn off the light (set material to black)
                        renderer.material.color = Color.black; // Directly set the color to black
                    }
                }
            }

            // Wait before the next blink
            yield return new WaitForSeconds(blinkSpeed);
        }
    }
}