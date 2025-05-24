using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class OnGrab : MonoBehaviour
{
    public string playerLayerName = "Player";
    public List<Collider> initiallyIgnoredColliders = new List<Collider>();

    private Collider[] grabbableColliders;
    private Collider[] playerColliders;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabbableColliders = GetComponentsInChildren<Collider>();

        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerColliders = player.GetComponentsInChildren<Collider>();
        }

        foreach (var grabbable in grabbableColliders)
        {
            foreach (var ignored in initiallyIgnoredColliders)
            {
                if (ignored != null)
                    Physics.IgnoreCollision(grabbable, ignored, true);
            }
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        foreach (var grabbable in grabbableColliders)
        {
            foreach (var player in playerColliders)
            {
                Physics.IgnoreCollision(grabbable, player, true);
            }

            foreach (var ignored in initiallyIgnoredColliders)
            {
                if (ignored != null)
                    Physics.IgnoreCollision(grabbable, ignored, false);
            }
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        foreach (var grabbable in grabbableColliders)
        {
            foreach (var player in playerColliders)
            {
                Physics.IgnoreCollision(grabbable, player, false);
            }

            foreach (var ignored in initiallyIgnoredColliders)
            {
                if (ignored != null)
                    Physics.IgnoreCollision(grabbable, ignored, true);
            }
        }
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }
}
