using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerAnimState PlayerState
    {
        get => _playerState;
        private set
        {
            _playerState = value;
            PlayerStateChanged_notifier?.Invoke(this, EventArgs.Empty);
        }
    }
    public ShootAttack ShootAttack => _shootAttack;

    public event EventHandler PlayerStateChanged_notifier;

    [Header("Movement settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    [Header("Shoot settings")]
    [SerializeField] private ShootAttack _shootAttack;

    [Header("GroundChecker settings")]
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _checkerRadius;

    [Header("Visual settings")]
    [SerializeField] private PlayerAnimController _playerAnimController;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Debug")]
    [SerializeField] private bool _enabledDebugLog;
    [SerializeField] private int _jumpsLeft;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isFlipped;
    [SerializeField] private PlayerAnimState _playerState;

    private Rigidbody2D _myRB;
    private PlayerInput _input;

    private void Awake()
    {
        _input = new PlayerInput();
        _input.Player.Jump.performed += context => Jump();
        _input.Player.Shoot.performed += context => _shootAttack.Attack();

        _myRB = this.GetComponent<Rigidbody2D>();
        _playerState = PlayerAnimState.Idle;
        _jumpsLeft = 1;

        _shootAttack.TargetPosition = Vector2.right;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Move(_input.Player.Move.ReadValue<Vector2>());
        CheckAndSetAnimState();
    }

    private void Move(Vector2 direction)
    {
        if (Math.Abs(direction.x) > 0.2f)
        {
            HorizontalMove(direction.x);
            CheckAndFlip(direction.x);

            if (_isGrounded)
                _playerState = PlayerAnimState.Run;
        }
        else
        {
            if (_isGrounded)
                _playerState = PlayerAnimState.Idle;
            StopMove();
        }
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            _myRB.velocity = new Vector2(_myRB.velocity.x, _jumpForce);
            _jumpsLeft = 1;
            _playerState = PlayerAnimState.Jump;
        }
        else
        {
            if (_jumpsLeft > 0)
            {
                _myRB.velocity = new Vector2(_myRB.velocity.x, _jumpForce);
                _jumpsLeft--;
                _playerState = PlayerAnimState.DoubleJump;
            }
        }
    }

    private void HorizontalMove(float direction)
    {
        _myRB.velocity = new Vector2(direction * _speed, _myRB.velocity.y);
    }

    private void StopMove() 
    {
        _myRB.velocity = new Vector2(0, _myRB.velocity.y);
    }

    private void CheckAndFlip(float direction)
    {
        if (direction > 0 && _isFlipped)
        {
            _isFlipped = false;
            _spriteRenderer.flipX = false;

            _shootAttack.TargetPosition = Vector2.right;
        }
        else if (direction < 0 && _isFlipped == false)
        {
            _isFlipped = true;
            _spriteRenderer.flipX = true;

            _shootAttack.TargetPosition = Vector2.left;
        }
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapCircle(_groundChecker.position, _checkerRadius, _groundMask))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    private void CheckAndSetAnimState()
    {
        if (_isGrounded)
        {
            _playerAnimController.ChangeAnimation(_playerState);
        }
        else
        {
            if (_myRB.velocity.y < 0)
            {
                _playerState = PlayerAnimState.Fall;
                _playerAnimController.ChangeAnimation(_playerState);
            }
            else if (_myRB.velocity.y > 0 && _playerState != PlayerAnimState.DoubleJump)
            {
                _playerState = PlayerAnimState.Jump;
            }

            _playerAnimController.ChangeAnimation(_playerState);
        }
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    public enum PlayerAnimState
    {
        Idle,
        Run,
        Jump,
        DoubleJump,
        Fall,
        Hit
    }
}
