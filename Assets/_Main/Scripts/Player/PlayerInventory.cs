using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    [SerializeField] private ItemData[] _inventoryList = new ItemData[3];
    [SerializeField] private int _currentlySelectedItemIndex;

    public ItemData[] GetInventoryList()
    {
        return _inventoryList;
    }

    public ItemData GetCurrentlySelectedItem()
    {
        return _inventoryList[_currentlySelectedItemIndex];
    }

    public void AddItem(Item itemToAdd)
    {
        if (_inventoryList.All(item => item != null))
        {
            ReplaceItem(itemToAdd);
        }
        else
        {
            for (int i = 0; i < _inventoryList.Length; i++)
            {
                if (_inventoryList[i] == null)
                {
                    _inventoryList[i] = itemToAdd.GetData();
                    break;
                }
            }
        }
    }

    private void ReplaceItem(Item itemToReplace)
    {
        ItemData ReplacedItem = _inventoryList[_currentlySelectedItemIndex];
        Instantiate(ReplacedItem.ItemPrefab, itemToReplace.gameObject.transform.parent);
        _inventoryList[_currentlySelectedItemIndex] = itemToReplace.GetData();
    }

}
