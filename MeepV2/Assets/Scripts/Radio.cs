using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Radio : MonoBehaviour
{
    public List<AudioClip> VoiceLines;
    public List<AudioClip> Songs;
    public AudioSource AudioSource;
    public TextMeshPro SongDisplay;
    public Slider SongSlider;
    private bool isPlaying = false;
    private float pausedTime = 0f;
    private AudioClip currentClip;

    private void Start()
    {
        if (AudioSource == null)
        {
            AudioSource = GetComponent<AudioSource>();
        }
        PlayRandomClip();
    }

    private void Update()
    {
        if (isPlaying && AudioSource.clip != null && AudioSource.isPlaying)
        {
            SongSlider.value = AudioSource.time / AudioSource.clip.length;
        }
    }

    private void PlayRandomClip()
    {
        List<AudioClip> allClips = new List<AudioClip>(VoiceLines);
        allClips.AddRange(Songs);

        if (allClips.Count == 0) return;

        currentClip = allClips[Random.Range(0, allClips.Count)];
        AudioSource.clip = currentClip;
        AudioSource.time = pausedTime;
        AudioSource.Play();
        isPlaying = true;
        UpdateSongDisplay();
        StartCoroutine(WaitForClipEnd(AudioSource.clip.length - pausedTime));
    }

    private IEnumerator WaitForClipEnd(float duration)
    {
        yield return new WaitForSeconds(duration);
        pausedTime = 0f;
        PlayRandomClip();
    }

    private void UpdateSongDisplay()
    {
        if (SongDisplay != null && AudioSource.clip != null)
        {
            SongDisplay.text = AudioSource.clip.name;
        }
    }

    private void OnDisable()
    {
        if (AudioSource.isPlaying)
        {
            pausedTime = AudioSource.time;
            AudioSource.Pause();
        }
    }

    private void OnEnable()
    {
        if (currentClip != null)
        {
            AudioSource.clip = currentClip;
            AudioSource.time = pausedTime;
            AudioSource.Play();
            isPlaying = true;
        }
        else
        {
            PlayRandomClip();
        }
    }
}
