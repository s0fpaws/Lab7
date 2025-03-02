using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour
{
    public Transform patrol, player, enemy;
    private Transform[] locations;
    private NavMeshAgent agent;
    private int curLocation;
    private bool chasing;
    private PlayerController controller;
    private Renderer enemyRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = player.GetComponent<PlayerController>();
        enemyRenderer = enemy.gameObject.GetComponent<Renderer>();
        InitializePatrol();
        NextPatrol();
        //set initial not red color here // you can change it from white if you want
        //enemyRenderer.material.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (chasing)
        {
            agent.SetDestination(player.position);
            float distance = Vector3.Distance(enemy.position, player.position);
            Debug.Log(distance);
            if (distance < 1.65f)
            {
                controller.enabled = false; // for whatever reason disabling the controller is the only way to get this to work
                player.position = new Vector3(5, 1.3f, 5);
                controller.enabled = true;
            }
        }
        if (!chasing && !agent.pathPending && agent.remainingDistance < 0.2f)
        {
            NextPatrol();
        }
    }

    void NextPatrol()
    {
        if (locations.Length == 0) return;

        agent.SetDestination(locations[curLocation].position);
        curLocation = (curLocation + 1) % locations.Length;
    }

    void InitializePatrol()
    {
        agent.speed = 5;
        locations = new Transform[patrol.childCount];
        for (int i = 0; i < patrol.childCount; i++)
        {
            locations[i] = patrol.GetChild(i).transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Chasing player");
            chasing = true;
            agent.speed = 9;
            enemy.localScale = new Vector3(1.5f, 2f, 1.5f);
            //change color to red here
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Resuming patrol");
            chasing = false;
            NextPatrol();
            agent.speed = 5;
            enemy.localScale = Vector3.one;
            //change color to not red here
        }
    }

    // I don't think this is even used because I don't think the enemies can fall down, so this is untested
    void FallenDown()
    {
        if (enemy.position.y < -10)
        {
            enemy.position = new Vector3(locations[0].position.x, locations[0].position.y, locations[0].position.z);
        }
    }
}
