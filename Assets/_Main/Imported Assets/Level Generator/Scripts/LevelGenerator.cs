using System.Collections.Generic;
using UnityEngine;

public class GridLevelGenerator : MonoBehaviour
{
    [Header("Settings")]
    public Transform levelParent;
    public float roomSize = 20f;
    public int maxRooms = 6;

    [Header("Prefabs")]
    public GameObject[] startRooms;
    public GameObject[] connectingRooms;
    public GameObject[] endRooms;

    private Dictionary<Vector2Int, GameObject> grid = new Dictionary<Vector2Int, GameObject>();
    private List<(Vector2Int pos, DoorDirection dir)> openDoors = new List<(Vector2Int, DoorDirection)>();

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        Vector2Int startPos = Vector2Int.zero;

        // Spawn start room
        GameObject startRoom = SpawnRoom(GetRandom(startRooms), startPos);
        grid.Add(startPos, startRoom);

        AddDoors(startPos);

        int roomsToSpawn = maxRooms - 2;

        // Spawn connecting rooms
        while (roomsToSpawn > 0 && openDoors.Count > 0)
        {
            var doorData = GetRandom(openDoors);
            openDoors.Remove(doorData);

            Vector2Int newPos = doorData.pos + DirectionToVector(doorData.dir);

            if (grid.ContainsKey(newPos))
                continue;

            GameObject room = SpawnRoom(GetRandom(connectingRooms), newPos);
            grid.Add(newPos, room);

            AddDoors(newPos);

            roomsToSpawn--;
        }

        // Spawn end room
        while (openDoors.Count > 0)
        {
            var doorData = GetRandom(openDoors);
            openDoors.Remove(doorData);

            Vector2Int newPos = doorData.pos + DirectionToVector(doorData.dir);

            if (grid.ContainsKey(newPos))
                continue;

            GameObject endRoom = SpawnRoom(GetRandom(endRooms), newPos);
            grid.Add(newPos, endRoom);

            break;
        }
    }

    // =========================
    // 🔧 Helpers
    // =========================

    private GameObject SpawnRoom(GameObject prefab, Vector2Int gridPos)
    {
        Vector3 worldPos = new Vector3(gridPos.x * roomSize, 0, gridPos.y * roomSize);

        return Instantiate(prefab, worldPos, Quaternion.identity, levelParent);
    }

    private void AddDoors(Vector2Int roomPos)
    {
        foreach (DoorDirection dir in System.Enum.GetValues(typeof(DoorDirection)))
        {
            Vector2Int newPos = roomPos + DirectionToVector(dir);

            if (!grid.ContainsKey(newPos))
            {
                openDoors.Add((roomPos, dir));
            }
        }
    }

    private Vector2Int DirectionToVector(DoorDirection dir)
    {
        switch (dir)
        {
            case DoorDirection.North: return new Vector2Int(0, 1);
            case DoorDirection.South: return new Vector2Int(0, -1);
            case DoorDirection.East: return new Vector2Int(1, 0);
            case DoorDirection.West: return new Vector2Int(-1, 0);
        }

        return Vector2Int.zero;
    }

    private T GetRandom<T>(IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    private GameObject GetRandom(GameObject[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
}