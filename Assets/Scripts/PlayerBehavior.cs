using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;
    public Animator animator2;
    public float moveSpeed = 10f;
    public float rotateSpeed = 75f;
    public float jumpVelocity = 1f;
    public float distanceToGround = 0.1f;
    public LayerMask groundLayer;
    public GameObject bullet;
    public float bulletSpeed = 0f;
    public float fireRate = 0.5F;
    private float nextFire = 0.0F;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;

    [Header("FootStepParametrs")]
    [SerializeField] private float baseStepSpeed = 0.2f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] FootStepClip = default;
    public float footStepTimer = 0;
    public bool isWalking = false;


    Vector3 velocity;
    bool IsGrounded;

    private float zInput;
    private float xInput;
    private bool mInput;
    private bool ACTIVE = false;
    
    private CapsuleCollider _col;
    private GameBehaviour _gameManager;
    //Raycast sword
    public Camera fpsCam;
    public float Range = 0.001f;
    public float impactForce = 30f;
    public int Damage = 1;


    // Start is called before the first frame update

    void Start()
    {
        _col = GetComponent<CapsuleCollider>();

        _gameManager = GameObject.Find("Exit").GetComponent<GameBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        mInput = false;
        Movement();

        if (isGrounded() && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (isGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpVelocity * -2f * gravity);
        }

        //fireball
        /*if (Input.GetMouseButtonDown(1) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject newBullet = Instantiate(bullet, this.transform.position, this.transform.rotation) as GameObject;
            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();
            bulletRB.velocity = this.transform.forward * bulletSpeed;
            //mInput = true;
            //FindObjectOfType<AudioManager>().Play("Sword");
        }*/
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot();
            FindObjectOfType<AudioManager>().Play("Sword");
            mInput = true;
        }
        else
        {
            mInput = false;
        }
        animator.SetBool("mInput", mInput);

        checkWalk();
        animator.SetBool("isWalking", isWalking);
        if (isWalking)
        { 
            Handle_FootSteps();
        }
    }

    public void checkWalk()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || (Input.GetKeyDown(KeyCode.W) && Input.GetKeyDown(KeyCode.S))) 
        {
            isWalking = true;
        }
        else if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || (Input.GetKeyUp(KeyCode.W) && Input.GetKeyUp(KeyCode.S))) 
        {
            isWalking = false;
        }
    }

    public void Movement()
    {
        zInput = Input.GetAxis("Vertical");
        xInput = Input.GetAxis("Horizontal");

        Vector3 move = transform.right * xInput + transform.forward * zInput;
        if (move.magnitude > 1)
            move /= move.magnitude;
        controller.Move(move * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public bool isGrounded()
    {
        /*Vector3 capsuleBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        bool grounded = Physics.CheckCapsule(_col.bounds.center, capsuleBottom, distanceToGround, groundLayer, QueryTriggerInteraction.Ignore);
        return grounded;*/
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        return IsGrounded;
    }

    private void Handle_FootSteps()
    {
        footStepTimer -= Time.deltaTime;
        if (isGrounded() && footStepTimer <= 0)
        {
            footstepAudioSource.PlayOneShot(FootStepClip[Random.Range(0, FootStepClip.Length-1)]);
            footStepTimer = baseStepSpeed;
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, Range))
        {
            EnemyBehaviour targetSkelet = hit.transform.GetComponent<EnemyBehaviour>();
            BatDemonBehaviour targetBat = hit.transform.GetComponent<BatDemonBehaviour>();
            BossDemonBehaviour targetBoss = hit.transform.GetComponent<BossDemonBehaviour>();
            LittleDevilBehaviour targetDevil = hit.transform.GetComponent<LittleDevilBehaviour>();
            if (targetSkelet != null)
            {
                targetSkelet.TakeDamage(Damage);
            }
            else if(targetBat != null)
            {
                targetBat.TakeDamage(Damage);
            }
            else if (targetBoss != null)
            {
                targetBoss.TakeDamage(Damage);
            }
            else if(targetDevil != null)
            {
                targetDevil.TakeDamage(Damage);
            }
           /* if (hit.rigidbody != hit.collider.isTrigger)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }*/

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Enemy" || collision.gameObject.name == "Enemy2" || collision.gameObject.name == "BatDemon" || collision.gameObject.name == "BossDemon" || collision.gameObject.name == "LittleDevil")
        {
            _gameManager.Lives -= 1;
            FindObjectOfType<AudioManager>().Play("Hurt");
        }
        if(collision.gameObject.name == "Trap")
        {
            FindObjectOfType<AudioManager>().Play("Hurt");
            _gameManager.Lives -= 99;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "stakes")
        {
            ACTIVE = true;
            animator2.SetBool("ACTIVE", ACTIVE);
            FindObjectOfType<AudioManager>().Play("Hurt");
            _gameManager.Lives -= 1;
            moveSpeed = 2f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "stakes")
        {
            ACTIVE = false;
            animator2.SetBool("ACTIVE", ACTIVE);
            moveSpeed = 7f;
        }
    }
}
