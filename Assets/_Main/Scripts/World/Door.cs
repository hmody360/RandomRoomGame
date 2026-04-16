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
    public GameObject doorObj;

    private void Awake()
    {
        doorObj = gameObject;
    }
}

