using System.Collections.Generic;
using UnityEngine;

public class MovementCircularRoute : MonoBehaviour
{
    [SerializeField] private List<Transform> _routePoints;
    [SerializeField] private float _speed;

    [Header("Debug")]
    [SerializeField] private Transform _currentPoint;

    private int _currentPointIndex;

    private void Awake()
    {
        if (_routePoints == null)
        {
            Debug.LogError($"{this.gameObject.name} has no routePoints. Script \"{this.name}\" disabled.");
            this.enabled = false;
        }

        _currentPoint = _routePoints[0];
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(this.transform.position, _currentPoint.position) < 0.2f)
        {
            NextPoint();
        }
        else
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, _currentPoint.position, _speed * Time.deltaTime);
        }
    }

    private void NextPoint()
    {
        if (_currentPointIndex == _routePoints.Count - 1)
        {
            _currentPointIndex = 0;
        }
        else
        {
            _currentPointIndex ++;
        }

        _currentPoint = _routePoints[_currentPointIndex];
    }
}