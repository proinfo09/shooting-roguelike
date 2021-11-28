using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBullet : MonoBehaviour
{
    Rigidbody2D theRb;
    Vector3 lastVelocity;
    public int bouncingSpeed;
    private void Awake()
    {
        theRb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        theRb.velocity = Vector2.one * bouncingSpeed * Time.deltaTime;
        lastVelocity = theRb.velocity;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 b_dir = Vector2.Reflect(lastVelocity.normalized, other.contacts[0].normal);
        theRb.velocity = b_dir * bouncingSpeed;
        if (other.gameObject.CompareTag("Wall"))
        {
            
        }
    }
}
