using System;
using UnityEngine;
public class EnemyController : MonoBehaviour 
{
    [SerializeField] private NPCMovement _movement;
    [SerializeField] private NPCVision _vision;
    [SerializeField] private CreatureHealth _health;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _attack_gameobject;

    [Header("Debug")]
    [SerializeField] private EnemyState _currentState = EnemyState.Idle;

    private Rigidbody2D _myRB;
    private bool _isFlipped;
    private IAttack _attack;

    private ShootAttack _shootAttack;

    private void Awake()
    {
        _myRB = this.GetComponent<Rigidbody2D>();
        _attack = _attack_gameobject.GetComponent<IAttack>();
        _attack.StartedAttack_notifier += StopMove;
        _attack.FinishedAttack_notifier += SetIdleState;

        if (_attack is ShootAttack)
        {
            _shootAttack = _attack as ShootAttack;
        }
    }

    private void Update()
    {
        if (_currentState == EnemyState.Attack)
        {
            return;
        }
        else
        {
            CheckAndFlip();
            bool run = false;

            if (_vision.CheckTriggersInVisionZone())
            {
                run = _movement.MoveToTarget(_vision.NearestDetectedTrigger.transform.position);

                if (_attack.CheckTargetsInAttackDistance() && _attack.IsReadyToAttack)
                {
                    CheckAndFlip();

                    if (_shootAttack != null)
                    {
                        _shootAttack.TargetPosition = _vision.NearestDetectedTrigger.transform.position;
                    }

                    _attack.Attack();

                    ChangeAnimation(EnemyState.Attack);
                    return;
                }
            }
            else
            {
                if (_vision.LastTargetPosition != Vector2.zero)
                {
                    run = _movement.MoveToTarget(_vision.LastTargetPosition);
                }
            }

            if (run)
            {
                ChangeAnimation(EnemyState.Run);
                return;
            }
            else
            {
                ChangeAnimation(EnemyState.Idle);
                return;
            }
        }
    }

    private void SetIdleState(object sender, EventArgs e)
    {
        ChangeAnimation(EnemyState.Idle);
    }

    private void ChangeAnimation(EnemyState state)
    {
        if (state == _currentState)
        {
            return;
        }

        _currentState = state;

        switch (_currentState)
        {
            case EnemyState.Idle:
                _animator.Play("Idle");
                break;
            case EnemyState.Run:
                _animator.Play("Run");
                break;
            case EnemyState.Attack:
                _animator.Play("Attack");
                break;
            default:
                break;
        }
    }

    private void CheckAndFlip()
    {
        if (_movement.LeftDirection == false && _isFlipped)
        {
            _isFlipped = false;
            _spriteRenderer.flipX = false;
        }
        else if (_movement.LeftDirection && _isFlipped == false)
        {
            _isFlipped = true;
            _spriteRenderer.flipX = true;
        }
    }

    private void StopMove(object sender, EventArgs e)
    {
        _movement.StopMove();
    }

    enum EnemyState
    {
        Idle,
        Run,
        Attack
    }
}