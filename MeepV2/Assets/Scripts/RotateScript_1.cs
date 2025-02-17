using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // Set the rotation speeds (in degrees per second) for each axis
    public float rotationSpeedX = 0f; // No rotation around X-axis
    public float rotationSpeedY = 30f; // Rotation around Y-axis
    public float rotationSpeedZ = 0f; // No rotation around Z-axis

    private void Update()
    {
        // Rotate the object around its local axes
        transform.Rotate(Vector3.right, rotationSpeedX * Time.deltaTime);
        transform.Rotate(Vector3.up, rotationSpeedY * Time.deltaTime);
        transform.Rotate(Vector3.forward, rotationSpeedZ * Time.deltaTime);
    }
}