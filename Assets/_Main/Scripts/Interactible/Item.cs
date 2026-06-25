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
        _collider.enabled = false;
        AddAndDisplayItemMessages();
       
    }

    public ItemData GetData()
    {
        return data;
    }

    private void AddAndDisplayItemMessages()
    {
        if (data.ItemMessages != null && data.ItemMessages.Count > 0)
        {
            Typewriter.EmptyMessageList();
            foreach (string message in data.ItemMessages)
            {
                if (message.Length > 0)
                {
                    if (message == data.ItemMessages[^1])
                    {
                        Typewriter.AddMessage(message, StartInteraction);
                    }
                    else
                    {
                        Typewriter.AddMessage(message);
                    }
                    
                }
            }

            Typewriter.Activate();
        }
        else
        {
          StartInteraction();
        }
    }

    private void StartInteraction()
    {
        if (data.ItemPickupSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(data.ItemPickupSound);
        }

        if (_itemAnimator != null && _interactionCoroutine == null && data.InteractionAnimation != null)
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
                _meshRenderer.enabled = false;
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().AddItem(this);
                Destroy(this.gameObject, 1.5f);
            }

            _collider.enabled = true;
        }
    }

    private IEnumerator InteractionEnum()
    {
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
