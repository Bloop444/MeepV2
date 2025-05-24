using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableColliderOnGrab : MonoBehaviour
{
    private Collider objectCollider;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody body;

    void Start()
    {
        objectCollider = GetComponent<Collider>();
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        body = GetComponent<Rigidbody>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (objectCollider != null)
        {
            objectCollider.enabled = false;

            body.useGravity = false;
            body.isKinematic = true;
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        if (objectCollider != null)
        {
            objectCollider.enabled = true;

            body.useGravity = true;
            body.isKinematic = false;
        }
    }
}