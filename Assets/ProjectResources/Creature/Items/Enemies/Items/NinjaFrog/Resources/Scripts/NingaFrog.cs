using UnityEngine;

public class NingaFrog : Enemy
{
    public float ReloadTime { get => _reloadTime; set => _reloadTime = value; }

    [Header("NingaFrog settings")]
    [Header("Attack settings")]
    [SerializeField] private float _reloadTime;
    [SerializeField] private float _damageDistance;
    [SerializeField] private float _distanceToStartAttack;

    [Header("Vision settings")]
    [SerializeField] private float _visionDistance;

    private void Update()
    {
    }

    private void CheckPlayerInVision()
    {
        Ray2D leftVisionRay;
        Ray2D RightVisionRay;
    }
}