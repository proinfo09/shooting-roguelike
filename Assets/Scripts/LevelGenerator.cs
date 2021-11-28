using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;
    public int distanceToEnd;

    [Header("Shop Room")]
    public bool includeShop;
    public int minDistanceToShop, maxDistanceToShop;

    [Header("Chest Room")]
    public bool includeChest;
    public int minDistanceToChest, maxDistanceToChest;

    public Color startColor, endColor, shopColor, chestColor;
    public Transform generatorPoint;
    public enum Direction { up, right, down, left};
    public Direction selectedDirection;

    public float xOffset = 18, yOffset = 10;

    public LayerMask isRoom;

    GameObject endRoom, shopRoom, chestRoom;

    List<GameObject> layoutRoomObjects = new List<GameObject>();

    public RoomPrefabs rooms;

    List<GameObject> generatedOutlines = new List<GameObject>();

    public RoomCenter centerStart, centerEnd, centerShop, centerChest;
    public RoomCenter[] potentialCenters;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;
        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();
        for(int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);
            layoutRoomObjects.Add(newRoom);
            if(i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                endRoom = newRoom;
            }
            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();
            while(Physics2D.OverlapCircle(generatorPoint.position, 0.2f, isRoom))
            {
                MoveGenerationPoint();
            }
        }

        if (includeShop)
        {
            int shopSelector = Random.Range(minDistanceToShop, maxDistanceToShop + 1);
            shopRoom = layoutRoomObjects[shopSelector];
            layoutRoomObjects.RemoveAt(shopSelector);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }

        if (includeChest)
        {
            int chestSelector = Random.Range(minDistanceToChest, maxDistanceToChest + 1);
            chestRoom = layoutRoomObjects[chestSelector];
            layoutRoomObjects.RemoveAt(chestSelector);
            chestRoom.GetComponent<SpriteRenderer>().color = chestColor;
        }

        //create outline room
        CreateRoomOutline(Vector3.zero);
        foreach(GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }
        CreateRoomOutline(endRoom.transform.position);

        if (includeShop)
        {
            CreateRoomOutline(shopRoom.transform.position);
        }

        if (includeChest)
        {
            CreateRoomOutline(chestRoom.transform.position);
        }

        foreach (GameObject outline in generatedOutlines)
        {
            bool generateCenter = true;
            if(outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }
            if(outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }
            if (includeShop)
            {
                if (outline.transform.position == shopRoom.transform.position)
                {
                    Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }
            if (includeChest)
            {
                if (outline.transform.position == chestRoom.transform.position)
                {
                    Instantiate(centerChest, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }
            if (generateCenter)
            {
                int centerSeclect = Random.Range(0, potentialCenters.Length);
                Instantiate(potentialCenters[centerSeclect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }

    public void MoveGenerationPoint()
    {
        switch (selectedDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;
            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;
            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;
            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }

    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), .2f, isRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), .2f, isRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), .2f, isRoom);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), .2f, isRoom);

        int directionCount = 0;
        if (roomAbove)
        {
            directionCount++;
        }
        if (roomLeft)
        {
            directionCount++;
        }
        if (roomRight)
        {
            directionCount++;
        }
        if (roomBelow)
        {
            directionCount++;
        }
        switch (directionCount)
        {
            case 0:
                Debug.LogError("Found No Room Exists!!");
                break;
            case 1:
                if (roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                }
                if (roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                }
                if (roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                }
                if (roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                }
                break;
            case 2:
                if(roomAbove && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                }
                if (roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
                }
                if (roomAbove && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpLeft, roomPosition, transform.rotation));
                }
                if (roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                }
                if (roomLeft && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftDown, roomPosition, transform.rotation));
                }
                if (roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleRightDown, roomPosition, transform.rotation));
                }
                break;
            case 3:
                if(roomAbove && roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpLeftRight, roomPosition, transform.rotation));
                }
                if (roomAbove && roomLeft && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpLeftDown, roomPosition, transform.rotation));
                }
                if (roomAbove && roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, roomPosition, transform.rotation));
                }
                if (roomLeft && roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleLeftRightDown, roomPosition, transform.rotation));
                }
                break;
            case 4:
                generatedOutlines.Add(Instantiate(rooms.fourWay, roomPosition, transform.rotation));
                break;
        }
    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, singleRight, singleLeft, singleDown,
        doubleUpDown, doubleUpLeft, doubleUpRight, doubleLeftRight, doubleLeftDown, doubleRightDown,
        tripleUpLeftRight, tripleUpLeftDown, tripleUpRightDown, tripleLeftRightDown,
        fourWay;
}





//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class LevelGenerator : MonoBehaviour
//{
//    public GameObject layoutRoom;
//    public int distanceToEnd;

//    [Header("Shop Room")]
//    public bool includeShop;
//    public int minDistanceToShop, maxDistanceToShop;

//    [Header("Chest Room")]
//    public bool includeChest;
//    public int minDistanceToChest, maxDistanceToChest;

//    public Color startColor, endColor, shopColor, chestColor;
//    public Transform generatorPoint;
//    public enum Direction { up, right, down, left };
//    public Direction selectedDirection;

//    public float xOffset = 18, yOffset = 10;

//    public LayerMask isRoom;

//    GameObject endRoom, shopRoom, chestRoom;

//    List<GameObject> layoutRoomObjects = new List<GameObject>();

//    public RoomPrefabs rooms;

//    List<GameObject> generatedOutlines = new List<GameObject>();

//    public RoomCenter centerStart, centerEnd, centerShop, centerChest;
//    public RoomCenter[] potentialCenters;

//    // Start is called before the first frame update
//    void Start()
//    {
//        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;
//        selectedDirection = (Direction)Random.Range(0, 4);
//        MoveGenerationPoint();
//        for (int i = 0; i < distanceToEnd; i++)
//        {
//            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);
//            layoutRoomObjects.Add(newRoom);
//            if (i + 1 == distanceToEnd)
//            {
//                newRoom.GetComponent<SpriteRenderer>().color = endColor;
//                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
//                endRoom = newRoom;
//            }
//            selectedDirection = (Direction)Random.Range(0, 4);
//            MoveGenerationPoint();
//            while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, isRoom))
//            {
//                MoveGenerationPoint();
//            }
//        }

//        if (includeShop)
//        {
//            int shopSelector = Random.Range(minDistanceToShop, maxDistanceToShop + 1);
//            shopRoom = layoutRoomObjects[shopSelector];
//            layoutRoomObjects.RemoveAt(shopSelector);
//            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
//        }

//        if (includeChest)
//        {
//            int chestSelector = Random.Range(minDistanceToChest, maxDistanceToChest + 1);
//            chestRoom = layoutRoomObjects[chestSelector];
//            layoutRoomObjects.RemoveAt(chestSelector);
//            chestRoom.GetComponent<SpriteRenderer>().color = chestColor;
//        }

//        //create outline room
//        CreateRoomOutline(Vector3.zero);
//        foreach (GameObject room in layoutRoomObjects)
//        {
//            CreateRoomOutline(room.transform.position);
//        }
//        CreateRoomOutline(endRoom.transform.position);

//        if (includeShop)
//        {
//            CreateRoomOutline(shopRoom.transform.position);
//        }

//        if (includeChest)
//        {
//            CreateRoomOutline(chestRoom.transform.position);
//        }

//        foreach (GameObject outline in generatedOutlines)
//        {
//            bool generateCenter = true;
//            if (outline.transform.position == Vector3.zero)
//            {
//                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
//                generateCenter = false;
//            }
//            if (outline.transform.position == endRoom.transform.position)
//            {
//                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
//                generateCenter = false;
//            }
//            if (includeShop)
//            {
//                if (outline.transform.position == shopRoom.transform.position)
//                {
//                    Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
//                    generateCenter = false;
//                }
//            }
//            if (includeChest)
//            {
//                if (outline.transform.position == chestRoom.transform.position)
//                {
//                    Instantiate(centerChest, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
//                    generateCenter = false;
//                }
//            }
//            if (generateCenter)
//            {
//                int centerSeclect = Random.Range(0, potentialCenters.Length);
//                Instantiate(potentialCenters[centerSeclect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
//            }
//        }

//    }

//    // Update is called once per frame
//    void Update()
//    {
//#if UNITY_EDITOR
//        if (Input.GetKeyDown(KeyCode.R))
//        {
//            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//        }
//#endif
//    }

//    public void MoveGenerationPoint()
//    {
//        switch (selectedDirection)
//        {
//            case Direction.up:
//                generatorPoint.position += new Vector3(0f, yOffset, 0f);
//                break;
//            case Direction.right:
//                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
//                break;
//            case Direction.down:
//                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
//                break;
//            case Direction.left:
//                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
//                break;
//        }
//    }

//    public void CreateRoomOutline(Vector3 roomPosition)
//    {
//        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), .2f, isRoom);
//        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), .2f, isRoom);
//        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), .2f, isRoom);
//        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), .2f, isRoom);

//        int directionCount = 0;
//        if (roomAbove)
//        {
//            directionCount++;
//        }
//        if (roomLeft)
//        {
//            directionCount++;
//        }
//        if (roomRight)
//        {
//            directionCount++;
//        }
//        if (roomBelow)
//        {
//            directionCount++;
//        }
//        switch (directionCount)
//        {
//            case 0:
//                Debug.LogError("Found No Room Exists!!");
//                break;
//            case 1:
//                if (roomAbove)
//                {
//                    generatedOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
//                }
//                if (roomLeft)
//                {
//                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
//                }
//                if (roomRight)
//                {
//                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
//                }
//                if (roomBelow)
//                {
//                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
//                }
//                break;
//            case 2:
//                if (roomAbove && roomBelow)
//                {
//                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
//                }
//                if (roomLeft && roomRight)
//                {
//                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
//                }
//                if (roomAbove && roomLeft)
//                {
//                    generatedOutlines.Add(Instantiate(rooms.doubleUpLeft, roomPosition, transform.rotation));
//                }
//                if (roomAbove && roomRight)
//                {
//                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
//                }
//                if (roomLeft && roomBelow)
//                {
//                    generatedOutlines.Add(Instantiate(rooms.doubleLeftDown, roomPosition, transform.rotation));
//                }
//                if (roomRight && roomBelow)
//                {
//                    generatedOutlines.Add(Instantiate(rooms.doubleRightDown, roomPosition, transform.rotation));
//                }
//                break;
//            case 3:
//                if (roomAbove && roomLeft && roomRight)
//                {
//                    generatedOutlines.Add(Instantiate(rooms.tripleUpLeftRight, roomPosition, transform.rotation));
//                }
//                if (roomAbove && roomLeft && roomBelow)
//                {
//                    generatedOutlines.Add(Instantiate(rooms.tripleUpLeftDown, roomPosition, transform.rotation));
//                }
//                if (roomAbove && roomRight && roomBelow)
//                {
//                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, roomPosition, transform.rotation));
//                }
//                if (roomLeft && roomRight && roomBelow)
//                {
//                    generatedOutlines.Add(Instantiate(rooms.tripleLeftRightDown, roomPosition, transform.rotation));
//                }
//                break;
//            case 4:
//                generatedOutlines.Add(Instantiate(rooms.fourWay, roomPosition, transform.rotation));
//                break;
//        }
//    }
//}

//[System.Serializable]
//public class RoomPrefabs
//{
//    public GameObject singleUp, singleRight, singleLeft, singleDown,
//        doubleUpDown, doubleUpLeft, doubleUpRight, doubleLeftRight, doubleLeftDown, doubleRightDown,
//        tripleUpLeftRight, tripleUpLeftDown, tripleUpRightDown, tripleLeftRightDown,
//        fourWay;
//}
