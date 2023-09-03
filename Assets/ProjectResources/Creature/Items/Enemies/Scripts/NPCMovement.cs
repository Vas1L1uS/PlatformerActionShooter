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

    public void Move(float direction)
    {
        _myRB.velocity = new Vector2(direction * _speed, _myRB.velocity.y);
    }

    public bool CheckStopPoints()
    {
        if (_leftStopPoint != null)
        {
            if (this.transform.position.x < _leftStopPoint.transform.position.x)
            {
                return true;
            }
        }
        else if (_rightStopPoint != null)
        {
            if (this.transform.position.x > _rightStopPoint.transform.position.x)
            {
                return true;
            }
        }

        return false;
    }

    public void StopMove()
    {
        _myRB.velocity = new Vector2(0, _myRB.velocity.y);
        Stopped_notifier?.Invoke(this, EventArgs.Empty);
    }
}