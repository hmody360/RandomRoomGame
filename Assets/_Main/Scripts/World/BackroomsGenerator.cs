using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;

public class BackroomsGenerator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject levelParent;
    [SerializeField] private float roomSize;
    [SerializeField] private int roomAmount;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] startingRooms;
    [SerializeField] private GameObject[] connectingRooms;
    [SerializeField] private GameObject[] endingRooms;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private GameObject[] randomItemPrefabs;

    private Dictionary<Vector2Int, Room> _grid = new();
    private List<(Vector2Int pos, Door door)> availableDoors = new List<(Vector2Int, Door)>();
    private List<ItemSpot> availableItemSpots = new List<ItemSpot>();
    private NavMeshSurface _levelNavMeshSurface;

    private void Awake()
    {
        if(GameManager.instance != null)
        {
            roomAmount = GameManager.instance.GetNoOfRoomsToGenerate();
        }
        GenerateLevel();
    }

    private void Start()
    {
        
    }

    private void GenerateLevel() //Generate Start, Connecting and Ending Rooms in Order
    {
        //Begin from the center of the grid
        Vector2Int startPos = Vector2Int.zero;
        GameObject startRoomObj = SpawnRoom(GetRandomPrefab(startingRooms), startPos);

        _levelNavMeshSurface = startRoomObj.GetComponent<NavMeshSurface>(); // Get the NavMeshSurface from the first room

        Room startRoom = startRoomObj.GetComponent<Room>();

        _grid.Add(startPos, startRoom); //Add the room using its vectorial postion and the room's component

        RoomVisibilityManager.Instance.RegisterRoom(startRoom);

        AddDoors(startPos, startRoomObj.GetComponentsInChildren<Door>()); //add the room's doors to have them checked later for other rooms

        if (roomAmount < 3) //reset to the least logical room amount
        {
            roomAmount = 3;
        }

        int connectingRoomsToAdd = roomAmount - 2; //connecting rooms minus the start and ending rooms

        while (connectingRoomsToAdd > 0 && availableDoors.Count > 0) //Start Adding connecting rooms by selecting a random door, removing it from the list
        {
            var openDoor = GetRandomFromList(availableDoors);
            availableDoors.Remove(openDoor);

            Vector2Int newPos = openDoor.pos + DirectionToVector(openDoor.door.direction);

            if (_grid.ContainsKey(newPos)) //checking if there's a room in the grid spot (if yes restart loop and check for a different door, if no create the room)
                continue;


            GameObject connectingRoomObj = SpawnRoom(GetRandomPrefab(connectingRooms), newPos); //Spawn the new room by selecting a random room and filling the grid spot (then adding it to the grid)

            Room connectingRoom = connectingRoomObj.GetComponent<Room>();

            _grid.Add(newPos, connectingRoom);

            RoomVisibilityManager.Instance.RegisterRoom(connectingRoom);

            AddDoors(newPos, connectingRoomObj.GetComponentsInChildren<Door>()); //Add the doors of this new room to the door list.

            AddItemSpots(connectingRoomObj.GetComponentsInChildren<ItemSpot>()); //Add the itemspots of this new room to use later for item addition

            connectingRoomsToAdd--; //After adding the room decrease the counter
        }

        while (availableDoors.Count > 0) //Adding the final room in an available grid spot.
        {
            var openDoor = GetRandomFromList(availableDoors);
            availableDoors.Remove(openDoor);

            Vector2Int newPos = openDoor.pos + DirectionToVector(openDoor.door.direction);

            if (_grid.ContainsKey(newPos))
                continue;

            GameObject finalRoomObj = SpawnRoom(GetRandomPrefab(endingRooms), newPos);

            Room finalRoom = finalRoomObj.GetComponent<Room>();

            _grid.Add(newPos, finalRoom);
            RoomVisibilityManager.Instance.RegisterRoom(finalRoom);

            AddDoors(newPos, finalRoomObj.GetComponentsInChildren<Door>());

            break;
        }

        //Placing the Key at a random spot
        if (keyPrefab != null)
        {
            PlaceItemRandom(keyPrefab);
        }

        //Place Random Items
        if (randomItemPrefabs != null && randomItemPrefabs.Length > 0)
        {
            foreach (GameObject obj in randomItemPrefabs)
            {
                if (obj != null)
                {
                    PlaceItemRandom(obj);
                }

            }
        }

        ConnectRooms();

        //Baking the level's navmesh after its creation
        if(_levelNavMeshSurface != null)
        {
            _levelNavMeshSurface.BuildNavMesh();
        }

        RemoveLeftoverDoors();

        CacheAllRoomRenderers();

        RoomVisibilityManager.Instance.EnterRoom(startRoom);

    }

    private void RemoveLeftoverDoors() //Patch Doors that are not connected to a room by checking if the direction has no grid spot adjacent to it.
    {
        foreach ( var doorData in availableDoors)
        {
            Vector2Int newPos = doorData.pos + DirectionToVector(doorData.door.direction);

            if (_grid.ContainsKey(newPos))
                continue;

            GameObject doorObj = doorData.door.doorObj;

            Instantiate(wallPrefab, doorObj.transform.position, doorObj.transform.rotation, doorObj.transform.parent);
            DestroyImmediate(doorObj); //Immediately destroying the door to avoid including it within the cahced renders list for each room.
        }
    }


    // Helpers

    private GameObject SpawnRoom(GameObject prefab, Vector2Int gridPos) //Room spawner realtive to the grid postion * the room size.
    {
        Vector3 worldPostion = new Vector3(gridPos.x * roomSize, 0, gridPos.y * roomSize);

        return Instantiate(prefab, worldPostion, Quaternion.identity, levelParent.transform);
    }

    private void AddDoors(Vector2Int roomPos, Door[] doors) //Add the list of doors if the adjacnt grid spot is free for adding a room.
    {

        foreach (Door door in doors)
        {
            Vector2Int newPos = roomPos + DirectionToVector(door.direction);

            if (!_grid.ContainsKey(newPos))
            {
                availableDoors.Add((roomPos, door));
            }
        }
    }

    private void ConnectRooms()
    {
        foreach(var roomPair in _grid)
        {
            Vector2Int pos = roomPair.Key;

            Room room = roomPair.Value;

            CheckNeighbor(room, pos + Vector2Int.up);
            CheckNeighbor(room, pos + Vector2Int.down);
            CheckNeighbor(room, pos + Vector2Int.left);
            CheckNeighbor(room, pos + Vector2Int.right);
        }
    }

    private void CacheAllRoomRenderers()
    {
        foreach(Room room in _grid.Values)
        {
            room.CacheRenderers();
        }
    }

    private void CheckNeighbor(Room room, Vector2Int pos)
    {
        if(_grid.TryGetValue(pos, out Room neighbour))
        {
            room.Neighbours.Add(neighbour);
        }
    }

    private void AddItemSpots(ItemSpot[] itemSpots)
    {
        foreach(ItemSpot itemSpot in itemSpots)
        {
            availableItemSpots.Add(itemSpot);
        }
    }

    private void PlaceItemRandom(GameObject itemObj)
    {
        int SpotIndex = Random.Range(0, availableItemSpots.Count - 1);
        ItemSpot SpotToPlace = availableItemSpots[SpotIndex];
        Instantiate(itemObj, SpotToPlace.transform.position + (Vector3.up * 0.05f) ,itemObj.transform.rotation, SpotToPlace.transform);
        SpotToPlace.TriggerOccupation();
        availableItemSpots.RemoveAt(SpotIndex);
    }

    private Vector2Int DirectionToVector(DoorDirection direction) //Convert the door direction to a vector which translates its adjacent grid postion within this room.
    {
        switch (direction)
        {
            case DoorDirection.North:
                return new Vector2Int(0, 1);
            case DoorDirection.South:
                return new Vector2Int(0, -1);
            case DoorDirection.East:
                return new Vector2Int(1, 0);
            case DoorDirection.West:
                return new Vector2Int(-1, 0);
        }

        return Vector2Int.zero;
    }

    private T GetRandomFromList<T>(IList<T> list) //Get a random item from a generic list.
    {
        return list[Random.Range(0, list.Count)];
    }

    private GameObject GetRandomPrefab(GameObject[] array) //get a random item from an array of gameObjects.
    {
        return array[Random.Range(0, array.Length)];
    }

    public bool TryGetRoom(Vector2Int gridPosition, out Room room)
    {
        return _grid.TryGetValue(gridPosition, out room);
        
    }

    public float GetRoomSize()
    {
        return roomSize;
    }
}
