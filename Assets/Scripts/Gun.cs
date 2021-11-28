using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletToFire;
    public float timeBetweenShots;
    float shotCounter;

    public string weaponName;
    public Sprite gunUI;

    public int itemCost;
    public Sprite gunShopSprite;

    [SerializeField]
    private float defDistanceRay = 100;
    public bool isLaser;
    public LineRenderer m_lineRenderer;
    Transform m_transform;
    float hitCounter;

    public bool isShotgun;
    public int numberOfBullets;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.canMove && !LevelManager.instance.isPaused)
        {
            if (shotCounter > 0)
            {
                shotCounter -= Time.deltaTime;
            }
            else
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
                {
                    if (isShotgun)
                    {
                        AudioManager.instance.PlaySFX(7);
                        ShotgunShoot();
                    }
                    else if (isLaser)
                    {
                        m_lineRenderer.enabled = true;
                        ShootLaser();
                    }
                    else
                    {
                        AudioManager.instance.PlaySFX(7);
                        Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                    }
                    shotCounter = timeBetweenShots;
                }
                else
                {
                    if (isLaser)
                    {
                        m_lineRenderer.enabled = false;
                    }
                }

                //if (Input.GetMouseButton(0))
                //{
                //    shotCounter -= Time.deltaTime;
                //    if (shotCounter <= 0)
                //    {
                //        AudioManager.instance.PlaySFX(7);
                //        Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                //        shotCounter = timeBetweenShots;
                //    }
                //}
            }
        }
    }

    void ShootLaser()
    {
        if(Physics2D.Raycast(transform.position, transform.right))
        {
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, transform.right);
            Draw2DRay(firePoint.position, _hit.point);
            if (_hit)
            {
                if (_hit.transform.tag == "Enemy")
                {
                    LaserHitEnemy(_hit);
                }
            }
        }
        else
        {
            Draw2DRay(firePoint.position, firePoint.transform.right * defDistanceRay);
        }
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);
    }

    void LaserHitEnemy(RaycastHit2D enemyHit)
    {
        if (hitCounter > 0)
        {
            hitCounter -= Time.deltaTime;
        }
        else
        {
            if (enemyHit)
            {
                enemyHit.transform.GetComponent<EnemyController>().DamageEnemy(40);
                var enemyHitEffect = enemyHit.transform.GetComponent<EnemyController>().hitEffect;
                Instantiate(enemyHitEffect, enemyHit.transform.position, enemyHit.transform.rotation);
            }
            hitCounter = 1f;
        }
    }

    void ShotgunShoot() 
    {
        for(int i = 0; i < numberOfBullets; i++)
        {
            Instantiate(bulletToFire, firePoint.position, Quaternion.Euler(0f, 0f, firePoint.eulerAngles.z + i * 5f));
        }
        for (int x = 0; x < numberOfBullets; x++)
        {
            Instantiate(bulletToFire, firePoint.position, Quaternion.Euler(0f, 0f, firePoint.eulerAngles.z + x * 7f));
        }
    }
}
