using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyVision : MonoBehaviour
{

    public Animator animator;
    public Transform player;
    private NavMeshAgent agent;
    private bool ATK;
    private bool DMG = false;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            agent.destination = player.position;
            Debug.Log("Player detected - attack!");
            ATK = true;
        }
        animator.SetBool("ATK", ATK);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Player out of range, resume patrol");
            ATK = false;
            DMG = false;
        }
        animator.SetBool("ATK", ATK);
        animator.SetBool("DMG", DMG);
    }
}
