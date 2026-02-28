using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public struct FireSnapshot
{
    public Vector3 origin;
    public Vector3 direction;

    public FireSnapshot(Vector3 origin, Vector3 direction)
    {
        this.origin = origin;
        this.direction = direction;
    }
}

public class ShooterInput : PlyInput
{
    public event Action OnFire;

    [SerializeField] private GameObject _playerModel;

    [SerializeField] private float _firePointOffset = 1f;
    [SerializeField] private float _fireDistance = 50f;

    private InputAction _fireUpAction;
    private InputAction _fireDownAction;

    public bool IsInitialized { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Initialize()
    {
        _fireUpAction = _playerInput.actions["FireUp"];
        _fireDownAction = _playerInput.actions["FireDown"];
    }

    protected override void Start()
    {
        base.Start();
        _fireUpAction.performed += OnFireUp;
        _fireDownAction.performed += OnFireDown;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _fireUpAction.performed -= OnFireUp;
        _fireDownAction.performed -= OnFireDown;
    }

    protected override void ActiveInput()
    {
        _fireUpAction.Enable();
        _fireDownAction.Enable();
    }

    protected override void DeactivateInput()
    {
        _fireUpAction.Disable();
        _fireDownAction.Disable();
    }

    private void OnFireUp(InputAction.CallbackContext ctx)
    {
        Vector3 logicalDir = Vector3.forward;
        FireSnapshot snapshot = CreateSnapshot(logicalDir);

        _playerModel.transform.DORotate(new Vector3(0, 0, 0), 0.2f)
            .OnComplete(() => FireSequence(snapshot));
    }

    private void OnFireDown(InputAction.CallbackContext ctx)
    {
        Vector3 logicalDir = Vector3.back;
        FireSnapshot snapshot = CreateSnapshot(logicalDir);

        _playerModel.transform.DORotate(new Vector3(0, 180f, 0), 0.2f)
            .OnComplete(() => FireSequence(snapshot));
    }

    private FireSnapshot CreateSnapshot(Vector3 dir)
    {
        return new FireSnapshot(
            origin: transform.position + dir * _firePointOffset,
            direction: dir
        );
    }

    private void FireSequence(FireSnapshot snapshot)
    {
        OnFire?.Invoke();

        Manager.Game.turnStack.Enqueue(
            new PlayerAction(
                execute: () =>
                {
                    Fire(snapshot);
                },
                turnDelta: 0,
                generateTick: true
            )
        );

        Manager.Game.turnStack.Resolve();
    }

    private void Fire(FireSnapshot snapshot, float rate = 1)
    {
        int monsterLayer = LayerMask.GetMask("Monster");

        if (Physics.Raycast(
            snapshot.origin,
            snapshot.direction,
            out RaycastHit hit,
            _fireDistance,
            monsterLayer))
        {
            if (hit.collider.TryGetComponent(out Monster monster))
            {
                Manager.Status.ApplyPlayerAttack(monster, hit, rate);
            }
        }

        Debug.DrawRay(
            snapshot.origin,
            snapshot.direction * _fireDistance,
            Color.red,
            0.2f
        );
    }
}