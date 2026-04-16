using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Settings")]
    #region Level Settings
    public GameObject levelRoomsParent;
    #region Level Prefabs
    [SerializeField] private GameObject[] startingRooms;
    [SerializeField] private GameObject[] connectingRooms;
    [SerializeField] private GameObject[] endingRooms;
    [SerializeField] private GameObject[] nextSceneRooms;

    #endregion
    public int maximumTotalRooms = 3;
    //public bool deleteExtraDoors;
    #endregion

    #region Private Variables
    private int remainingRooms;
    private GameObject spawnedStartingRoom;
    private List<GameObject> spawnedInbetweenRooms;
    private GameObject spawnedNextSceneRoom;
    private List<GameObject> doorsList;
    private int pickedIndex = 0;
    private bool foundOverlap = false;
    #endregion

    private void Awake()
    {
        // This is only here to avoid console errors if the prefabs are not assigned
        if (startingRooms == null)
            startingRooms = new GameObject[] { };
        if (connectingRooms == null)
            connectingRooms = new GameObject[] { };
        if (endingRooms == null)
            endingRooms = new GameObject[] { };
        if (nextSceneRooms == null)
            nextSceneRooms = new GameObject[] { };
        
        // Check that the value is not less than the logical minimum of rooms
        if (maximumTotalRooms < 3)
            maximumTotalRooms = 3;

        // Initialise lists
        spawnedInbetweenRooms = new List<GameObject>();
        doorsList = new List<GameObject>();
    }

    private void Start()
    {
        // Track how many rooms remain to be added
        remainingRooms = maximumTotalRooms;

        // Start by spawning a starting room in the position and rotation of the "Level Rooms Parent" and child the room to it
        spawnedStartingRoom =  Instantiate(startingRooms[Mathf.RoundToInt( Random.Range(0, startingRooms.Length - 1) )], levelRoomsParent.transform.position, levelRoomsParent.transform.rotation, levelRoomsParent.transform);
        remainingRooms--;

        // Insert all doors in the spawned starting room to the list using a loop
        GameObject door;
        for (int i = 0; i < spawnedStartingRoom.transform.GetChild(0).childCount; i++)
        {
            door = spawnedStartingRoom.transform.GetChild(0).GetChild(i).gameObject;
            doorsList.Add(door);
            print( spawnedStartingRoom.name +"'s child: "+"\"" +door.name + "\" added");
        }
        door = null;

        // Start populating the connecting rooms using a loop
        GameObject room, spawnedRoom;
        int doorIndex = 0;

        // This variable is to make sure the starting room doors are all filled first
        int loop = 0;
        while (remainingRooms - 1 > 0)
        {
            print("Remaining rooms: "+remainingRooms);

            pickedIndex = Random.Range(0, doorsList.Count - 1);
            door = doorsList[ loop < 4? 0 : pickedIndex];
            doorsList.RemoveAt(loop < 4 ? 0 : pickedIndex);
            loop++;

            print(door.name +" removed");
            
            // Grabbing the opposite side door in the picked room
            switch (door.name.Substring(5))
            {
                default:
                case "N":
                    room = connectingRooms[Random.Range(0, connectingRooms.Length - 1)].transform.GetChild(0).GetChild(3).gameObject;
                    doorIndex = 3;
                    break;
                case "E":
                    room = connectingRooms[Random.Range(0, connectingRooms.Length - 1)].transform.GetChild(0).GetChild(2).gameObject;
                    doorIndex = 2;
                    break;
                case "W":
                    room = connectingRooms[Random.Range(0, connectingRooms.Length - 1)].transform.GetChild(0).GetChild(1).gameObject;
                    doorIndex = 1;
                    break;
                case "S":
                    room = connectingRooms[Random.Range(0, connectingRooms.Length - 1)].transform.GetChild(0).GetChild(0).gameObject;
                    doorIndex = 0;
                    break;
            }

            print("Selected spawned room: " +room.name);

            foreach (GameObject listedRoom in spawnedInbetweenRooms)
            {
                if (door.transform.position - room.transform.position == listedRoom.transform.position)
                    foundOverlap = true;
            }

            if (foundOverlap)
            {
                foundOverlap = false;
                continue;
            }

            // Spawn the room
            spawnedRoom = Instantiate(room.transform.parent.parent.gameObject, door.transform.position - room.transform.position, door.transform.parent.parent.rotation, door.transform.parent.parent);
            spawnedInbetweenRooms.Add(spawnedRoom);

            // Add the spawned room's doors to the list
            doorsList.Add(spawnedRoom.transform.GetChild(0).GetChild( (doorIndex + 1) % 4).gameObject);
            doorsList.Add(spawnedRoom.transform.GetChild(0).GetChild( (doorIndex + 2) % 4).gameObject);
            doorsList.Add(spawnedRoom.transform.GetChild(0).GetChild( (doorIndex + 3) % 4).gameObject);


            // Decrease the loop check so it eventually breaks out of loop
            remainingRooms--;
        }

        // Choose a door for the Next Scene Room
        pickedIndex = Random.Range(0, doorsList.Count - 1);
        door = doorsList[pickedIndex];
        doorsList.RemoveAt(pickedIndex);

        // Grab opposite door
        switch (door.name.Substring(5))
        {
            default:
            case "N":
                room = nextSceneRooms[Random.Range(0, connectingRooms.Length - 1)].transform.GetChild(0).GetChild(3).gameObject;
                doorIndex = 3;
                break;
            case "E":
                room = nextSceneRooms[Random.Range(0, connectingRooms.Length - 1)].transform.GetChild(0).GetChild(2).gameObject;
                doorIndex = 2;
                break;
            case "W":
                room = nextSceneRooms[Random.Range(0, connectingRooms.Length - 1)].transform.GetChild(0).GetChild(1).gameObject;
                doorIndex = 1;
                break;
            case "S":
                room = nextSceneRooms[Random.Range(0, connectingRooms.Length - 1)].transform.GetChild(0).GetChild(0).gameObject;
                doorIndex = 0;
                break;
        }
        // Spawn Next Scene Room
        spawnedNextSceneRoom = Instantiate(nextSceneRooms[Random.Range(0, connectingRooms.Length - 1)], door.transform.position - room.transform.position, door.transform.parent.parent.rotation, door.transform.parent.parent);

        // Cleanup Next Scene Room's doors
        doorsList.Add(spawnedNextSceneRoom.transform.GetChild(0).GetChild((doorIndex + 1) % 4).gameObject);
        doorsList.Add(spawnedNextSceneRoom.transform.GetChild(0).GetChild((doorIndex + 2) % 4).gameObject);
        doorsList.Add(spawnedNextSceneRoom.transform.GetChild(0).GetChild((doorIndex + 3) % 4).gameObject);

        // Cleanup unused doors
        foreach (GameObject d in doorsList)
        {
            GameObject.Destroy(d);

        }
    }
}
