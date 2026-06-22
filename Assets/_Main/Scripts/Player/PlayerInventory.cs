using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    [SerializeField] private ItemData[] _inventoryList = new ItemData[3];
    [SerializeField] private int _currentlySelectedItemIndex;

    private PlayerInputHandler _pInput;

    private void Awake()
    {
        _pInput = GetComponent<PlayerInputHandler>();
    }

    private void OnEnable()
    {
        _pInput.OnInventorySwitch += ChangeSlot;
    }

    private void OnDisable()
    {
        _pInput.OnInventorySwitch -= ChangeSlot;
    }

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
                    UIManager.instance.InsertItemIcon(i, itemToAdd.GetData().ItemIcon);
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
        UIManager.instance.InsertItemIcon(_currentlySelectedItemIndex, itemToReplace.GetData().ItemIcon);
    }

    public void RemoveCurrentItem()
    {
        _inventoryList[_currentlySelectedItemIndex] = null;
        UIManager.instance.RemoveItemIcon(_currentlySelectedItemIndex);
    }

    private void ChangeSlot()
    {
        switch (_currentlySelectedItemIndex)
        {
            case 0:
                _currentlySelectedItemIndex = 1;
                break;
            case 1:
                _currentlySelectedItemIndex = 2;
                break;
            case 2:
                _currentlySelectedItemIndex = 0;
                break;
            default:
                _currentlySelectedItemIndex = 0;
                break;

        }

        UIManager.instance.SelectItemSlot(_currentlySelectedItemIndex);
    }

}
