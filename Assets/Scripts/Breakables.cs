using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    public GameObject[] brokenPieces, smallPieces;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    public int breakSound = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" || other.tag == "PlayerBullet")
        {
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(breakSound);

            //show broken pieces
            int randomPiece = Random.Range(0, brokenPieces.Length);
            Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
            if(randomPiece != 0)
            {
                int piecesToDrop = Random.Range(0, smallPieces.Length);
                for (int i = 0; i < piecesToDrop; i++)
                {
                    int randomSmallPiece = Random.Range(0, smallPieces.Length);
                    Instantiate(smallPieces[randomSmallPiece], transform.position, transform.rotation);
                }
            }

            //drop items
            if(shouldDropItem)
            {
                float dropChance = Random.Range(0f, 100f);

                if(dropChance < itemDropPercent)
                {
                    int randomItem = Random.Range(0, itemsToDrop.Length);

                    Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
                }
            }
        }
    }


}
