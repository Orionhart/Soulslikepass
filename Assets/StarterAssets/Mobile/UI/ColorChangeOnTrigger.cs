using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeOnTrigger : MonoBehaviour
{
    public string targetTag = "Bullet";
    public Color redColor = Color.red;
    public Color defaultColor = Color.white;
    public float resetDelay = 2f;

    private Renderer objectRenderer;
    private bool isChangingColor = false;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isChangingColor && other.gameObject.CompareTag(targetTag))
        {
            isChangingColor = true;
            objectRenderer.material.color = redColor;
            Invoke("ResetColor", resetDelay);
        }
    }

    private void ResetColor()
    {
        objectRenderer.material.color = defaultColor;
        isChangingColor = false;
    }
}