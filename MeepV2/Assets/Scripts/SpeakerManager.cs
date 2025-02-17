using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeakerManager : MonoBehaviour
{

    public GameObject Music;
    public TextMeshPro Text;
    public MeshRenderer MeshRenderer;
    public Material UnMutedMaterial;
    public Material MutedMaterial;
    public string PlayerPrefName;
    public string HandTag = "HandTag";

    void Start()
    {
        string Check = PlayerPrefs.GetString(PlayerPrefName);
        if(Check == "Muted")
        {
            Music.SetActive(false);
            MeshRenderer.material = MutedMaterial;
            Text.SetText("Muted");
        }
        else if (Check == "Unmuted")
        {
            Music.SetActive(true);
            MeshRenderer.material = UnMutedMaterial;
            Text.SetText("Unmuted");
        }
        else if (Check == "")
        {
            Music.SetActive(true);
            MeshRenderer.material = UnMutedMaterial;
            Text.SetText("Unmuted");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == HandTag)
        {
            if (Music.activeSelf == true)
            {
                Music.SetActive(false);
                PlayerPrefs.SetString(PlayerPrefName, "Muted");
                MeshRenderer.material = MutedMaterial;
                Text.SetText("Muted");
            }
            else if (Music.activeSelf == false)
            {
                Music.SetActive(true);
                PlayerPrefs.SetString(PlayerPrefName, "Unmuted");
                MeshRenderer.material = UnMutedMaterial;
                Text.SetText("Unmuted");
            }
        }
    }
}
