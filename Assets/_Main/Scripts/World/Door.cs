using UnityEngine;

public enum DoorDirection
{
    North,
    East,
    South,
    West
}

public class Door : MonoBehaviour
{
    public DoorDirection direction;
    public GameObject doorObj; //To destroy the door if not connected to a room

    private void Awake()
    {
        doorObj = gameObject;
    }
}

