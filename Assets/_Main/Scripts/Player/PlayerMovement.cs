using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speeds")]
    public float speed;
    public float jumpForce;
    public float rotationSpeed;

    [Header("Ground Checking")]
    [SerializeField] private float _groundCheckerOffset = -0.9f;
    [SerializeField] private float _groundCheckerRadius = 0.3f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Indicators")]
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isWalking;

    [Header("AudioSources")]
    [SerializeField] AudioSource WalkAS;
    [SerializeField] AudioSource JumpAS;


    private Rigidbody _playerRB;
    private PlayerInputHandler _input;
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
    }

    private void OnEnable()
    {
        _input.OnJump += HandleJump;
    }

    private void OnDisable()
    {
        _input.OnJump -= HandleJump;
    }


    private void HandleJump()//check if jump button is pressed and player is on ground
    {
        if(_isGrounded)
        {
            _playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            JumpAS.PlayOneShot(JumpAS.clip);
        }

    }

    private void moveAndRotate() //Movement Logic
    {
        //Movement Axis Changing
        Vector2 input = _input.MoveInput;

        _isWalking = _input.MoveInput.sqrMagnitude > 0.01f && _isGrounded;

        if (_isWalking && !WalkAS.isPlaying)
        {
            WalkAS.Play();
        }else if (!_isWalking && WalkAS.isPlaying)
        {
            WalkAS.Stop();
        }

       //Realtive Camera Movement with rotation

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

    private void CheckGround() //Check if player is grounded
    {
        _isGrounded = Physics.CheckSphere(transform.position + Vector3.up * _groundCheckerOffset, _groundCheckerRadius, _groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * _groundCheckerOffset, _groundCheckerRadius);
    }
}
