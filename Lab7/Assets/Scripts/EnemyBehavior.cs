using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player; 
        public float speed = 3f;
    public float chaseRange = 5f;
    public float attackRange = 1.5f;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
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
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    void Attack()
    {
        Debug.Log("Attack!");
    }
}