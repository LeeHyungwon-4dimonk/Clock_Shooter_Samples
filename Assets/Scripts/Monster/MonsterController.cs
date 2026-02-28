using DG.Tweening;
using System;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public event Action<Collision> OnHit;
    public int DirectionIndex { get; private set; }
    public int DistanceStep { get; private set; }

    [SerializeField] private float _spawnRadius = 9f;
    [SerializeField] private int _arriveTurn = 7;
    [SerializeField] private float _yOffset = 1f;
    [SerializeField] private float _moveDuration = 0.2f;
    [SerializeField] private GameObject _monsterModel;

    private float _moveDistance;

    private void Awake()
    {

    }

    public void Initialize(int directionIndex)
    {
        transform.DOKill();

        DirectionIndex = directionIndex;
        DistanceStep = 0;
        _moveDistance = _spawnRadius / _arriveTurn;

        Manager.Game.monsterPositionManager.Register(this);
        SetMonsterLookDirection(directionIndex);
        UpdatePositionImmediate();
    }

    private void SetMonsterLookDirection(int index)
    {
        int yRotation = -90 - index * 45;
        if (yRotation <= -180) yRotation += 360;

        _monsterModel.transform.localRotation = Quaternion.Euler(0, yRotation, 0);
    }

    public int GetTargetStep(int delta)
    {
        if (delta > 0) return DistanceStep + 1;
        if (delta < 0) return Mathf.Max(0, DistanceStep - 1);
        return DistanceStep;
    }

    public void ApplyMove(int targetStep)
    {
        DistanceStep = targetStep;

        UpdatePosition();
    }

    private void UpdatePositionImmediate()
    {
        transform.localPosition = CalculatePosition();
    }

    private void UpdatePosition()
    {
        transform.DOKill();
        transform.DOLocalMove(CalculatePosition(), _moveDuration).SetEase(Ease.OutQuad);
    }

    private Vector3 CalculatePosition()
    {
        float angle = DirectionIndex * (360f / 8f) * Mathf.Deg2Rad;
        float radius = _spawnRadius - (DistanceStep * _moveDistance);

        return new Vector3(Mathf.Cos(angle) * radius, _yOffset, Mathf.Sin(angle) * radius);
    }    

    private void OnCollisionEnter(Collision collision)
    {
        OnHit?.Invoke(collision);
    }
}