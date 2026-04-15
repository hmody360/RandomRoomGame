using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float rotationSpeed;

    [SerializeField] private float _groundCheckerOffset = -0.9f;
    [SerializeField] private float _groundCheckerRadius = 0.3f;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private bool _isGrounded;


    private Rigidbody _playerRB;
    private PlayerInputHandler _input;

    private Vector2 _moveInput;
    private Transform _cameraTransform;
    private Quaternion _targetRoation;
    private Vector3 _playerDirection;

    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInputHandler>();
        _cameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        _targetRoation = transform.rotation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CheckGround();
    }

    void FixedUpdate()
    {
        moveAndRotate();
        HandleJump();
    }


    private void HandleJump()
    {
        if(_input.JumpPressed && _isGrounded)
        {
            _playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        _input.ResetJump();
    }

    private void moveAndRotate()
    {
        //Movement Axis Changing
        Vector2 input = _input.MoveInput;

        //Realtive Camera Movement

        Vector3 CameraForward = _cameraTransform.forward;
        Vector3 CameraRight = _cameraTransform.right;

        CameraForward.y = 0f;
        CameraRight.y = 0f;

        Vector3 forwardRealtive = input.y * CameraForward;
        Vector3 rightRealtive = input.x * CameraRight;

        _playerDirection = (forwardRealtive + rightRealtive).normalized;

        Vector3 movementDir = _playerDirection * speed;

        _playerRB.linearVelocity = new Vector3(movementDir.x, _playerRB.linearVelocity.y, movementDir.z);

        if(_playerDirection != Vector3.zero)
        {
            _targetRoation = Quaternion.LookRotation(_playerDirection);
        }

        _playerRB.MoveRotation(Quaternion.Lerp(transform.rotation, _targetRoation, rotationSpeed * Time.fixedDeltaTime));
        
    }

    private void CheckGround()
    {
        _isGrounded = Physics.CheckSphere(transform.position + Vector3.up * _groundCheckerOffset, _groundCheckerRadius, _groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * _groundCheckerOffset, _groundCheckerRadius);
    }
}
