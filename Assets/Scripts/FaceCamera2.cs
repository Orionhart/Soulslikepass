using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera2 : MonoBehaviour
{
    private Transform cameraTransform;

    private void Start()
    {
        // Find the main camera in the scene
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        // Make the object face the camera
        transform.LookAt(cameraTransform);
    }
}
