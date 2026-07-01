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
            if (renderer == null)
                continue;
            renderer.enabled = visible; 
        }
    }

    public void CacheRenderers()
    {
        renderers = GetComponentsInChildren<Renderer>(true);
    }
}
