using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float destructionDelay = 5f;

    private void Start()
    {
        Invoke("DestroyObject", destructionDelay);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}