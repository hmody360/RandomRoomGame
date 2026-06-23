using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Create Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] private int itemId;
    [SerializeField] private string itemName;
    [SerializeField] private List<string> itemMessages;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private AudioClip itemPickupSound;
    [SerializeField] private AudioClip itemSelectSound;
    [SerializeField] private AnimationClip interactionAnimation;
    [SerializeField] private bool isPickable;

    public int ItemID => itemId;
    public string ItemName => itemName;
    public Sprite ItemIcon => itemIcon;
    public GameObject ItemPrefab => itemPrefab;
    public List<string> ItemMessages => itemMessages;
    public AudioClip ItemPickupSound => itemPickupSound;
    public AudioClip ItemSelectSound => itemSelectSound;
    public AnimationClip InteractionAnimation => interactionAnimation;
    public bool IsPickable => isPickable;

}
