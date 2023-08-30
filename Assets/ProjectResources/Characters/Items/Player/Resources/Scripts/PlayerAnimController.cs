using UnityEngine;
using static PlayerController;

public class PlayerAnimController : MonoBehaviour
{
    private Animator _myAnimator;
    private PlayerAnimState _playerAnimState;

    private void Awake()
    {
        _myAnimator = this.GetComponent<Animator>();
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
}
