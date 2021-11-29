using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;

    [Header("Chase Player")]
    public bool shouldChasePlayer;
    public float rangeToChasePlayer;
    private Vector3 moveDirection;

    [Header("Run Away")]
    public bool shouldRunAway;
    public float runawayRange;

    [Header("Wandering")]
    public bool shouldWander;
    public float wanderLength, pauseLength;
    float wanderCounter, pauseCounter;
    Vector3 wanderDirection;

    [Header("Patrolling")]
    public bool shouldPatrol;
    public Transform[] patrolPoint;
    int currentPatrolPoint;

    [Header("Shooting")]
    public bool shouldShoot;
    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    public float shootRange;

    float fireCounter;

    [Header("Drop Item")]
    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    [Header("Variables")]
    public SpriteRenderer theBody;

    public Animator anim;

    public int health = 150;

    public int deathSound = 1;

    public List<GameObject> deathSplatters = new List<GameObject>();
    public GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        if (shouldWander)
        {
            pauseCounter = Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (theBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            moveDirection = Vector3.zero;


            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer && shouldChasePlayer)
            {
                moveDirection = PlayerController.instance.transform.position - transform.position;
            }
            else
            {
                if (shouldWander)
                {
                    if(wanderCounter > 0)
                    {
                        wanderCounter -= Time.deltaTime;

                        //move the enemy
                        moveDirection = wanderDirection;

                        if(wanderCounter <= 0)
                        {
                            pauseCounter = Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
                        }
                    }

                    if(pauseCounter > 0)
                    {
                        pauseCounter -= Time.deltaTime;

                        if(pauseCounter <= 0)
                        {
                            wanderCounter = Random.Range(wanderLength * 0.75f, wanderLength * 1.25f);
                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                        }
                    }
                }

                if (shouldPatrol)
                {
                    moveDirection = patrolPoint[currentPatrolPoint].position - transform.position;
                    if(Vector3.Distance(transform.position, patrolPoint[currentPatrolPoint].position) < 0.2f)
                    {
                        currentPatrolPoint++;
                        if(currentPatrolPoint >= patrolPoint.Length)
                        {
                            currentPatrolPoint = 0;
                        }
                    }
                }
            }

            if(shouldRunAway && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < runawayRange)
            {
                moveDirection = transform.position - PlayerController.instance.transform.position;
            }


            moveDirection.Normalize();
            theRB.velocity = moveDirection * moveSpeed;


            if (shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < shootRange)
            {
                fireCounter -= Time.deltaTime;
                if (fireCounter <= 0)
                {
                    fireCounter = fireRate;
                    Instantiate(bullet, firePoint.position, firePoint.rotation);
                }
            }

        }
        else
        {
            theRB.velocity = Vector2.zero;
        }


        if (moveDirection != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    public void DamageEnemy(int dmg)
    {
        health -= dmg;

        Instantiate(hitEffect, transform.position, transform.rotation);

        if(health <= 0)
        {
            LevelManager.instance.currentScore += 10;
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(deathSound);
            //drop items
            if (shouldDropItem)
            {
                float dropChance = Random.Range(0f, 100f);

                if (dropChance < itemDropPercent)
                {
                    int randomItem = Random.Range(0, itemsToDrop.Length);

                    Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
                }
            }

            int selectedSplatter = Random.Range(0, deathSplatters.Count);

            int rotation = Random.Range(0, 4);
            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation * 90f));
        }
    }
}
