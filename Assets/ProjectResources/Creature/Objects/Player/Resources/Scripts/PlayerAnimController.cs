using System;
using System.Collections;
using UnityEngine;
using static PlayerController;

public class PlayerAnimController : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private SpriteRenderer _player_spriteRenderer;
    [SerializeField] private float _flickerFrequency;

    private Animator _myAnimator;
    private PlayerAnimState _playerAnimState;
    private bool _noDamage;
    private Coroutine _noDamage_coroutine;
    private Coroutine _flickerNoDamage_coroutine;

    private void Awake()
    {
        _myAnimator = this.GetComponent<Animator>();
        _playerHealth.GetDamage_notifier += GetDamage;
    }

    public void ChangeAnimation(PlayerAnimState playerAnimState)
    {
        if (playerAnimState == _playerAnimState)
        {
            return;
        }

        switch (playerAnimState)
        {
            case PlayerAnimState.Idle:
                _myAnimator.Play("Idle");
                _playerAnimState = PlayerAnimState.Idle;
                break;
            case PlayerAnimState.Run:
                _myAnimator.Play("Run");
                _playerAnimState = PlayerAnimState.Run;
                break;
            case PlayerAnimState.Jump:
                _myAnimator.Play("Jump");
                _playerAnimState = PlayerAnimState.Jump;
                break;
            case PlayerAnimState.DoubleJump:
                _myAnimator.Play("DoubleJump");
                _playerAnimState = PlayerAnimState.DoubleJump;
                break;
            case PlayerAnimState.Fall:
                _myAnimator.Play("Fall");
                _playerAnimState = PlayerAnimState.Fall;
                break;
            case PlayerAnimState.Hit:
                _myAnimator.Play("Hit");
                _playerAnimState = PlayerAnimState.Hit;
                break;
            default:
                _myAnimator.Play("Idle");
                _playerAnimState = PlayerAnimState.Idle;
                break;
        }
    }

    private void GetDamage(object sender, EventArgs e)
    {
        _noDamage = true;

        _noDamage_coroutine = StartCoroutine(TimerNoDamage(_playerHealth.TimeNoDamage));
        _flickerNoDamage_coroutine = StartCoroutine(TimerFlicker(_flickerFrequency));
    }

    private IEnumerator TimerFlicker(float time)
    {
        while (_noDamage)
        {
            _player_spriteRenderer.color = new Color(1, 0.5f, 0.5f, 0.75f);
            yield return new WaitForSeconds(time);
            _player_spriteRenderer.color = new Color(1, 0, 0, 0.25f);
            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator TimerNoDamage(float time)
    {
        yield return new WaitForSeconds(time);
        _noDamage = false;
        StopCoroutine(_flickerNoDamage_coroutine);
        _player_spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}