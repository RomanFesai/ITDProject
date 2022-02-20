using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    public GameBehaviour gameManager;

    void Start()
    {
        gameManager = GameObject.Find("Exit").GetComponent<GameBehaviour>();   
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Player")
        {
            FindObjectOfType<AudioManager>().Play("Key");

            Destroy(this.transform.parent.gameObject);

            Debug.Log("Item collected");

            gameManager.Items += 1;
        }
    }
}
