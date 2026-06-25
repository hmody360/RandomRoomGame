using UnityEngine;

public class ItemSpot : MonoBehaviour
{
    [SerializeField] private bool isOccupied = false;

    public void TriggerOccupation()
    {
        isOccupied = !isOccupied;
    }

    public bool GetOccupation()
    {
        return isOccupied;
    }
}
