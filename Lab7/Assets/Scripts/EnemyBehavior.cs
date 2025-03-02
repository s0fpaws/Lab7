using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float chaseRange = 10f;  // Adjusted for testing
    public float attackRange = 2f;  // Adjusted for testing

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not assigned!");
        }
    }

    private void Update()
    {
        if (player == null)
        {
            Debug.Log("Player reference is missing!");
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        Debug.Log("Distance to Player: " + distance);

        if (distance <= chaseRange && distance > attackRange)
        {
            Debug.Log("Chasing Player");
            ChasePlayer();
        }
        else if (distance <= attackRange)
        {
            Debug.Log("Attacking Player");
            Attack();
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void Attack()
    {
        Debug.Log("Attack!");
    }
}
