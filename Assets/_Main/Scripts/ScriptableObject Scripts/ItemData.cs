using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Create Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] private int itemId;
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private AudioClip itemPickupSound;
    [SerializeField] private AnimationClip interactionAnimation;
    [SerializeField] private bool isPickable;

    public int ItemID => itemId;
    public string ItemName => itemName;
    public Sprite ItemIcon => itemIcon;
    public GameObject ItemPrefab => itemPrefab;
    public string ItemDescription => itemDescription;
    public AudioClip ItemPickupSound => itemPickupSound;
    public AnimationClip InteractionAnimation => interactionAnimation;
    public bool IsPickable => isPickable;

}
