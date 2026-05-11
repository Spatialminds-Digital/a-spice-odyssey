using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : MonoBehaviour
{
    private ArcadeInputs _arcadeInputs;

    public static InputService Instance;

    public Action<float> OnMove;
    public Action OnInteract;

    public Action OnUIUp;
    public Action OnUIDown;
    public Action OnUISelect;
    public Action OnUIStart;

    private float _quitTimer;

    [SerializeField] private float QuitTime = 180;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);

        Instance = this;

        _arcadeInputs = new ArcadeInputs();
    }

    void OnEnable()
    {
        _arcadeInputs.Player.Move.performed += HandlePlayerMove;
        _arcadeInputs.Player.Interact.performed += HandlePlayerInteract;
        _arcadeInputs.UI.Select.performed += HandleUISelect;
        _arcadeInputs.UI.Up.performed += HandleUIUp;
        _arcadeInputs.UI.Down.performed += HandleUIDown;
        _arcadeInputs.UI.Quit.performed += HandleUIQuit;
        _arcadeInputs.UI.Start.performed += HandleUISelect;   

        _arcadeInputs.Player.Enable();
        _arcadeInputs.UI.Enable();
    }

    void OnDisable()
    {
        _arcadeInputs.Player.Move.performed -= HandlePlayerMove;
        _arcadeInputs.Player.Interact.performed -= HandlePlayerInteract;
        _arcadeInputs.UI.Select.performed -= HandleUISelect;
        _arcadeInputs.UI.Up.performed -= HandleUIUp;
        _arcadeInputs.UI.Down.performed -= HandleUIDown;
        _arcadeInputs.UI.Quit.performed -= HandleUIQuit;
        _arcadeInputs.UI.Start.performed -= HandleUISelect;   

        _arcadeInputs.Player.Disable();
        _arcadeInputs.UI.Disable();
    }

    private void HandlePlayerMove(InputAction.CallbackContext ctx)
    {
        _quitTimer = 0;
        OnMove?.Invoke(ctx.ReadValue<float>());
    }

    private void HandlePlayerInteract(InputAction.CallbackContext ctx)
    {
        _quitTimer = 0;
        OnInteract?.Invoke();
    }

    private void HandleUIUp(InputAction.CallbackContext ctx)
    {
        _quitTimer = 0;
        OnUIUp?.Invoke();
    }

    private void HandleUIDown(InputAction.CallbackContext ctx)
    {
        _quitTimer = 0;
        OnUIDown?.Invoke();
    }

    private void HandleUISelect(InputAction.CallbackContext ctx)
    {
        _quitTimer = 0;
        OnUISelect?.Invoke();
    }

    private void HandleUIStart(InputAction.CallbackContext ctx)
    {
        _quitTimer = 0;
        OnUIStart?.Invoke();
    }

    private void HandleUIQuit(InputAction.CallbackContext ctx)
    {
#if UNITY_STANDALONE_WIN || UNITY_ANDROID || UNITY_STANDALONE
        Application.Quit();
#endif
    }

    void Start()
    {
        _quitTimer = 0;
    }

    void Update()
    {
        QuitTimer();
    }

    private void QuitTimer()
    {
#if UNITY_STANDALONE_WIN || UNITY_ANDROID || UNITY_STANDALONE
        _quitTimer += Time.deltaTime;
        if (_quitTimer > 180)
        {
            Application.Quit();
        }
#endif
    }
}
