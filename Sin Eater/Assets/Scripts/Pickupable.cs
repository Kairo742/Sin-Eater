using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public Quaternion Rotation;
    public int AmountOfObjects = 1;
    public bool IsDiscrete = true, IsThrowable = true;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        if(IsDiscrete) _rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
    }


    private void OnCollisionEnter(Collision col)
    {
        if(!IsDiscrete)
        {
            _rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            IsDiscrete = true;
        }
    }
}
