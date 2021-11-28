using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerRocket : MonoBehaviour
{
    public static PlayerRocket instance;
    public float speed = 7.5f;
    public Rigidbody2D theRB;

    public GameObject explosion;
    public int damageToGive = 50;

    public int impactSound = 2;

    public bool isHoming;
    public float rotateSpeed = 200f;
    GameObject target;

    //public bool isBouncing;
    //public int bouncingTime;
    //public float bouncingSpeed;
    //Vector2 b_dir;
    //ContactPoint2D[] contacts = new ContactPoint2D[10];

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        theRB.velocity = transform.right * speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);

        AudioManager.instance.PlaySFX(impactSound);

        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive);
        }

        if (other.tag == "Boss")
        {
            BossController.instance.TakeDamage(damageToGive);
            Instantiate(BossController.instance.hitEffect, transform.position, transform.rotation);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
