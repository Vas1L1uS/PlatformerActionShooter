using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NPCMovement : MonoBehaviour
{
    public event EventHandler Stopped_notifier;

    [SerializeField] private float _speed;
    [SerializeField] private Transform _leftStopPoint;
    [SerializeField] private Transform _rightStopPoint;

    private Rigidbody2D _myRB;

    private void Awake()
    {
        _myRB = this.GetComponent<Rigidbody2D>();
    }

    public bool MoveToTarget(Vector2 targetPosition)
    {
        int direction = 1;

        if (targetPosition.x < this.transform.position.x)
        {
            direction = -1;
            if (this.transform.position.x < _leftStopPoint.transform.position.x)
            {
                StopMove();
                return false;
            }
        }
        else
        {
            if (this.transform.position.x > _rightStopPoint.transform.position.x)
            {
                StopMove();
                return false;
            }
        }

        if (Math.Abs(targetPosition.x - this.transform.position.x) < 0.25f)
        {
            StopMove();
            return false;
        }

        _myRB.velocity = new Vector2(direction * _speed, _myRB.velocity.y);
        return true;
    }

    public void StopMove()
    {
        _myRB.velocity = new Vector2(0, _myRB.velocity.y);
        Stopped_notifier?.Invoke(this, EventArgs.Empty);
    }
}