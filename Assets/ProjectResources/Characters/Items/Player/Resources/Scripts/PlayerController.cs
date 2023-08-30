using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    [Header("Shoot settings")]
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private GameObject _bullet_prefab;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private int _maxStockBullet;
    [SerializeField] private float _reloadTime;

    [Header("GroundChecker settings")]
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _checkerRadius;

    [Header("Visual settings")]
    [SerializeField] private PlayerAnimController _playerAnimController;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Debug")]
    [SerializeField] private bool _enabledDebugLog;
    [SerializeField] private int _bulletLeft;
    [SerializeField] private int _jumpsLeft;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isFlipped;
    [SerializeField] private PlayerAnimState _playerState;

    private Rigidbody2D _myRB;
    private PlayerInput _input;

    private Coroutine _reloadCoroutine;

    private void Awake()
    {
        _input = new PlayerInput();
        _input.Player.Jump.performed += context => Jump();
        _input.Player.Shoot.performed += context => Shoot();

        _myRB = this.GetComponent<Rigidbody2D>();
        _playerState = PlayerAnimState.Idle;
        _jumpsLeft = 1;
        _bulletLeft = _maxStockBullet;
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
            CheckAndFlip();

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

    private void Shoot()
    {
        if (_bulletLeft <= 0)
            return;

        int directionFactor = 1;
        if (_isFlipped)
            directionFactor = -1;

        GameObject newBullet = Instantiate(_bullet_prefab, _bulletSpawnPoint.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector3(_bulletSpeed * directionFactor, 0, 0);
        _bulletLeft --;

        if (_bulletLeft <= 0)
            _reloadCoroutine = StartCoroutine(ShootReload(_reloadTime));
    }

    private void HorizontalMove(float direction)
    {
        _myRB.velocity = new Vector2(direction * _speed, _myRB.velocity.y);
    }

    private void StopMove() 
    {
        _myRB.velocity = new Vector2(0, _myRB.velocity.y);
    }

    private void CheckAndFlip()
    {
        if (_myRB.velocity.x > 0 && _isFlipped)
        {
            _isFlipped = false;
            _spriteRenderer.flipX = false;
        }
        else if (_myRB.velocity.x < 0 && _isFlipped == false)
        {
            _isFlipped = true;
            _spriteRenderer.flipX = true;
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

    private IEnumerator ShootReload(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        _bulletLeft = _maxStockBullet;
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
