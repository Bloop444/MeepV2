using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceVisualizer : MonoBehaviour
{
    [Header("Made by Glitched Cat Studios!\nPlease give credits if you use it!")]
    [Space]
    public AudioSource audioSource;
    public GameObject[] visualizerBars;
    public float heightMultiplier = 10.0f;
    public BarAlignment barAlignment = BarAlignment.Middle;

    private float[] samples = new float[512];
    private float[] spectrum = new float[512];
    private float[] barHeights;

    private void Start()
    {
        barHeights = new float[visualizerBars.Length];

        for (int i = 0; i < visualizerBars.Length; i++)
        {
            barHeights[i] = visualizerBars[i].transform.localScale.y;
        }
    }

    private void Update()
    {
        audioSource.GetOutputData(samples, 0);
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        int samplesPerBar = spectrum.Length / visualizerBars.Length;

        for (int i = 0; i < visualizerBars.Length; i++)
        {
            float sum = 0.0f;

            for (int j = 0; j < samplesPerBar; j++)
            {
                int index = i * samplesPerBar + j;
                if (index < spectrum.Length)
                {
                    sum += spectrum[index];
                }
            }

            float averageHeight = (sum / samplesPerBar) * heightMultiplier;
            barHeights[i] = Mathf.Lerp(barHeights[i], averageHeight, Time.deltaTime * 50);
        }

        for (int i = 0; i < visualizerBars.Length; i++)
        {
            float barHeight = barHeights[i];
            Vector3 scale = visualizerBars[i].transform.localScale;
            Vector3 position = visualizerBars[i].transform.localPosition;

            switch (barAlignment)
            {
                case BarAlignment.Middle:
                    visualizerBars[i].transform.localScale = new Vector3(scale.x, barHeight, scale.z);
                    break;
                case BarAlignment.Bottom:
                    visualizerBars[i].transform.localScale = new Vector3(scale.x, barHeight, scale.z);
                    visualizerBars[i].transform.localPosition = new Vector3(position.x, barHeight / 2, position.z);
                    break;
                case BarAlignment.Top:
                    visualizerBars[i].transform.localScale = new Vector3(scale.x, barHeight, scale.z);
                    visualizerBars[i].transform.localPosition = new Vector3(position.x, -barHeight / 2, position.z);
                    break;
            }
        }
    }
}

public enum BarAlignment
{
    Middle,
    Bottom,
    Top
}
