using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public static PlayerBullet instance;
    public float speed = 7.5f;
    public Rigidbody2D theRB;

    public GameObject impactEffect;
    public int damageToGive = 50;

    public int impactSound = 2;

    public bool isHoming;
    public float homingRange;
    public float rotationSpeed = 1f;
    Vector3 direction;
    Transform target;
    Quaternion rotateToTarget;
    bool isTarget;

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
    private void Update()
    {
        if (isHoming && GameObject.FindGameObjectWithTag("Enemy"))
        {
            Debug.Log(GameObject.FindGameObjectWithTag("Enemy").activeInHierarchy);
            target = GameObject.FindGameObjectWithTag("Enemy").transform;
            if (target)
            {
                if (Vector3.Distance(transform.position, target.position) > 0.5f && Vector3.Distance(transform.position, target.position) < homingRange)
                {
                    direction = (target.position - transform.position).normalized;
                }
            }
            isTarget = true;
        }
        else
        {
            isTarget = false;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isTarget || direction == new Vector3(0f,0f,0f))
        {
            direction = transform.right;
            theRB.velocity = direction * speed;
        }
        else
        {
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            rotateToTarget = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * rotationSpeed);
            theRB.velocity = new Vector2(direction.x, direction.y) * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
        Debug.Log(other.IsTouchingLayers());
        //if (!isBouncing)
        //{

        //}
        //else
        //{
        //    if (other.tag != "Enemy" || other.tag == "Boss")
        //    {

        //        other.GetContacts(contacts);
        //        Vector2 wallNormal = contacts[0].normal;
        //        Vector2 b_dir = Vector2.Reflect(theRB.velocity.normalized, wallNormal).normalized;
        //        Debug.Log(b_dir);
        //    }
        //}
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
