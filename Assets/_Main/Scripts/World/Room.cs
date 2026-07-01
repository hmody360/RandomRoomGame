using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    [SerializeField] private Renderer[] renderers;
    public List<Room> Neighbours = new List<Room>();

    public void SetVisible(bool visible)
    {
        foreach (Renderer renderer in renderers)
        {

            renderer.enabled = visible; 
        }
    }

    public void CacheRenderers()
    {
        renderers = GetComponentsInChildren<Renderer>(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        else
        {
            RoomVisibilityManager.Instance.EnterRoom(this);
        }
    }
}
