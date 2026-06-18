using UnityEngine;

public class SpriteRotatoToCamera : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        // Automatically find and cache the main scene camera
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (cameraTransform != null)
        {
            // Makes the sprite face the camera perfectly
            transform.LookAt(transform.position + cameraTransform.forward);
        }
    }
}
