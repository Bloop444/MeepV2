using UnityEngine;
using Photon.Pun;

public class SecureDoor : MonoBehaviourPunCallbacks
{
    public Transform door;
    public Transform openTarget;
    public Transform closeTarget;
    public float speed = 5f;

    public AudioClip moveClip;
    public AudioClip openClip;
    public AudioClip closedClip;
    public AudioClip warningClip;

    public AudioSource audioSource;

    private bool isOpen = false;
    private bool isMoving = false;
    private Vector3 targetPosition;

    void Update()
    {
        if (isMoving)
        {
            float step = speed * Time.deltaTime;
            door.position = Vector3.MoveTowards(door.position, targetPosition, step);

            if (!audioSource.isPlaying || audioSource.clip != moveClip)
            {
                audioSource.clip = moveClip;
                audioSource.loop = true;
                audioSource.pitch = Mathf.Clamp(speed / 5f, 0.5f, 2f);
                audioSource.Play();
            }

            if (Vector3.Distance(door.position, targetPosition) < 0.01f)
            {
                isMoving = false;
                audioSource.Stop();
                PlayOneShot(isOpen ? openClip : closedClip);
            }
        }
    }

    [PunRPC]
    public void ToggleDoor()
    {
        isOpen = !isOpen;
        targetPosition = isOpen ? openTarget.position : closeTarget.position;
        isMoving = true;
    }

    public void PlayWarning()
    {
        PlayOneShot(warningClip);
    }

    public void PlayOneShot(AudioClip clip)
    {
        audioSource.loop = false;
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(clip);
    }
}
