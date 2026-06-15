using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{

    [SerializeField] public ItemData data;
    private Collider _collider;
    private AudioSource _audioSource;
    private MeshRenderer _meshRenderer;
    public string ActionName => "Pick up " + data.ItemName;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _audioSource = GetComponent<AudioSource>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Interact()
    {
        _collider.enabled = false;
        _meshRenderer.enabled = false;
        _audioSource.PlayOneShot(data.ItemPickupSound);
        GameManager.instance.ToggleKeyObtained();
        Destroy(this.gameObject, 1.5f);
    }
            


}
