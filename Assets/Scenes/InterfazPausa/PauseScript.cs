using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PauseScript : MonoBehaviour
{
    private VisualElement root;
    private VisualElement pauseContainer;
    private Button pauseButton;
    private InputSystem_Actions controls;
    private void Awake()
    {
        controls = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        pauseContainer = root.Q<VisualElement>("PauseContainer");
        pauseButton = root.Q<Button>("PauseButton");
        if (pauseButton != null) pauseButton.clicked += TogglePause;
        controls.UI.Pause.performed += EscapePause;
        if (pauseButton != null) pauseButton.focusable = false;
        controls.Enable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        if (pauseButton != null) pauseButton.clicked -= TogglePause;
        controls.UI.Pause.performed -= EscapePause;
        controls.Disable();
    }

    private void EscapePause(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    private void TogglePause()
    {
        bool isTitleVisible = pauseContainer != null && pauseContainer.resolvedStyle.display != DisplayStyle.None;

        if (pauseContainer != null) pauseContainer.style.display = isTitleVisible ? DisplayStyle.None : DisplayStyle.Flex;


        Debug.Log("pausa juego");
    }
}
