using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject shot;
    [SerializeField] private float playerSensitivity; // This variable controls how sensitive the enemy is to the player. Higher is more sensitive.
    [SerializeField] private float attackDelayTime = 2f;
    //[SerializeField] private float shootStrength = 10f; // For optional ranged attack later down the script
    [SerializeField] private float speed;
    [SerializeField] private float attackDistance = 4f;

    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material attackMaterial;

    private bool detectedPlayer;

    void Start()
    {
        InvokeRepeating("ShootPlayer", 0.5f, attackDelayTime);
    }

    void Update()
    {
        detectedPlayer = Vector3.Distance(player.transform.position, transform.position) / PlayerLightSystem.Instance.LightAmount < playerSensitivity; // Higher the light, the more chance of being found

        if (detectedPlayer)
        {
            renderer.material = attackMaterial;
            Vector3 targetPosition = player.transform.position;
            targetPosition.y = transform.position.y;  // Ensure the enemy only rotates on the Y-axis
            transform.LookAt(targetPosition);

            if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
            {
                // Move directly towards the player
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }
        }
        else
        {
            renderer.material = normalMaterial;
        }
    }

    void ShootPlayer()
    {
        // The golem is simply attacking the player every attackDelayTime seconds.

        if (detectedPlayer && Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
        {
            player.GetComponent<PlayerMovement>().Attacked(1f);

            /*
            var cannonball = Instantiate(shot, transform.position + transform.forward * 2f, transform.rotation); // Ranged attack code if anybody needs it
            cannonball.GetComponent<Rigidbody>().AddForce(transform.forward * shootStrength);
            Physics.IgnoreCollision(cannonball.GetComponent<Collider>(), GetComponent<Collider>(), true);
            */
        }
    }
}
