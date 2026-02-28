using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlyInput : MonoBehaviour
{
    protected PlayerInput _playerInput;

    protected virtual void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        Initialize();
    }

    protected virtual void Initialize() { }

    protected virtual async void Start()
    {
        while (!ManagerBootstrapper.IsBootstrapped)
            await Task.Yield();

        await Manager.Game.WaitForReady();

        Manager.Game.IsGamePaused += OnGamePaused;
    }

    protected virtual void OnDisable()
    {
        Manager.Game.IsGamePaused -= OnGamePaused;
    }

    protected virtual void OnGamePaused()
    {
        if (Manager.Game.isGamePaused) DeactivateInput();
        else ActiveInput();
    }

    protected virtual void ActiveInput()
    {
        _playerInput.ActivateInput();
    }

    protected virtual void DeactivateInput()
    {
        _playerInput.DeactivateInput();
    }
}