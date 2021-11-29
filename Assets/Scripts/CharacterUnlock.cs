using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlock : MonoBehaviour
{
    bool canUnlock;
    public GameObject message;

    public CharacterSelector[] charSelects;
    CharacterSelector playerToUnlock;

    public SpriteRenderer cageSR;
    // Start is called before the first frame update
    void Start()
    {
        playerToUnlock = charSelects[Random.Range(0, charSelects.Length)];

        cageSR.sprite = playerToUnlock.playerToSpawn.bodySR.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (canUnlock)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerPrefs.SetInt(playerToUnlock.playerToSpawn.name, 1);
                Instantiate(playerToUnlock, transform.position, transform.rotation);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canUnlock = true;
            message.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canUnlock = false;
            message.SetActive(false);
        }
    }
}
