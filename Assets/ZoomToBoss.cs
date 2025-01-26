using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomToBoss : MonoBehaviour
{
    public float ForwardSpeed = 30f;
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * ForwardSpeed;
    }
}
