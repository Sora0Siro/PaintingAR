using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float multiplier = 30f;
    private void Update()
    {
        transform.Rotate(Vector3.up, speed * multiplier);
    }
}