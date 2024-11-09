using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private GameObject player;
    [SerializeField] private List<Vector3> patrolLocations = new List<Vector3>();
    [SerializeField] private float playerSensitivity; // This variable controls how sensitive the enemy is to the player. Higher is more sensitive.

    private bool isChasingPlayer;

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) / PlayerLightSystem.Instance.LightAmount > playerSensitivity) // Higher the light, the more chance of being found
        {

        }
    }
}
