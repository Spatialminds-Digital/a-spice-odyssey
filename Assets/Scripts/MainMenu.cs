using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public enum MenuState
    {
        Main,
        Intro,
        HowTo,
        Scores
    }

    public enum MenuButton
    {
        Play,
        HowTo,
        Scores
    }

    [Header("State GameObjects")]
    [SerializeField] private GameObject mainStateObject;
    [SerializeField] private GameObject introStateObject;
    [SerializeField] private GameObject howToStateObject;
    [SerializeField] private GameObject scoresStateObject;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button howToButton;
    [SerializeField] private Button scoresButton;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.3f;

    private InputService _inputService;
    private MenuState _currentState = MenuState.Main;
    private MenuButton _selectedButton = MenuButton.Play;

    private Button[] _buttons;

    void Start()
    {
        _inputService = InputService.Instance;
        _buttons = new Button[] { playButton, howToButton, scoresButton };

        SetState(MenuState.Main);
        UpdateButtonSelection();
    }

    void OnEnable()
    {
        if (_inputService == null)
            _inputService = InputService.Instance;

        if (_inputService != null)
        {
            _inputService.OnUIUp += HandleUp;
            _inputService.OnUIDown += HandleDown;
            _inputService.OnUISelect += HandleSelect;
            _inputService.OnUIStart += HandleStart;
        }
    }

    void OnDisable()
    {
        if (_inputService != null)
        {
            _inputService.OnUIUp -= HandleUp;
            _inputService.OnUIDown -= HandleDown;
            _inputService.OnUISelect -= HandleSelect;
            _inputService.OnUIStart -= HandleStart;
        }
    }

    private void HandleDown()
    {
        if (_currentState != MenuState.Main)
            return;

        int currentIndex = (int)_selectedButton;
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = 2;

        _selectedButton = (MenuButton)currentIndex;
        UpdateButtonSelection();
    }

    private void HandleUp()
    {
        if (_currentState != MenuState.Main)
            return;

        int currentIndex = (int)_selectedButton;
        currentIndex++;
        if (currentIndex > 2)
            currentIndex = 0;

        _selectedButton = (MenuButton)currentIndex;
        UpdateButtonSelection();
    }

    private void HandleSelect()
    {
        if (_currentState == MenuState.Main)
        {
            switch (_selectedButton)
            {
                case MenuButton.Play:
                    SetState(MenuState.Intro);
                    break;
                case MenuButton.HowTo:
                    SetState(MenuState.HowTo);
                    break;
                case MenuButton.Scores:
                    SetState(MenuState.Scores);
                    break;
            }
        }
        else if (_currentState != MenuState.Intro)
        {
            SetState(MenuState.Main);
        }
    }

    private void HandleStart()
    {
        HandleSelect();
    }

    private void SetState(MenuState newState)
    {
        _currentState = newState;

        if (mainStateObject != null) mainStateObject.SetActive(false);
        if (introStateObject != null) introStateObject.SetActive(false);
        if (howToStateObject != null) howToStateObject.SetActive(false);
        if (scoresStateObject != null) scoresStateObject.SetActive(false);

        GameObject targetObject = null;
        switch (newState)
        {
            case MenuState.Main:
                targetObject = mainStateObject;
                break;
            case MenuState.Intro:
                targetObject = introStateObject;
                break;
            case MenuState.HowTo:
                targetObject = howToStateObject;
                break;
            case MenuState.Scores:
                targetObject = scoresStateObject;
                break;
        }

        if (targetObject != null)
        {
            targetObject.SetActive(true);
            AnimateStateIn(targetObject);
        }

        if (newState == MenuState.Main)
        {
            UpdateButtonSelection();
        }
    }

    private void AnimateStateIn(GameObject stateObject)
    {
        stateObject.transform.localScale = Vector3.zero;
        stateObject.transform.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack);
    }

    private void UpdateButtonSelection()
    {
        int index = (int)_selectedButton;
        if (_buttons != null && index >= 0 && index < _buttons.Length && _buttons[index] != null)
        {
            EventSystem.current.SetSelectedGameObject(_buttons[index].gameObject);
        }
    }
}
