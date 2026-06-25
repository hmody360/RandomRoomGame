using UnityEngine;

public class RayInteractor : MonoBehaviour
{
    private static Vector3 ViewportCenter = new Vector3(0.5f, 0.5f);

    private Camera _camera;
    private PlayerInputHandler _input;

    private IInteractable _currentInteractable;
    private Outline _currentOutline;

    [SerializeField] private float _maxDistance = 6f;
    [SerializeField] private LayerMask _interactableMask;

    private void Awake()
    {
        _camera = Camera.main;
        _input = GetComponent<PlayerInputHandler>();
    }

    private void OnEnable()
    {
        _input.OnInteract += Interact;
    }

    private void OnDisable()
    {
        _input.OnInteract -= Interact;
    }

    private void Update()
    {
        Ray ray = _camera.ViewportPointToRay(ViewportCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _interactableMask))
        {
            GameObject hitObject = hit.collider.gameObject;

            hitObject.TryGetComponent(out IInteractable interactable);
            hitObject.TryGetComponent(out Outline outline);

            // Update if target changed
            if (interactable != _currentInteractable)
            {
                ClearCurrentInteractable();

                _currentInteractable = interactable;
                _currentOutline = outline;

                if (_currentOutline != null)
                    _currentOutline.enabled = true;

                if (_currentInteractable != null)
                    UIManager.instance.ShowInteractText(_currentInteractable.ActionName);
            }
        }
        else
        {
            ClearCurrentInteractable();
        }
    }

    private void Interact()
    {
        _currentInteractable?.Interact();
    }

    private void ClearCurrentInteractable()
    {
        if (_currentOutline != null)
        {
            _currentOutline.enabled = false;
            _currentOutline = null;
        }

        if (_currentInteractable != null)
        {
            UIManager.instance.HideInteractText();
            _currentInteractable = null;
        }
    }
}