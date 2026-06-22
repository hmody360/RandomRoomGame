using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Locker : MonoBehaviour, IInteractable
{
    public string ActionName => "Open Locker";

    [SerializeField] private ItemData requiredItem;
    private Collider _collider;
    private PlayerInventory _pInventory;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _pInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    public void Interact()
    {
        ItemData currentItem = _pInventory.GetCurrentlySelectedItem();
        if (_pInventory != null && currentItem != null && currentItem.ItemID == requiredItem.ItemID)
        {
            _collider.enabled = false;
            _pInventory.RemoveCurrentItem();
            //Open Locker
        }
        else
        {
            //Prompt to find the key
        }
    }
}
            
