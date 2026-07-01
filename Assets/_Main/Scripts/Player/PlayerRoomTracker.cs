using Unity.VisualScripting;
using UnityEngine;

public class PlayerRoomTracker : MonoBehaviour
{
    [SerializeField] private BackroomsGenerator _levelGenerator; // Must be assigned in Inspector

    private float roomSize;

    private Vector2Int currentRoomPos = new Vector2Int(int.MinValue, int.MinValue); // Start current room as start room.
    private Vector3 _playerPosition;
    private Vector2Int roomPos = new Vector2Int(int.MinValue, int.MinValue);

    private void Awake()
    {
        roomSize = _levelGenerator.GetRoomSize();
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        _playerPosition = transform.position;

        roomPos = new (Mathf.RoundToInt(_playerPosition.x / roomSize), Mathf.RoundToInt(_playerPosition.z / roomSize));

        if (roomPos == currentRoomPos)
            return;

        currentRoomPos = roomPos;

        if(_levelGenerator.TryGetRoom(roomPos, out Room room))
        {
            RoomVisibilityManager.Instance.EnterRoom(room);
        }
    }
}
