using System.Collections.Generic;
using UnityEngine;

public class BackroomsGenerator : MonoBehaviour
{
    [Header("Settings")]
    public GameObject levelParent;
    public float roomSize;
    public int roomAmount;
    public float keyGroundOffset = 2;

    [Header("Prefabs")]
    public GameObject[] startingRooms;
    public GameObject[] connectingRooms;
    public GameObject[] endingRooms;
    public GameObject wallPrefab;
    public GameObject keyPrefab;

    private Dictionary<Vector2Int, GameObject> _grid = new Dictionary<Vector2Int, GameObject>();
    private List<(Vector2Int pos, Door door)> availableDoors = new List<(Vector2Int, Door)>();

    private void Start()
    {
        GenerateLevel();
        RemoveLeftoverDoors();
    }

    private void GenerateLevel()
    {
        Vector2Int startPos = Vector2Int.zero;
        GameObject startRoom = SpawnRoom(GetRandomPrefab(startingRooms), startPos);

        _grid.Add(startPos, startRoom);

        AddDoors(startPos, startRoom.GetComponentsInChildren<Door>());

        if(roomAmount < 3)
        {
            roomAmount = 3;
        }

        int connectingRoomsToAdd = roomAmount - 2;
        int KeyRoomIndex = Random.Range(1, connectingRoomsToAdd);

        while (connectingRoomsToAdd > 0 && availableDoors.Count > 0)
        {
            var openDoor = GetRandomFromList(availableDoors);
            availableDoors.Remove(openDoor);

            Vector2Int newPos = openDoor.pos + DirectionToVector(openDoor.door.direction);

            if (_grid.ContainsKey(newPos))
                continue;

            GameObject connectingRoom = SpawnRoom(GetRandomPrefab(connectingRooms), newPos);
            _grid.Add(newPos, connectingRoom);

            AddDoors(newPos, connectingRoom.GetComponentsInChildren<Door>());

            if(KeyRoomIndex == connectingRoomsToAdd)
            {
                Instantiate(keyPrefab, connectingRoom.transform.position + Vector3.up * keyGroundOffset, keyPrefab.transform.rotation, connectingRoom.transform);
            }

            connectingRoomsToAdd--;
        }

        while(availableDoors.Count > 0)
        {
            var openDoor = GetRandomFromList(availableDoors);
            availableDoors.Remove(openDoor);

            Vector2Int newPos = openDoor.pos + DirectionToVector(openDoor.door.direction);

            if(_grid.ContainsKey(newPos))
                continue;

            GameObject finalRoom = SpawnRoom(GetRandomPrefab(endingRooms), newPos);
            _grid.Add(newPos, finalRoom);

            AddDoors(newPos, finalRoom.GetComponentsInChildren<Door>());

            break;
        }
    }

    private void RemoveLeftoverDoors()
    {
        foreach ( var doorData in availableDoors)
        {
            Vector2Int newPos = doorData.pos + DirectionToVector(doorData.door.direction);

            if (_grid.ContainsKey(newPos))
                continue;

            GameObject doorObj = doorData.door.doorObj;

            Instantiate(wallPrefab, doorObj.transform.position, doorObj.transform.rotation, doorObj.transform.parent);
            Destroy(doorObj);
        }
    }


    // Helpers

    private GameObject SpawnRoom(GameObject prefab, Vector2Int gridPos)
    {
        Vector3 worldPostion = new Vector3(gridPos.x * roomSize, 0, gridPos.y * roomSize);

        return Instantiate(prefab, worldPostion, Quaternion.identity, levelParent.transform);
    }

    private void AddDoors(Vector2Int roomPos, Door[] doors)
    {

        foreach(Door door in doors)
        {
            Vector2Int newPos = roomPos + DirectionToVector(door.direction);

            if (!_grid.ContainsKey(newPos))
            {
                availableDoors.Add((roomPos, door));
            }
        }
    }

    private Vector2Int DirectionToVector(DoorDirection direction)
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

    private T GetRandomFromList<T>(IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    private GameObject GetRandomPrefab(GameObject[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
}
