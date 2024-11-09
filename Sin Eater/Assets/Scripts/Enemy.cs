using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject shot;
    [SerializeField] private float playerSensitivity; // This variable controls how sensitive the enemy is to the player. Higher is more sensitive.
    [SerializeField] private float shootDelayTime = 2f;
    [SerializeField] private float shootStrength = 10f;

    private bool detectedPlayer;

    void Start()
    {
        InvokeRepeating("ShootPlayer", 0.5f, shootDelayTime);
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) / PlayerLightSystem.Instance.LightAmount < playerSensitivity) // Higher the light, the more chance of being found
        {
            detectedPlayer = true;

            transform.LookAt(player.transform.position);
        }
        else // If player moves out of range, disable the turret
        {
            detectedPlayer = false;
        }
    }

    void ShootPlayer()
    {
        // I have no idea what the turret is supposed to do, but I put in a basic "cannonball" shoot mechanism

        if (detectedPlayer)
        {
            var cannonball = Instantiate(shot, transform.position + transform.forward * 2f, transform.rotation);
            cannonball.GetComponent<Rigidbody>().AddForce(transform.forward * shootStrength);
            Physics.IgnoreCollision(cannonball.GetComponent<Collider>(), GetComponent<Collider>(), true);
        }
    }
}
