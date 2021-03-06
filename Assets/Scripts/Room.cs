using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public static Room instance;
    public bool closeWhenEntered/*, openWhenCleared*/;

    public GameObject[] doors;

    //public List<GameObject> enemies = new List<GameObject>();

    [HideInInspector]
    public bool roomActive;

    public GameObject mapHider;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(enemies.Count > 0 && roomActive && openWhenCleared)
        //{
        //    for(int i = 0; i < enemies.Count; i++)
        //    {
        //        if (enemies[i] == null)
        //        {
        //            enemies.RemoveAt(i);
        //            i--;
        //        }
        //    }

        //    if(enemies.Count == 0)
        //    {
        //        foreach (GameObject door in doors)
        //        {
        //            door.SetActive(false);
        //            closeWhenEntered = false;
        //        }
        //    }
        //}

        //if (enemies.Count == 0)
        //{
        //    foreach (GameObject door in doors)
        //    {
        //        door.SetActive(false);
        //        closeWhenEntered = false;
        //    }
        //}
    }

    public void OpenDoor()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
            closeWhenEntered = false;
            UIController.instance.mapDisplay.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            CameraController.instance.ChangeTarget(transform);
            if (closeWhenEntered)
            {
                foreach(GameObject door in doors)
                {
                    door.SetActive(true);
                }
                UIController.instance.mapDisplay.SetActive(false);
            }

            roomActive = true;

            mapHider.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            roomActive = false;

        }
    }
}
