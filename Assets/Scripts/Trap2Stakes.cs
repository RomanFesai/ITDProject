using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap2Stakes : MonoBehaviour
{
    private bool ACTIVE = false;
    public Animator animator;
    private GameBehaviour _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Exit").GetComponent<GameBehaviour>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            ACTIVE = true;
            animator.SetBool("ACTIVE", ACTIVE);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            ACTIVE = false;
            animator.SetBool("ACTIVE", ACTIVE);
        }
    }
}
