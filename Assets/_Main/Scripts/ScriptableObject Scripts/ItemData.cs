using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Create Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] private int itemId;
    [SerializeField] private string itemName;
    [SerializeField] private Image itemIcon;
    [SerializeField] private AudioClip itemPickupSound;

    public int ItemID => itemId;
    public string ItemName => itemName;
    public Image ItemIcon => itemIcon;
    public AudioClip ItemPickupSound => itemPickupSound;

}
