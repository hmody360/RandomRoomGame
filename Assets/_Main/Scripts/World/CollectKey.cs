using UnityEngine;

public class CollectKey : MonoBehaviour
{
    private Collider _collider;
    private AudioSource _audioSource;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _audioSource = GetComponent<AudioSource>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _collider.enabled = false;
            _meshRenderer.enabled = false;
            _audioSource.Play();
            GameManager.instance.ToggleKeyObtained();
            Destroy(this.gameObject, 1.5f);
        }
    }


}
