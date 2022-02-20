using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LittleDevilBehaviour : MonoBehaviour
{
    //public Animator animator;
    public Transform player;
    public Transform patrolRoute;
    public List<Transform> locations;
    public Transform target;

    private int locationIndex = 0;
    private NavMeshAgent agent;
    private int _lives = 999;
    //private bool ATK;
    //private bool DMG = false;

    public int EnemyLives
    {
        get
        {
            return _lives;
        }
        private set
        {
            _lives = value;
            if (_lives <= 0)
            {
                Destroy(this.gameObject);
                FindObjectOfType<AudioManager>().Play("BatDeath");
                Debug.Log("Enemy down.");
            }
        }
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InitializePatrolRoute();
        MoveToNextPatrolLocation();


        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (agent.remainingDistance < 0.2f && !agent.pathPending)
        {
            MoveToNextPatrolLocation();
        }

        if (target != null)
        {
            transform.LookAt(target);
        }

    }

    void InitializePatrolRoute()
    {
        foreach (Transform child in patrolRoute)
        {
            locations.Add(child);
        }
    }

    public void TakeDamage(int amount)
    {
        EnemyLives -= amount;
        Debug.Log("Critical hit!");
        FindObjectOfType<AudioManager>().Play("Hit");
    }

    void MoveToNextPatrolLocation()
    {

        if (locations.Count == 0)
            return;
        agent.destination = locations[locationIndex].position;

        locationIndex = (locationIndex + 1) % locations.Count;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            agent.destination = player.position;
            Debug.Log("Player detected - attack!");
            //ATK = true;
        }
        //animator.SetBool("ATK", ATK);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Player out of range, resume patrol");
            //ATK = false;
            //DMG = false;
        }
        //animator.SetBool("ATK", ATK);
        //animator.SetBool("DMG", DMG);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            EnemyLives -= 1;
            Debug.Log("Critical hit!");
            FindObjectOfType<AudioManager>().Play("Hit");
            //DMG = true;
        }
        else if (collision.gameObject.name != "Bullet(Clone)")
        {
            //DMG = false;
        }
        //animator.SetBool("DMG", DMG);
    }
}
