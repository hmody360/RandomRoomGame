using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{

    [SerializeField] private ItemData data;

    private Collider _collider;
    private AudioSource _audioSource;
    private MeshRenderer _meshRenderer;
    private Animator _itemAnimator;
    private Coroutine _interactionCoroutine;
    public string ActionName => ((data.IsPickable) ? "Pick up " : "Look At " ) + data.ItemName;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _audioSource = GetComponent<AudioSource>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _itemAnimator = GetComponent<Animator>();
    }

    public void Interact()
    {

        if (data.ItemDescription != null && data.ItemDescription.Length > 0)
        {
            UIManager.instance.StartPromptCoroutine(3 ,data.ItemDescription);
        }

        if (data.ItemPickupSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(data.ItemPickupSound);
        }

        if(_itemAnimator != null && _interactionCoroutine == null && data.InteractionAnimation != null)
        {
            Debug.Log("Yes Animation Interaction");
            _interactionCoroutine = StartCoroutine(InteractionEnum());
            _itemAnimator.SetTrigger("Interact");
        }
        else
        {
            Debug.Log("No Animation Interaction");
            if (data.IsPickable)
            {
                _collider.enabled = false;
                _meshRenderer.enabled = false;
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().AddItem(this);
                Destroy(this.gameObject, 1.5f);
            }
        }
       
    }

    public ItemData GetData()
    {
        return data;
    }

    private IEnumerator InteractionEnum()
    {
        _collider.enabled = false;

        yield return new WaitForSeconds(data.InteractionAnimation.length);

        if (data.IsPickable)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().AddItem(this);
            Destroy(this.gameObject);
        }
        else
        {
            _collider.enabled = true;
            _interactionCoroutine = null;
        }

    }

}
