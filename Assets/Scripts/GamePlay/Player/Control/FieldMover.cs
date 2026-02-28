using System;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Threading.Tasks;

public class FieldMover : PlyInput
{
    public event Action<float> OnRotate;

    [SerializeField] private float _rotateAngle;
    [SerializeField] private float _rotateDuration = 0.2f;

    private float _targetYRotation;

    protected override async void Start()
    {
        base.Start();

        while (!ManagerBootstrapper.IsBootstrapped)
            await Task.Yield();

        await Manager.Game.WaitForReady();

        _targetYRotation = transform.eulerAngles.y;
        _rotateAngle = 360 / Manager.Game.Direction;
    }

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        if (input.x > 0.1f)
        {
            Manager.Game.turnStack.Enqueue(
                new PlayerAction(
                    execute: () => Rotate(+_rotateAngle),
                    turnDelta: +1,
                    generateTick: true
                )
            );
        }
        else if (input.x < -0.1f && Manager.Game.turnStack.Current > 0)
        {
            Manager.Game.turnStack.Enqueue(
                new PlayerAction(
                    execute: () => Rotate(-_rotateAngle),
                    turnDelta: -1,
                    generateTick: false
                )
            );
        }

        Manager.Game.turnStack.Resolve();
    }
    private void Rotate(float deltaAngle)
    {
        _targetYRotation += deltaAngle;
        transform.DORotate(new Vector3(0, _targetYRotation, 0), _rotateDuration, RotateMode.Fast);
        OnRotate?.Invoke(deltaAngle);
    }
}
