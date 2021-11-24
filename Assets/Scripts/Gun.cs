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
                    AudioManager.instance.PlaySFX(7);
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                    shotCounter = timeBetweenShots;
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
}
