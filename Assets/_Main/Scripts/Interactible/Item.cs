using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{

    [SerializeField] public ItemData data;
    private Collider _collider;
    private AudioSource _audioSource;
    private MeshRenderer _meshRenderer;
    public string ActionName => ((data.IsPickable) ? "Pick up " : "Look At " ) + data.ItemName;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _audioSource = GetComponent<AudioSource>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Interact()
    {
        if(data.IsPickable)
        {
            _collider.enabled = false;
            _meshRenderer.enabled = false;
            GameManager.instance.ToggleKeyObtained();
            Destroy(this.gameObject, 1.5f);
        }

        if (data.ItemDescription != null && data.ItemDescription.Length > 0)
        {
            UIManager.instance.StartPromptCoroutine(3 ,data.ItemDescription);
        }

        if (data.ItemPickupSound != null)
        {
            _audioSource.PlayOneShot(data.ItemPickupSound);
        }
       
    }
            


}
