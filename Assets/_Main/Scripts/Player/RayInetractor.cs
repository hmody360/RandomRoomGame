using UnityEngine;
using UnityEngine.InputSystem;

public class RayInetractor : MonoBehaviour
{
    private Camera _camera;
    private Ray _rayToCast;

    private IInteractable _currentInteractible;
    private PlayerInputHandler _input;

    [SerializeField] private Outline _lastHitOutline;
    [SerializeField] private float _maxDistance = 6f;
    [SerializeField] LayerMask _interactible;

    private void OnEnable()
    {
        _input.OnInteract += Interact;
    }

    private void OnDisable()
    {
        _input.OnInteract -= Interact;
    }

    void Awake()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        _rayToCast = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f)); //Create a RayCast from the center of the camera

        if (Physics.Raycast(_rayToCast, out RaycastHit hit, _maxDistance, _interactible)) //if the ray cast hits an interactible object display an outline, if the player interacts, do the interaction set for this item
        {
            GameObject currentGameObject = hit.collider.gameObject;
            Outline currentOutlined = currentGameObject.GetComponent<Outline>();
            IInteractable item = currentGameObject.transform.GetComponent<IInteractable>();

            if (currentOutlined != _lastHitOutline) //Disable outlines once not looking at the current item
            {
                DisableCurrentOutline();
                _lastHitOutline = currentOutlined;
                EnableCurrentOutline();
            }


            _currentInteractible = item;
        }
        else
        {
            _currentInteractible = null;
            DisableCurrentOutline();
        }
    }

    private void Interact()
    {
        if (_currentInteractible != null)
        {
            _currentInteractible.Interact();
        }
        Debug.Log("Looking at:" + _currentInteractible);
    }


    private void DisableCurrentOutline()
    {
        if(_lastHitOutline != null)
        {
            _lastHitOutline.enabled = false;
            _lastHitOutline = null;
        }
    }

    private void EnableCurrentOutline()
    {
        if(_lastHitOutline != null)
        {
            _lastHitOutline.enabled = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_rayToCast);

    }
}
