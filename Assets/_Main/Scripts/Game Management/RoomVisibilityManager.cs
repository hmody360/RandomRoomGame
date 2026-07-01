using System.Collections.Generic;
using UnityEngine;

public class RoomVisibilityManager : MonoBehaviour
{
    public static RoomVisibilityManager Instance;

    private readonly List<Room> allRooms = new List<Room>();

    [SerializeField] int VisibleDepth;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (GameManager.instance != null)
        {
            Instance.VisibleDepth = GameManager.instance.GetRenderDistance();
        }
    }

    public void RegisterRoom(Room room)
    {
        allRooms.Add(room);
    }

    public void EnterRoom(Room room)
    {
        UpdateVisibility(room);
    }

    public void UpdateVisibility(Room currentRoom)
    {
        foreach(Room room in allRooms)
        {
            room.SetVisible(false);
        }

        Queue<(Room room, int depth)> queue = new();
        HashSet<Room> visited = new();

        queue.Enqueue((currentRoom, 0));
        visited.Add(currentRoom);

        while( queue.Count > 0)
        {
            var current = queue.Dequeue();

            current.room.SetVisible(true);

            if(current.depth >= VisibleDepth)
            {
                continue;
            }

            foreach (Room neighbour in current.room.Neighbours)
            {
                if(visited.Contains(neighbour))
                    continue;

                visited.Add(neighbour);

                queue.Enqueue((neighbour, current.depth + 1));
            }
        }

    }

}
